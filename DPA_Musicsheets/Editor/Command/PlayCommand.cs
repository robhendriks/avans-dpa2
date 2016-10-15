namespace DPA_Musicsheets.Editor.Command
{
    public class PlayCommand : Command
    {
        public PlayCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.Play();
        }
    }
}
