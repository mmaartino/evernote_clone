using EverNoteClone.ViewModel;
using EverNoteClone.ViewModel.Helpers;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EverNoteClone.View
{
    /// <summary>
    /// Logika interakcji dla klasy NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        NotesVM ViewModel;
        public NotesWindow()
        {
            InitializeComponent();
            ViewModel = (NotesVM)Resources["vmmm"];
            var FontFamilies = Fonts.SystemFontFamilies.OrderBy(f=>f.Source);
            FontFamilyCombo.ItemsSource = FontFamilies;
            List<double> Sizes = new List<double>() {8,9,10,11,12,14,16,18,20,28,48,72 };
            FontSizeCombo.ItemsSource = Sizes;
            ViewModel.SelectedNoteChanged += ViewModel_SelectedNoteChanged;
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if(string.IsNullOrEmpty(App.UserId))
            {
                LoginWindow loginwindow = new LoginWindow();
                loginwindow.ShowDialog();
                ViewModel.GetNotebooks();
            }
        }

        private void ViewModel_SelectedNoteChanged(object sender, EventArgs e)
        {
            ContentOfRichTextbox.Document.Blocks.Clear();
            if (ViewModel.SelectedNote != null)
            {
                if (!string.IsNullOrEmpty(ViewModel.SelectedNote.FileLocation))
                {
                    FileStream Filestreamm = new FileStream(ViewModel.SelectedNote.FileLocation, FileMode.Open);
                    var Contents = new TextRange(ContentOfRichTextbox.Document.ContentStart, ContentOfRichTextbox.Document.ContentEnd);
                    Contents.Load(Filestreamm, DataFormats.Rtf);
                }
            }
        }

        private void Exitbtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void Speechbtn_click(object sender, RoutedEventArgs e)
        {
            string region = "westeurope";
            string key = "1be6464f739749d6808366aec7a04aef";
            var SpeechConfigg = SpeechConfig.FromSubscription(key,region);
            using (var audioconfigg = AudioConfig.FromDefaultMicrophoneInput())
            {
                using (var recognizer = new SpeechRecognizer(SpeechConfigg,"pl-PL",audioconfigg))
                {
                    var result = await recognizer.RecognizeOnceAsync();
                    ContentOfRichTextbox.Document.Blocks.Add(new Paragraph(new Run(result.Text)));
                }
            }

        }

        private void ContentOfRichTextboxChanged(object sender, TextChangedEventArgs e)
        {
            int AmountOfCharacters = (new TextRange(ContentOfRichTextbox.Document.ContentStart, ContentOfRichTextbox.Document.ContentEnd)).Text.Length;
            StatusBarTextBlock.Text = $"Document length: {AmountOfCharacters} characters.";
        }

        private void BoldBtn_click(object sender, RoutedEventArgs e)
        {
            bool buttonIsChecked = ((ToggleButton)sender).IsChecked??false;
            //var TextToBold=new TextRange(ContentOfRichTextbox.Selection.Start,ContentOfRichTextbox.Selection.End)
            if(buttonIsChecked)
                ContentOfRichTextbox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                ContentOfRichTextbox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
        }

        private void SelectionTextChanged(object sender, RoutedEventArgs e)
        {
            var SelectedWeight = ContentOfRichTextbox.Selection.GetPropertyValue(FontWeightProperty);
            Boldbtn.IsChecked = (SelectedWeight != DependencyProperty.UnsetValue) && (SelectedWeight.Equals(FontWeights.Bold));

            var SelectedStyle= ContentOfRichTextbox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            Italicbtn.IsChecked= (SelectedStyle != DependencyProperty.UnsetValue) && (SelectedStyle.Equals(FontStyles.Italic));

            var SelectedUnderline = ContentOfRichTextbox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            Underlinebtn.IsChecked= (SelectedUnderline != DependencyProperty.UnsetValue) && (SelectedUnderline.Equals(TextDecorations.Underline));

            FontFamilyCombo.SelectedItem = ContentOfRichTextbox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            FontSizeCombo.Text = (ContentOfRichTextbox.Selection.GetPropertyValue(Inline.FontSizeProperty)).ToString();

        }

        private void ItalicBtn_click(object sender, RoutedEventArgs e)
        {
            bool buttonIsChecked = ((ToggleButton)sender).IsChecked ?? false;
            if (buttonIsChecked)
                ContentOfRichTextbox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                ContentOfRichTextbox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
        }

        private void UnderlineBtn_click(object sender, RoutedEventArgs e)
        {
            bool buttonIsChecked = ((ToggleButton)sender).IsChecked ?? false;
            if (buttonIsChecked)
                ContentOfRichTextbox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
            {
                TextDecorationCollection textDecorations;
                ((TextDecorationCollection)ContentOfRichTextbox.Selection.GetPropertyValue(Inline.TextDecorationsProperty)).TryRemove(TextDecorations.Underline,out textDecorations);
                ContentOfRichTextbox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void SelectionChangedComboFontFamily(object sender, SelectionChangedEventArgs e)
        {
            if(FontFamilyCombo.SelectedItem!=null)
            {
                ContentOfRichTextbox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyCombo.SelectedItem);
            }
        }
        private void ValueChangedComboFontSize(object sender, TextChangedEventArgs e)
        {
            ContentOfRichTextbox.Selection.ApplyPropertyValue(Inline.FontSizeProperty,FontSizeCombo.Text);
        }

        private void Savebtn_click(object sender, RoutedEventArgs e)
        {
            string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory,$"{ViewModel.SelectedNote.Id}.rtf");
            ViewModel.SelectedNote.FileLocation = rtfFile;
            DatabaseHelper.Update(ViewModel.SelectedNote);

            FileStream Filestream = new FileStream(rtfFile,FileMode.Create);
            var Contents = new TextRange(ContentOfRichTextbox.Document.ContentStart, ContentOfRichTextbox.Document.ContentEnd);
            Contents.Save(Filestream, DataFormats.Rtf);
        }
    }
}
