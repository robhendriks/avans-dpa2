using System;

namespace DPA_Musicsheets.Editor.Command
{
    public class ExportCommand : Command
    {
        public ExportCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.Export();
        }
    }
}
