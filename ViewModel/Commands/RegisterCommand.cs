using EverNoteClone.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EverNoteClone.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }

        public event EventHandler CanExecuteChanged;


        public RegisterCommand(LoginVM VM)
        {
            this.VM = VM;

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
            if (string.IsNullOrEmpty(user.ConfirmPassword))
                return false;
            if (user.Password != user.ConfirmPassword)
                return false;
            return true;
        }

        public void Execute(object parameter)
        {
            VM.Register();
        }
    }
}
