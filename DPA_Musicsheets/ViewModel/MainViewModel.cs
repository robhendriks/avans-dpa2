using DPA_Musicsheets.Editor;
using DPA_Musicsheets.Editor.Command;
using DPA_Musicsheets.Editor.State;
using DPA_Musicsheets.LilyPond;
using DPA_Musicsheets.Music;
using DPA_Musicsheets.Utility;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
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

        public Command NewCommand => Editor.NewCommand;
        public Command OpenCommand => Editor.OpenCommand;
        public Command SaveCommand => Editor.SaveCommand;
        public Command ExportCommand => Editor.ExportCommand;
        public Command UndoCommand => Editor.UndoCommand;
        public Command InsertCommand => Editor.InsertCommand;
        public Command ExitCommand => Editor.ExitCommand;
        public Command PlayCommand => Editor.PlayCommand;
        public Command StopCommand => Editor.StopCommand;

        public readonly EditorStateContext StateContext;
        public bool isSaved;
        public Memento MyMemento;

        private MusicPlayer player = new MusicPlayer();

        private bool _isPlaying = false;

        public bool IsPlaying
        {
            get { return _isPlaying;  }
            set
            {
                _isPlaying = value;
                RaisePropertyChanged(() => IsPlaying);
                CommandManager.InvalidateRequerySuggested();
            }
        }

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
                    isSaved = false;
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
            isSaved = true;
            MyMemento = new Memento();
            timer = new Timer(TimerHandler, autoEvent, Timeout.Infinite, Timeout.Infinite);

            Editable = new EditableBase(this);
            StateContext = new EditorStateContext(Editable);

            Editor = new Editor.Editor(
                new NewCommand(Editable),
                new OpenCommand(Editable),
                new SaveCommand(Editable),
                new ExportCommand(Editable),
                new UndoCommand(Editable),
                new InsertCommand(Editable),
                new ExitCommand(Editable),
                new PlayCommand(Editable),
                new StopCommand(Editable));
        }

        public bool IsMidi => !string.IsNullOrEmpty(FileName) && FileName.EndsWith("mid");

        public void Play()
        {
            if (!IsMidi || IsPlaying) return;
            IsPlaying = true;
            player.Play(FileName);
        }

        public void Stop()
        {
            if (!IsMidi || !IsPlaying) return;
            IsPlaying = false;
            player.Stop();
        }

        public void TimerHandler(object state)
        {
            if (timerBusy || !CanEdit)
            {
                return;
            }
            timerBusy = true;

            Debug.WriteLine("TICK");
            timer.Change(Timeout.Infinite, Timeout.Infinite);

            Application.Current.Dispatcher.Invoke(() =>
            {
                ValidateLilyPond();
                timerBusy = false;
            }, DispatcherPriority.ContextIdle);
        }

        public void Reset()
        {
            coldLoad = false;
            isSaved = true;

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
            isSaved = true;
            LilyPondSource = LilyPondError = "";

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
            }
            finally
            {
                reader.Dispose();
            }

            coldLoad = false;
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

                    MusicalSymbols.AddRange(result);
                    LilyPondError = "";
                    MyMemento.LilypondContent = LilyPondSource;
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
