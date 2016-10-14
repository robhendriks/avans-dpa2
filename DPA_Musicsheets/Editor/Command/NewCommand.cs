using System;

namespace DPA_Musicsheets.Editor.Command
{
    public class NewCommand : Command
    {
        public NewCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.New();
        }
    }
}
