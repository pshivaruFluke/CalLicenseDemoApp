﻿using CalLicenseDemo.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalLicenseDemo.Views
{
    /// <summary>
    /// Interaction logic for UserAccountSettings.xaml
    /// </summary>
    public partial class UserAccountSettings : Page
    {
        public UserAccountSettings()
        {
            InitializeComponent();
            var viewModel = new UserAccountSettingsViewModel();
            DataContext = viewModel;
            viewModel.NavigateNextPage = OnNextPageNavigated;
        }

        private void OnNextPageNavigated(string screenName, Dictionary<string,string> additionalInfo)
        {
            this.NavigationService.Navigate(new Dashboard());
        }
    }
}