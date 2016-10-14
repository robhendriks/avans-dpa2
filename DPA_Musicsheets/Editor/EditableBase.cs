using DPA_Musicsheets.Editor.State;
using DPA_Musicsheets.ViewModel;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace DPA_Musicsheets.Editor
{
    public class EditableBase : IEditable
    {
        public readonly MainViewModel ViewModel;

        public EditableBase(MainViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void Export()
        {
            Debug.WriteLine("Export");
        }

        public void Insert()
        {
            Debug.WriteLine("Insert");
        }

        public void New()
        {
            Debug.WriteLine("New");
        }

        public void Open()
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "Midi/Lilypond Files(.mid, .ly)|*.mid; *.ly" };
            if (dialog.ShowDialog() == true)
            {
                //MusicalSymbols.Clear();

                ViewModel.FileName = dialog.FileName;

                var fileInfo = new FileInfo(ViewModel.FileName);
                bool isLilyPond = (fileInfo.Extension == ".ly");

                if (isLilyPond)
                {
                    ViewModel.StateContext.State = new LilyPondState();
                    ViewModel.LoadLilyPond();
                }
                else
                {
                    ViewModel.StateContext.State = new MidiState();
                    ViewModel.LoadMidi();
                }
            }
        }

        public void Redo()
        {
            Debug.WriteLine("Redo");
        }

        public void Save()
        {
            Debug.WriteLine("Save");
        }

        public void Undo()
        {
            Debug.WriteLine("Undo");
        }

        public void Exit()
        {
            Debug.WriteLine("Exit");
        }
    }
}
