namespace DPA_Musicsheets.Editor.Command
{
    public class StopCommand : Command
    {
        public StopCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.Stop();
        }
    }
}
