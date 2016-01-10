using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace IboodDailyNotifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string iBoodXmlUrl;
        public MainWindow()
        {
            InitializeComponent();
            LoadSettingsForApp();
        }

        private void LoadSettingsForApp()
        {
            try
            {
                EmailReciever.Text = Properties.Settings.Default.EmailTo;
                smtp_host_txt.Text = Properties.Settings.Default.SMTP_Server;
                smtp_username_txt.Text = Properties.Settings.Default.SMTP_Username;
                if (Properties.Settings.Default.SMTP_Port != 0)
                {
                    smtp_port_txt.Text = Convert.ToString(Properties.Settings.Default.SMTP_Port);
                }
                smtp_emailfrom.Text = Properties.Settings.Default.EmailFrom;
                LoadSettingsForConsole();
                //fill the combobox with countries
                FillComboboxCountry();
                //fill keywords in textbox
                FillTextboxKeywords();
                ChangesSaved();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load Settings");
                throw;
            }
        }

        private void ChangesMade()
        {
            SaveStatus.Content = "You've made changes!";
            SaveStatus.Background = Brushes.OrangeRed;
        }

        private void ChangesSaved()
        {
            SaveStatus.Content = "Changes Saved!";
            SaveStatus.Background = Brushes.GreenYellow;
        }

        private void LoadSettingsForConsole()
        {
            //put countries in combobox
            if (String.IsNullOrEmpty(Properties.Settings.Default.IboodUrl))
            {
                Properties.Settings.Default.IboodUrl = "http://feeds.ibood.com/rss/";
            }
            if (String.IsNullOrEmpty(Properties.Settings.Default.IboodCountry))
            {
                Properties.Settings.Default.IboodCountry = "be";
            }

            //build the url
            buildIboodUrl();
        }

        private void FillTextboxKeywords()
        {
            try
            {
                if (Properties.Settings.Default.Keywords != null)
                {
                    string toTxtB = "";
                    foreach (var keyW in Properties.Settings.Default.Keywords)
                    {
                        if (String.IsNullOrEmpty(toTxtB))
                            toTxtB = keyW;
                        else
                            toTxtB += "\n" + keyW;
                    }
                    KeywordBox.Text = toTxtB;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillComboboxCountry()
        {
            var allCountries = new List<Tuple<string, string>>();
            allCountries.Add(new Tuple<string, string>("België", "be"));
            allCountries.Add(new Tuple<string, string>("Nederland", "nl"));
            allCountries.Add(new Tuple<string, string>("Deutschland", "de"));
            allCountries.Add(new Tuple<string, string>("Österreich", "at"));
            allCountries.Add(new Tuple<string, string>("United Kingdom", "uk"));
            allCountries.Add(new Tuple<string, string>("Ireland", "ie"));
            allCountries.Add(new Tuple<string, string>("Polska", "pl"));
            for (int i = 0; i < allCountries.Count - 1; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Text = allCountries[i].Item1.ToString();
                item.Value = allCountries[i].Item2.ToString();
                CountryComboBox.Items.Add(item);
            }

            foreach (var country in CountryComboBox.Items)
            {
                if ((country as ComboBoxItem).Value.ToString() == Properties.Settings.Default.IboodCountry)
                {
                    CountryComboBox.SelectedItem = country;
                }
            }
            //MessageBox.Show((CountryComboBox.SelectedItem as ComboBoxItem).Value.ToString());
        }

        private void buildIboodUrl()
        {
            try
            {
                iBoodXmlUrl = Properties.Settings.Default.IboodUrl + Properties.Settings.Default.IboodCountry + ".xml";
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot build Url... \nTry other parameters.");
                throw;
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.IboodCountry = (CountryComboBox.SelectedItem as ComboBoxItem).Value.ToString();
                Properties.Settings.Default.EmailTo = EmailReciever.Text;
                Properties.Settings.Default.EmailFrom = smtp_emailfrom.Text;
                Properties.Settings.Default.SMTP_Port = Convert.ToInt16(smtp_port_txt.Text);
                Properties.Settings.Default.SMTP_Password = smtp_pass_txt.Password;
                Properties.Settings.Default.SMTP_Username = smtp_username_txt.Text;
                Properties.Settings.Default.SMTP_Server = smtp_host_txt.Text;
                if (!String.IsNullOrWhiteSpace(KeywordBox.Text))
                {
                    StringCollection result = new StringCollection();
                    string[] tempRes = KeywordBox.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string keyW in tempRes)
                    {
                        result.Add(keyW);
                    }
                    Properties.Settings.Default.Keywords = result;
                }
                else
                    Properties.Settings.Default.Keywords = null;

                Properties.Settings.Default.Save();
                ChangesSaved();
            }
            catch (Exception)
            {
                MessageBox.Show("Changes could not be saved.");
                throw;
            }
        }

        private void IboodNow_Click(object sender, RoutedEventArgs e)
        {
            string StatusCnt = (string)SaveStatus.Content;
            if (StatusCnt.Contains("made"))
            {
                MessageBoxResult dialogResult = MessageBox.Show("Do you want to save the changes you have made?", "Changes detected", MessageBoxButton.YesNoCancel);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    SaveChanges_Click(sender,e);
                    CheckIboodDeals();
                }
                else if (dialogResult == MessageBoxResult.No)
                {
                    CheckIboodDeals();
                }
            }
            
        }

        public void CheckIboodDeals()
        {
            try
            {
                LoadSettingsForConsole();
                // https://msdn.microsoft.com/en-us/library/dc0c9ekk(v=vs.110).aspx

                XmlDocument doc = new XmlDocument();
                doc.Load(iBoodXmlUrl);
                List<item> productList = new List<item>();
                XmlNodeList elemList = doc.GetElementsByTagName("item");
                for (int i = 0; i < elemList.Count; i++)
                {
                    //search with link for sertain attributes
                    string addedTags = "<item>" + elemList[i].InnerXml + "</item>";
                    XmlSerializer serializer = new XmlSerializer(typeof(item));
                    using (StringReader reader = new StringReader(addedTags))
                    {
                        productList.Add((item)(serializer.Deserialize(reader)));
                    }
                }
                List<item> EmailProducts = new List<item>();
                foreach (var itemOnline in productList)
                {
                    foreach (var searchItem in Properties.Settings.Default.Keywords)
                    {
                        if (itemOnline.title.ToLower().Contains(searchItem.ToLower()))
                        {
                            // MessageBox.Show("Found the following item: \n"+itemOnline.title);
                            EmailProducts.Add(itemOnline);
                            break;
                        }
                    }
                }
                sendEmail(EmailProducts);
            }
            catch (Exception)
            {
                MessageBox.Show("Deals of today cannot be loaded! \nTry again later.");
                throw;
            }
        }

        private void sendEmail(List<item> listToEmail)
        {
            try
            {
                if (!String.IsNullOrEmpty(Properties.Settings.Default.EmailTo)&&
                    !String.IsNullOrEmpty(Properties.Settings.Default.EmailFrom)&&
                    !String.IsNullOrEmpty(Properties.Settings.Default.SMTP_Server)&&
                    !String.IsNullOrEmpty(Properties.Settings.Default.SMTP_Username) &&
                    !String.IsNullOrEmpty(Properties.Settings.Default.SMTP_Password) &&
                    Properties.Settings.Default.SMTP_Port != 0)
                {
                    MailMessage mail = new MailMessage(Properties.Settings.Default.EmailFrom, Properties.Settings.Default.EmailTo);
                    SmtpClient client = new SmtpClient();
                    client.Port = Properties.Settings.Default.SMTP_Port;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = Properties.Settings.Default.SMTP_Server;
                    client.EnableSsl = true;
                    client.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.SMTP_Username, Properties.Settings.Default.SMTP_Password);
                    mail.Subject = "We found something for you!";
                    mail.Body = "Test";
                    client.Send(mail);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void KeywordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangesMade();
        }

        private void EmailReciever_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangesMade();
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangesMade();
        }

        private void smtp_port_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangesMade();
        }

        private void smtp_host_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangesMade();
        }

        private void smtp_username_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangesMade();
        }

        private void smtp_pass_txt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ChangesMade();
        }

        private void smtp_emailfrom_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
