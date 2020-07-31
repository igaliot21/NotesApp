using NotesApp.Model;
using NotesApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NotesApp.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        SpeechRecognitionEngine speechRecognizer;
        NotesVM notesVM;
        public NotesWindow()
        {

            InitializeComponent();
            notesVM = new NotesVM();
            mainContainer.DataContext = notesVM;
            notesVM.SelectedNoteChanged += NotesVM_SelectedNoteChanged;
            /*
            var currentCulture = from r in SpeechRecognitionEngine.InstalledRecognizers()
                                 where r.Culture.Equals(Thread.CurrentThread.CurrentCulture)
                                 select r;
            */
            var currentCulture = SpeechRecognitionEngine.InstalledRecognizers()
                                                        .Where(r => r.Culture.Equals(Thread.CurrentThread.CurrentUICulture))
                                                        .FirstOrDefault();
            speechRecognizer = new SpeechRecognitionEngine(currentCulture);
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.AppendDictation();

            speechRecognizer.LoadGrammar(new Grammar(grammarBuilder));
            speechRecognizer.SetInputToDefaultAudioDevice();
            speechRecognizer.SpeechRecognized += SpeechRecognizer_SpeechRecognized;

            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 28, 48, 72 };

        }

        private void NotesVM_SelectedNoteChanged(object sender, EventArgs e)
        {
            richTxtContent.Document.Blocks.Clear();
            if ((!string.IsNullOrEmpty(notesVM.SelectedNote.FileLocation) && notesVM.SelectedNote.FileLocation != "FileLocation")) {
                FileStream fileStream = new FileStream(notesVM.SelectedNote.FileLocation, FileMode.Open);
                TextRange range = new TextRange(richTxtContent.Document.ContentStart, richTxtContent.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }    
        }

        protected override void OnActivated(EventArgs e)
        {
            if (App.UserId == 0)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                notesVM.ReadNotebooks();
            }
            base.OnActivated(e);

        }

        private void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            richTxtContent.Document.Blocks.Add(new Paragraph(new Run(e.Result.Text)));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void toolBarBtnSpeech_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton).IsChecked ?? false) 
                speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
            else 
                speechRecognizer.RecognizeAsyncStop();
        }

        private void richTxtContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            int numCharacters = (new TextRange(richTxtContent.Document.ContentStart, richTxtContent.Document.ContentEnd)).Text.Length;
            txtStatus.Text = $"Document lenght: {numCharacters} characters";
        }

        private void toolBarBtnBold_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton).IsChecked ?? false)
                richTxtContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty,FontWeights.Bold);
            else
                richTxtContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
        }

        private void toolBarBtnItalic_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton).IsChecked ?? false)
                richTxtContent.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                richTxtContent.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
        }

        private void toolBarBtnUnderline_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton).IsChecked ?? false)
                richTxtContent.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
                richTxtContent.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        }

        private void richTxtContent_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedBoldState = richTxtContent.Selection.GetPropertyValue(Inline.FontWeightProperty);
            toolBarBtnBold.IsChecked = (selectedBoldState != DependencyProperty.UnsetValue) && (selectedBoldState.Equals(FontWeights.Bold));
            
            var selectedItalicState = richTxtContent.Selection.GetPropertyValue(Inline.FontStyleProperty);
            toolBarBtnItalic.IsChecked = (selectedItalicState != DependencyProperty.UnsetValue) && (selectedItalicState.Equals(FontStyles.Italic));

            var selectedUndelineState = richTxtContent.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            toolBarBtnUnderline.IsChecked = (selectedUndelineState != DependencyProperty.UnsetValue) && (selectedUndelineState.Equals(TextDecorations.Underline));

            cmbFontFamily.SelectedItem = richTxtContent.Selection.GetPropertyValue(Inline.FontFamilyProperty);

            cmbFontSize.Text = (richTxtContent.Selection.GetPropertyValue(Inline.FontSizeProperty)).ToString();

        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null) 
                richTxtContent.Selection.ApplyPropertyValue(Inline.FontFamilyProperty,cmbFontFamily.SelectedItem);    
        }

        private void cmbFontSize_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbFontSize.Text))
                richTxtContent.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        private void btnSaveNote_Click(object sender, RoutedEventArgs e)
        {
            string rtfFile = string.Empty;
            if (notesVM.SelectedNote.FileLocation == "FileLocation"){
                rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, $"{notesVM.SelectedNote.Id}.rtf");
                notesVM.SelectedNote.FileLocation = rtfFile;
            }
            else rtfFile = notesVM.SelectedNote.FileLocation;

            var fileStream = new FileStream(rtfFile, FileMode.Create);
            var range = new TextRange(richTxtContent.Document.ContentStart, richTxtContent.Document.ContentEnd);
            range.Save(fileStream, DataFormats.Rtf);

            notesVM.UpdatedSelectedNote();
        }
    }
}
