using DynDns53.Core;
using DynDns53.CoreLib;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public IConfigHandler ConfigHandler { get; set; }
        public bool ReloadConfig { get; set; }
        
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ConfigHandler == null) { throw new ArgumentNullException("ConfigHandler"); }

            var config = ConfigHandler.GetConfig();
            updateIntervalTextBox.Text = config.UpdateInterval.ToString();
            accessKeyTextBox.Text = config.Route53AccessKey;
            secretKeyTextBox.Text = config.Route53SecretKey;
            runAtStartCheckBox.IsChecked = config.RunAtSystemStart;
            config.DomainList.ForEach(d => domainsListView.Items.Add(d));

            ReloadConfig = false;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var newConfig = new DynDns53Config()
            {
                UpdateInterval = int.Parse(updateIntervalTextBox.Text),
                Route53AccessKey = accessKeyTextBox.Text,
                Route53SecretKey = secretKeyTextBox.Text,
                RunAtSystemStart = runAtStartCheckBox.IsChecked.Value
            };

            newConfig.DomainList = new List<HostedDomainInfo>();

            List<HostedDomainInfo> newList = domainsListView.Items.Cast<HostedDomainInfo>().ToList();

            newList.ToList().ForEach(li =>
           {
               newConfig.DomainList.Add(li);
           });

            ConfigHandler.SaveConfig(newConfig);

            ReloadConfig = true;

            Close();
        }

        private void deleteDomainButton_Click(object sender, RoutedEventArgs e)
        {
            domainsListView.Items.Remove(domainsListView.SelectedItem);
        }

        private void domainsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            deleteDomainButton.IsEnabled = !(domainsListView.SelectedItems.Count == 0);
        }

        private void addDomainButton_Click(object sender, RoutedEventArgs e)
        {
            AddDomainWindow addDomain = new AddDomainWindow();
            addDomain.ShowDialog();
            domainsListView.Items.Add(new HostedDomainInfo() { DomainName = addDomain.DomainName, ZoneId = addDomain.ZoneId });
        }
    }
}
