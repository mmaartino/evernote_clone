using EverNoteClone.Model;
using EverNoteClone.ViewModel.Commands;
using EverNoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace EverNoteClone.ViewModel
{
    public class NotesVM:INotifyPropertyChanged
    {

        public ObservableCollection<Notebook> Notebooks { get; set; }
        
        public ObservableCollection<Note> Notes { get; set; }


        private Notebook selectednotebook;
        public Notebook SelectedNotebook
        {
            get { return selectednotebook; }
            set 
            { 
                selectednotebook = value;
                OnPropertyChanged("SelectedNotebook");
                GetNotes();
            }
        }
        private Note selectedNote;

        public Note SelectedNote
        {
            get { return selectedNote; }
            set 
            { 
                selectedNote = value;
                OnPropertyChanged("SelectedNote");
                SelectedNoteChanged?.Invoke(this,new EventArgs());
            }
        }

        private Visibility IsVisible;

        public Visibility _IsVisible
        {
            get { return IsVisible; }
            set
            { 
                IsVisible = value;
                OnPropertyChanged("_IsVisible");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectedNoteChanged;

        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public EditCommand EditCommand { get; set; }
        public StopEditingCommand StopEditingCommand { get; set; }

        public NotesVM()
        {
            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            EditCommand = new EditCommand(this);
            StopEditingCommand = new StopEditingCommand(this);
            _IsVisible = Visibility.Collapsed;
            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();
            GetNotebooks();
        }

        public void CreateNotebook()
        {
            Notebook NewNotebook = new Notebook()
            {
                Name="New Notebook"


            };
            DatabaseHelper.Insert(NewNotebook);
            GetNotebooks();
        }
        public void CreateNote(int Notebookid)
        {
            Note NewNote = new Note()
            {
                NotebookId = Notebookid,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title="New note"
            };

            DatabaseHelper.Insert(NewNote);
            GetNotes();
        }
        public void GetNotebooks()
        {
           var notebooks=DatabaseHelper.Read<Notebook>();
            Notebooks.Clear();
            foreach(Notebook notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }
        private void GetNotes()
        {
            if (SelectedNotebook != null)
            {
                var notes = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == SelectedNotebook.Id).ToList();
                Notes.Clear();
                foreach (Note note in notes)
                {
                    Notes.Add(note);
                }
            }
        }
        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(PropertyName));
        }


        public void StartEditing()
        {
            _IsVisible = Visibility.Visible;
        }
        public void StopEditing(Notebook notebook)
        {
            _IsVisible = Visibility.Collapsed;
            DatabaseHelper.Update(notebook);
            GetNotebooks();
        }
    }

}

