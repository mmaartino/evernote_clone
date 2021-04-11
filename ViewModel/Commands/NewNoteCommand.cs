using EverNoteClone.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EverNoteClone.ViewModel.Commands
{
    public class NewNoteCommand : ICommand
    {
        public NotesVM VM { get; set; }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public NewNoteCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            Notebook SelectedNotebook = (Notebook)parameter;
            if(SelectedNotebook!=null)
            {
                return true;
            }
            return false;
            
        }

        public void Execute(object parameter)
        {
            Notebook SelectedNotebook = (Notebook)parameter;
            VM.CreateNote(SelectedNotebook.Id);
        }
    }
}
