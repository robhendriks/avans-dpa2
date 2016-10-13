using DPA_Musicsheets.Utility;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly MusicPlayer player = new MusicPlayer();

        private bool _isLily = false;

        public bool IsLily
        {
            get { return _isLily; }
            private set
            {
                _isLily = value;
                RaisePropertyChanged(() => IsLily);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool _playing = false;

        public bool Playing
        {
            get { return _playing; }
            private set
            {
                _playing = value;
                RaisePropertyChanged(() => Playing);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _fileName = string.Empty;

        public string FileName
        {
            get { return _fileName; }
            private set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private ObservableCollection<MusicalSymbol> _musicalSymbols = new ObservableCollection<MusicalSymbol>();

        public ObservableCollection<MusicalSymbol> MusicalSymbols
        {
            get { return _musicalSymbols; }
            set { _musicalSymbols = value; }
        }

        public bool HasFilename => !string.IsNullOrEmpty(FileName);

        public ICommand OpenCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand ShowCommand { get; private set; }

        public MainViewModel()
        {
            OpenCommand = new RelayCommand(OnOpenCommand, CanOpen);
            PlayCommand = new RelayCommand(OnPlayCommand, CanPlay);
            StopCommand = new RelayCommand(OnStopCommand, CanStop);
            ShowCommand = new RelayCommand(OnShowCommand, CanShow);
        }

        public bool CanOpen()
        {
            return !Playing;
        }

        public bool CanPlay()
        {
            return HasFilename && !Playing;
        }

        public bool CanStop()
        {
            return Playing;
        }

        public bool CanShow()
        {
            return HasFilename;
        }

        public void OnOpenCommand()
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "Midi/Lilypond Files(.mid, .ly)|*.mid; *.ly" };
            if (dialog.ShowDialog() == true)
            {
                MusicalSymbols.Clear();

                FileName = dialog.FileName;

                var fileInfo = new FileInfo(FileName);
                IsLily = (fileInfo.Extension == ".ly");

                if (IsLily)
                {
                    var reader = new StreamReader(FileName);
                    var lexer = new LilyPond.LilyPondLexer(reader);
                    var parser = new LilyPond.LilyPondParser(lexer);

                    foreach (var pair in parser.Parameters)
                    {
                        Debug.WriteLine(pair);

                        switch (pair.Key)
                        {
                            case "clef":
                                if (pair.Value == "treble")
                                {
                                    MusicalSymbols.Add(new Clef(ClefType.GClef, 2));
                                }
                                else if (pair.Value == "bass")
                                {
                                    MusicalSymbols.Add(new Clef(ClefType.FClef, 2));
                                }
                                else
                                {
                                    MusicalSymbols.Add(new Clef(ClefType.CClef, 2));
                                }
                                break;
                            case "time":
                                var parts = pair.Value.Split('/');

                                uint beats = uint.Parse(parts[0]);
                                uint beatType = uint.Parse(parts[1]);

                                MusicalSymbols.Add(new PSAMControlLibrary.TimeSignature(TimeSignatureType.Numbers, beats, beatType));
                                break;
                            case "tempo":
                                break;
                        }
                    }

                    var baseNote = (parser.Notes.Count > 2 ? parser.Notes[1] : null);

                    int i = 0;
                    foreach (var note in parser.Notes)
                    {
                        if (i == parser.Notes.Count -1) i = parser.Notes.Count - 2;

                        if (!note.IsRelative)
                        {

                            MusicalSymbols.Add(MusicalSymbolFactory.Create(baseNote, note, parser.Notes[i+1], parser.Notes[i - 1]));
                            if (note.HasBarLine)
                            {
                                MusicalSymbols.Add(new Barline());
                            }
                        }

                        i++;
                    }
                }
                else
                {
                    ShowCommand.Execute(null);
                }
            }
        }

        public void OnPlayCommand()
        {
            Playing = true;
            player.Play(FileName);
        }

        public void OnStopCommand()
        {
            Playing = false;
            player.Stop();
        }

        public void OnShowCommand()
        {
        }
    }
}
