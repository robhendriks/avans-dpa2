namespace DPA_Musicsheets.Utility
{
    class Editor
    {
        private Memento myMemento;

        public string LilypondContent { get; private set; }

        //FUNCS:
        //CTRL + S          Save as Lilypond
        //CTRL + S + P      Save as PDF
        //CTRL + O          Open File
        //ALT + C           Insert clef Treble
        //ALT + S           Insert \Tempo 4=120
        //ALT + T           Insert \Time 4/4
        //ALT + T + 4       Insert \Time 4/4
        //ALT + T + 3       Insert \Time 3/4
        //ALT + T + 6       Insert \Time 6/8

        //Ask to save file when exiting program. (only when lilypond is not saved.)
        //When 1.5 sec nothing happened -> Check if valid / Generate BLADMZUIEK. You can continue typing when generating.
        //Return to (multiple) Bookmarks.

        //Use of MEMENTO and STATE pattern.kapotjeplof
        //­ De mogelijkheid om keystrokes ná elkaar te doen (ALT A, ALT P is als PDF opslaan bijv.).
        //Mooi spul aangezien er geen ALT + A shortcut is volgens school :'))))).




        public Editor()
        {

        }

        public void SetContent(string lilypondContent)
        {
            LilypondContent = lilypondContent;
            if (IsValidLilypond(lilypondContent))
            {
                myMemento = new Memento(lilypondContent);
            }
            else
            {
                throw new LilyPond.LilyPondException("The content you entered is not valid Lilypond.");
            }
        }

        public void Revert()
        {
            if (myMemento.LilypondContent == null)
            {
                throw new LilyPond.LilyPondException("No state to go back to.");
            }
            else
            {
                LilypondContent = myMemento.LilypondContent;
            }

        }

        public bool IsValidLilypond(string lilypondContent)
        {
            //TODO, check if input is valid lilypond

            return false;
        }

        private void AddTime(int amount)
        {
            //Add \Time command
            //Default = 4/4, 4 = 4/4, 6 = 6/8
        }
    }
}
