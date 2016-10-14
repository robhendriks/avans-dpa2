using System;

namespace DPA_Musicsheets.Editor.Command
{
    public class ExitCommand : Command
    {
        public ExitCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.Exit();
        }
    }
}
