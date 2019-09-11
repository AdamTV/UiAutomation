using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using static UiaImplementer.Program;

namespace WpfUiaPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AutomationProperties.SetAutomationId(Tname, "ID_NAME");
            AutomationProperties.SetAutomationId(Tage, "ID_AGE");
            AutomationProperties.SetAutomationId(Temail, "ID_EMAIL");
            //string path = @"C:\Users\adams\source\repos\WpfUiaPractice\UiaImplementer\bin\Debug\UiaImplementer.dll";
            // Process.Start(path);
            Main(null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(@"C:\Users\adams\source\repos\WpfUiaPractice\Person.xml");
            //var XmlNodeList = xmlDoc.SelectNodes("/Person");
            var xn = xmlDoc.SelectSingleNode("//person");
            //foreach (XmlNode xn in XmlNodeList)
            //{
            string name = xn["name"].InnerText;
            string age = xn["age"].InnerText;
            string email = xn["email"].InnerText;
            //}
            Tname.Text = name;
            Tage.Text = age;
            Temail.Text = email;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Tname.Text = "";
            Tage.Text = "";
            Temail.Text = "";
        }
    }
}
