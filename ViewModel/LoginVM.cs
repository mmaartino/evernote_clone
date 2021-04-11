using EverNoteClone.Model;
using EverNoteClone.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace EverNoteClone.ViewModel
{
    public class LoginVM:INotifyPropertyChanged
    {

        private User user;
        private bool IsShowingRegister = false;
        private string username;

        public string UserName
        {
            get { return username; }
            set 
            { 
                username = value;
                User = new User
                {
                    UserName = username,
                    Password = this.Password

                };
                OnPropertyChanged("UserName");
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                User = new User
                {
                    UserName = this.UserName,
                    Password = password

                };
                OnPropertyChanged("Password");
            }
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        private Visibility loginvis;

        public event PropertyChangedEventHandler PropertyChanged;

        public Visibility Loginvis
        {
            get { return loginvis; }
            set 
            { 
                loginvis = value;
                OnPropertyChanged("Loginvis");
            }
        }
        private Visibility registervis;
        public Visibility Registervis
        {
            get { return registervis; }
            set
            {
                registervis = value;
                OnPropertyChanged("Registervis");
            }
        }
        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        public ShowRegisterCommand ShowRegisterCommand { get; set; }

        public LoginVM()
        {
            Loginvis = Visibility.Visible;
            Registervis = Visibility.Collapsed;
            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
            ShowRegisterCommand = new ShowRegisterCommand(this);
            User = new User();
        }
        public void SwitchViews()
        {
            IsShowingRegister = !IsShowingRegister;

            if(IsShowingRegister)
            {
                Registervis = Visibility.Visible;
                Loginvis = Visibility.Collapsed;

            }
            else 
            {
                Registervis = Visibility.Collapsed;
                Loginvis = Visibility.Visible;

            }
        }
        public void Login()
        {

        }
        public void Register()
        {

        }
        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
