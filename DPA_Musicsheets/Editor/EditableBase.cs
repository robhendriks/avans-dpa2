﻿using DPA_Musicsheets.Editor.State;
using DPA_Musicsheets.LilyPond;
using DPA_Musicsheets.ViewModel;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace DPA_Musicsheets.Editor
{
    public class EditableBase : IEditable
    {
        private static readonly OpenFileDialog openDialog
            = new OpenFileDialog() { Filter = "Midi/LilyPond Files(.mid, .ly)|*.mid; *.ly" };

        private static readonly SaveFileDialog saveDialog
            = new SaveFileDialog();

        public readonly MainViewModel ViewModel;

        public EditableBase(MainViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void Export()
        {
            saveDialog.Filter = "Pdf Files|*.pdf";

            if (ViewModel.CanEdit && saveDialog.ShowDialog() == true)
            {
                try
                {
                    LilyPondToPDF.SaveLilypondToPdf(ViewModel.FileName, saveDialog.FileName);
                    MessageBox.Show("Export complete.");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Could not export PDF: " + e.Message);
                }
            }
        }

        public void Insert(string content)
        {
            if (ViewModel.CanEdit)
            {
                int start = ViewModel.SelectionStart;
                int length = ViewModel.SelectionLength;

                var source = ViewModel.LilyPondSource;
                var builder = new StringBuilder(source);

                if (length > 0)
                {
                    builder.Remove(start, length)
                        .Insert(start, content);
                }
                else
                {
                    builder.Insert(start, content);
                }

                ViewModel.LilyPondSource = builder.ToString();
            }
        }

        public void New()
        {
            ViewModel.Reset();
        }

        public void Open()
        {
            if (openDialog.ShowDialog() == true)
            {
                ViewModel.FileName = openDialog.FileName;

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
            if (ViewModel.CanEdit)
            {
                if (string.IsNullOrEmpty(ViewModel.FileName))
                {
                    saveDialog.Filter = "LilyPond Files|*.ly";
                    if (saveDialog.ShowDialog() == true)
                    {
                        ViewModel.FileName = saveDialog.FileName;
                        LilyPondToFile(saveDialog.FileName);
                    }
                }
                else
                {
                    LilyPondToFile(ViewModel.FileName);
                }
            }
        }

        public void LilyPondToFile(string fileName)
        {
            Debug.WriteLine("meh savings");

            try
            {
                using (var sw = new StreamWriter(fileName, false))
                {
                    sw.WriteLine(ViewModel.LilyPondSource);
                    sw.Flush();
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Unable to save file: " + e.Message);
            }
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
