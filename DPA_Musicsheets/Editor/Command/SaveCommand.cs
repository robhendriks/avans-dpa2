using System;

namespace DPA_Musicsheets.Editor.Command
{
    public class SaveCommand : Command
    {
        public SaveCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.Save();
        }
    }
}
