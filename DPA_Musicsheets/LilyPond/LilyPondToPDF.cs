using System;
using System.Diagnostics;
using System.IO;

namespace DPA_Musicsheets.LilyPond
{
    public class LilyPondToPDF
    {
        public static void SaveLilypondToPdf()
        {
            //???????
            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = @"c:\temp\";
            string sourceFileName = "Twee-emmertjes-water-halen";
            string targetFolder = @"c:\users\mmaaschu\desktop\";
            string targetFileName = "Test";

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("--pdf \"{0}{1}\"", sourceFolder, sourceFileName + ".ly"),
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited) { /* Wait for exit */ }

            File.Copy(sourceFolder + sourceFileName + ".pdf", targetFolder + targetFileName + ".pdf", true);
        }
    }
}
