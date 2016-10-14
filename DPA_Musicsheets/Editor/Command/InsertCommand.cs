using System;

namespace DPA_Musicsheets.Editor.Command
{
    public class InsertCommand : Command
    {
        public InsertCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.Insert();
        }
    }
}
