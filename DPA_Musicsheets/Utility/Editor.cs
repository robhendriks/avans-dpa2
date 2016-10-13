namespace DPA_Musicsheets.Utility
{
    class Editor
    {
        private Memento myMemento;

        public string LilypondContent { get; private set; }

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
    }
}
