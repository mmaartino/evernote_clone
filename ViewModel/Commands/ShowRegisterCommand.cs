using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EverNoteClone.ViewModel.Commands
{
    public class ShowRegisterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public LoginVM ViewModel { get; set; }

        public ShowRegisterCommand(LoginVM vv)
        {
            ViewModel = vv;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.SwitchViews();
        }
    }
}
