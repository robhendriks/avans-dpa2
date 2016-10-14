using DPA_Musicsheets.Editor;
using DPA_Musicsheets.Editor.Command;
using DPA_Musicsheets.LilyPond;
using DPA_Musicsheets.Music;
using DPA_Musicsheets.Utility;
using GalaSoft.MvvmLight;
using PSAMControlLibrary;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public EditableBase Editable { get; set; }
        public Editor.Editor Editor { get; set; }

        public Command NewCommand { get; set; }
        public Command OpenCommand { get; set; }
        public Command SaveCommand { get; set; }
        public Command ExportCommand { get; set; }
        public Command UndoCommand { get; set; }
        public Command RedoCommand { get; set; }
        public Command InsertCommand { get; set; }
        public Command ExitCommand { get; set; }

        public readonly EditorStateContext StateContext;

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _lilyPondSource;

        public string LilyPondSource
        {
            get { return _lilyPondSource; }
            set
            {
                _lilyPondSource = value;
                RaisePropertyChanged(() => LilyPondSource);
            }
        }

        private bool _isValidLilyPond;

        public bool IsValidLilyPond
        {
            get { return _isValidLilyPond; }
            set
            {
                _isValidLilyPond = value;
                RaisePropertyChanged(() => IsValidLilyPond);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private ObservableCollection<MusicalSymbol> _musicalSymbols = new ObservableCollection<MusicalSymbol>();

        public ObservableCollection<MusicalSymbol> MusicalSymbols
        {
            get { return _musicalSymbols; }
            set { _musicalSymbols = value; }
        }

        public bool CanEdit => StateContext.CanEdit();

        public MainViewModel()
        {
            Editable = new EditableBase(this);
            StateContext = new EditorStateContext(Editable);

            ExportCommand = new ExportCommand(Editable);
            InsertCommand = new InsertCommand(Editable);
            NewCommand = new NewCommand(Editable);
            OpenCommand = new OpenCommand(Editable);
            RedoCommand = new RedoCommand(Editable);
            SaveCommand = new SaveCommand(Editable);
            UndoCommand = new UndoCommand(Editable);
            ExitCommand = new ExitCommand(Editable);

            Editor = new Editor.Editor(NewCommand, OpenCommand, SaveCommand, ExportCommand,
                UndoCommand, RedoCommand, InsertCommand, ExitCommand);
        }

        public void LoadMidi()
        {
            MusicalSymbols.Clear();
            LilyPondSource = "";

            var reader = new MidiReader(FileName);

            MusicalSymbols.Add(new Clef(ClefType.GClef, 2));
            MusicalSymbols.Add(new PSAMControlLibrary.TimeSignature(TimeSignatureType.Numbers,
                (uint)reader.Meta.TimeSignature.A, (uint)reader.Meta.TimeSignature.B));

            MusicalSymbols.Generate(reader.Notes, null);
        }

        public void LoadLilyPond()
        {
            var reader = new StreamReader(FileName);

            try
            {
                MusicalSymbols.Clear();

                string source;
                var result = LilyPondParser.parse(reader, out source);
                LilyPondSource = source;

                MusicalSymbols.AddRange(result);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //private void ValidateLilyPond()
        //{
        //    LilyPondError = "";
        //    var reader = new StringReader(LilyPond);

        //    try
        //    {
        //        MusicalSymbols.Clear();
        //        ParseLilyPond(reader);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.StackTrace);
        //        LilyPondError = e.Message;
        //    }
        //}

    }
}
