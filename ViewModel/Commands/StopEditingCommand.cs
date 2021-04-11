using EverNoteClone.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EverNoteClone.ViewModel.Commands
{
    public class StopEditingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public NotesVM ViewModel { get; set; }

        public StopEditingCommand(NotesVM vmm)
        {
            ViewModel = vmm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Notebook pp = (Notebook)parameter;
            if(pp!=null)
            {
                ViewModel.StopEditing(pp);
            }
            
        }
    }
}
