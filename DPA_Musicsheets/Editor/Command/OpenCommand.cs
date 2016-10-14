using System;

namespace DPA_Musicsheets.Editor.Command
{
    public class OpenCommand : Command
    {
        public OpenCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.Open();
        }
    }
}
