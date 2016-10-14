using System;
using System.Diagnostics;
using System.IO;

namespace DPA_Musicsheets.LilyPond
{
    public class LilyPondToPDF
    {
        public static readonly string LilyPondPath = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";

        public static void SaveLilypondToPdf(string sourceFileName, string targetFileName)
        {
            var fileInfo = new FileInfo(targetFileName);
            var sourceDirectory = fileInfo.DirectoryName;

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = $"--pdf -o \"{Path.GetFileNameWithoutExtension(targetFileName)}\" \"{sourceFileName}\"",
                    FileName = LilyPondPath
                }
            };

            Debug.WriteLine($"{sourceFileName} -> {targetFileName}");

            process.Start();
            process.WaitForExit();
        }
    }
}
