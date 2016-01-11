using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace IboodDailyNotifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string iBoodXmlUrl;
        Dictionary<string, string> arguments = new Dictionary<string, string>();
        public MainWindow()
        {
            InitializeComponent();
            string[] args = Environment.GetCommandLineArgs();

            for (int index = 1; index < args.Length; index += 2)
            {
                string arg = args[index].Replace("-", "");
                arguments.Add(arg, args[index + 1]);
            }

            if (arguments.ContainsKey("SilentNow"))
            {
                if (arguments["SilentNow"] == "true")
                {
                    LoadSettingsForConsole();
                    CheckIboodDeals();
                    Application.Current.Shutdown();
                }
            }
            LoadSettingsForApp();
        }

        private void LoadSettingsForApp()
        {
            try
            {
                IFTTT_eventname_txt.Text = Properties.Settings.Default.IFTTT_eventName;
                IFTTT_key_txt.Text = Properties.Settings.Default.IFTTT_key;
                LoadSettingsForConsole();
                //fill the combobox with countries
                FillComboboxCountry();
                //fill keywords in textbox
                FillTextboxKeywords();
                // Set changes as saved
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
                Properties.Settings.Default.IFTTT_key = IFTTT_key_txt.Text;
                Properties.Settings.Default.IFTTT_eventName = IFTTT_eventname_txt.Text;
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
            else
                CheckIboodDeals();
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
                List<item> PushProducts = new List<item>();
                foreach (var itemOnline in productList)
                {
                    foreach (var searchItem in Properties.Settings.Default.Keywords)
                    {
                        if (itemOnline.title.ToLower().Contains(searchItem.ToLower()))
                        {
                            //MessageBox.Show("Found the following item: \n"+itemOnline.title);
                            PushProducts.Add(itemOnline);
                            break;
                        }
                    }
                }
                if (PushProducts != null)
                {
                    sendToIFTTT(PushProducts);
                }
                else
                {
                    if (arguments.ContainsKey("SilentNow"))
                    {
                        if (arguments["SilentNow"] != "true")
                        {
                            MessageBox.Show("Our job is done!\nWe have checked all the items but nothing found!");
                        }
                    }
                    else
                        MessageBox.Show("Our job is done!\nWe have checked all the items but nothing found!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Deals of today cannot be loaded! \nTry again later.");
                throw;
            }
        }

        private void sendToIFTTT(List<item> pushProducts)
        {
            try
            {
                if (String.IsNullOrEmpty(Properties.Settings.Default.IFTTT_eventName)&&String.IsNullOrEmpty(Properties.Settings.Default.IFTTT_key))
                    throw new Exception("IFTTT Key or eventname not filled in!");
                foreach (var product in pushProducts)
                {
                    var url = "https://maker.ifttt.com/trigger/" + Properties.Settings.Default.IFTTT_eventName + "/with/key/" + Properties.Settings.Default.IFTTT_key;
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            value1 = product.title,
                            value2 = product.link
                        });

                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        if (!result.StartsWith("Congratulations"))
                        {
                            MessageBox.Show("Mislukt. Foutinformatie:" + result);
                        }
                    }


                    //using (var client = new System.Net.WebClient())
                    //{

                    //    var url = "https://maker.ifttt.com/trigger/" + Properties.Settings.Default.IFTTT_eventName + "/with/key/" + Properties.Settings.Default.IFTTT_key + "?value1=" + product.title + "?value2=" + product.link + "?value3=" + product.description;
                    //    string result = client.DownloadString(new Uri(url));
                    //    if (!result.StartsWith("Congratulations"))
                    //    {
                    //        MessageBox.Show("Mislukt. Foutinformatie:" + result);
                    //    }
                    //}
                }
                if (arguments.ContainsKey("SilentNow"))
                {
                    if (arguments["SilentNow"] != "true")
                    {
                        MessageBox.Show("Items will be send to you!");
                    }
                }
                else
                    MessageBox.Show("Items will be send to you!");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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

        private void IFTTT_key_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangesMade();
        }

        private void IFTTT_eventname_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangesMade();
        }
    }
}
