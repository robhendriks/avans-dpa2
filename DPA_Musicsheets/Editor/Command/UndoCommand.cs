namespace DPA_Musicsheets.Editor.Command
{
    public class UndoCommand : Command
    {
        public UndoCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.Undo();
        }
    }
}
