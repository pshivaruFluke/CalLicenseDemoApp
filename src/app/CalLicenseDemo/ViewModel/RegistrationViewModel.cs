﻿using System.Windows.Input;
using CalLicenseDemo.Logic;
using CalLicenseDemo.Model;

namespace CalLicenseDemo.ViewModel
{
    internal class RegistrationViewModel : BaseEntity
    {
        private readonly User user;

        public RegistrationViewModel()
        {
            user = new User();
            RegterCommand = new RelayCommand(RegisterNewUser);
        }

        public string FName
        {
            get { return user.FName; }
            set
            {
                user.FName = value;
                OnPropertyChanged("FName");
            }
        }

        public string LName
        {
            get { return user.LName; }
            set
            {
                user.LName = value;
                OnPropertyChanged("LName");
            }
        }

        public string Email
        {
            get { return user.Email; }
            set
            {
                user.Email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Password
        {
            get { return user.Password; }
            set
            {
                user.Password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Organization
        {
            get { return user.Organization.Name; }
            set
            {
                user.Organization.Name = value;
                OnPropertyChanged("Organization");
            }
        }


        private ICommand RegterCommand { get; set; }

        private void RegisterNewUser(object param)
        {
            var logic = new LicenseLogic {User = user};
            var status = logic.CreateUserInfo();
        }
    }
}