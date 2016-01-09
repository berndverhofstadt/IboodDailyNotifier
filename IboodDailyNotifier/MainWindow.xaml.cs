using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
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
                LoadSettingsForConsole();
                //fill the combobox with countries
                FillComboboxCountry();
                //fill keywords in textbox
                FillTextboxKeywords();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load Settings");
                throw;
            }
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
            Properties.Settings.Default.IboodCountry = (CountryComboBox.SelectedItem as ComboBoxItem).Value.ToString();
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
        }

        private void IboodNow_Click(object sender, RoutedEventArgs e)
        {
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

                XmlNodeList elemList = doc.GetElementsByTagName("item");
                for (int i = 0; i < elemList.Count; i++)
                {
                    //search with link for sertain attributes

                    MessageBox.Show(elemList[i].InnerXml);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Deals of today cannot be loaded! \nTry again later.");
                throw;
            }
        }
    }
}
