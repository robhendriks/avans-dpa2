using System;

namespace DPA_Musicsheets.Editor.Command
{
    public class RedoCommand : Command
    {
        public RedoCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.Redo();
        }
    }
}
