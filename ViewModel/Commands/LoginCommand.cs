using EverNoteClone.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EverNoteClone.ViewModel.Commands
{
    public class LoginCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public LoginVM VM { get; set; }

        public LoginCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            
            User user = (User)parameter;
            if (user == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(user.UserName))
                return false;
            if (string.IsNullOrEmpty(user.Password))
                return false;
            return true;
        }

        public void Execute(object parameter)
        {
            VM.Login();
        }
    }
}
