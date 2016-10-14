using DPA_Musicsheets.Editor;
using DPA_Musicsheets.Editor.Command;
using DPA_Musicsheets.Editor.State;
using DPA_Musicsheets.LilyPond;
using DPA_Musicsheets.Music;
using DPA_Musicsheets.Utility;
using GalaSoft.MvvmLight;
using PSAMControlLibrary;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

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

        private readonly Timer timer;
        private bool timerBusy = false;
        private bool coldLoad = true;

        private string _lilyPondSource;

        public string LilyPondSource
        {
            get { return _lilyPondSource; }
            set
            {
                _lilyPondSource = value;
                RaisePropertyChanged(() => LilyPondSource);
                if (!timerBusy && !coldLoad)
                {
                    timerBusy = true;
                    timer.Change(1500, 1500);
                }
            }
        }

        private string _lilyPondError;

        public string LilyPondError
        {
            get { return _lilyPondError; }
            set
            {
                _lilyPondError = value;
                RaisePropertyChanged(() => LilyPondError);
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

        public int SelectionStart { get; set; }
        public int SelectionLength { get; set; }

        public bool CanEdit => StateContext.CanEdit();

        public MainViewModel()
        {
            var autoEvent = new AutoResetEvent(false);

            timer = new Timer(TimerHandler, autoEvent, Timeout.Infinite, Timeout.Infinite);

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

        private static object mutex = new object();

        public void TimerHandler(object state)
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timerBusy = false;

            Application.Current.Dispatcher.Invoke(() => ValidateLilyPond(), DispatcherPriority.ContextIdle);
        }

        public void Reset()
        {
            FileName = "";
            StateContext.State = new LilyPondState();
            LilyPondSource = LilyPondError = "";
            MusicalSymbols.Clear();
        }

        public void LoadMidi()
        {
            MusicalSymbols.Clear();
            LilyPondSource = LilyPondError = "";

            var reader = new MidiReader(FileName);

            MusicalSymbols.Add(new Clef(ClefType.GClef, 2));
            MusicalSymbols.Add(new PSAMControlLibrary.TimeSignature(TimeSignatureType.Numbers,
                (uint)reader.Meta.TimeSignature.A, (uint)reader.Meta.TimeSignature.B));

            MusicalSymbols.Generate(reader.Notes, null);
        }

        public void LoadLilyPond()
        {
            coldLoad = true;
            LilyPondSource = LilyPondError = "";
            coldLoad = false;

            var reader = new StreamReader(FileName);

            try
            {
                MusicalSymbols.Clear();

                string source;
                var result = LilyPondParser.parse(reader, out source);
                LilyPondSource = source;

                MusicalSymbols.AddRange(result);

                LilyPondError = "";
            }
            catch (Exception e)
            {
                LilyPondError = e.Message;
                MessageBox.Show(e.Message);
            }
            finally
            {
                reader.Dispose();
            }
        }

        private void ValidateLilyPond()
        {
            lock (this)
            {
                LilyPondError = "";
                var reader = new StringReader(LilyPondSource);

                try
                {
                    MusicalSymbols.Clear();

                    string source;
                    var result = LilyPondParser.parse(reader, out source);
                    LilyPondSource = source;

                    MusicalSymbols.AddRange(result);

                    LilyPondError = "";
                }
                catch (Exception e)
                {
                    LilyPondError = e.Message;
                }
                finally
                {
                    reader.Dispose();
                }
            }
        }
    }
}
