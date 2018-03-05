using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DynDns53.UI
{
    /// <summary>
    /// Interaction logic for AddDomain.xaml
    /// </summary>
    public partial class AddDomainWindow : Window
    {
        public string DomainName { get; set; }
        public string ZoneId { get; set; }

        public AddDomainWindow()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            DomainName = domainNameTextBox.Text;
            ZoneId = zoneIdTextBox.Text;
            Close();
        }
    }
}
