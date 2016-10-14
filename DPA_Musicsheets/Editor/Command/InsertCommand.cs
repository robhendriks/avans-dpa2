using System;
using System.Diagnostics;

namespace DPA_Musicsheets.Editor.Command
{
    public class InsertCommand : Command
    {
        public InsertCommand(IEditable editable) : base(editable)
        {
        }

        public override void Execute()
        {
            Editable.Insert(Parameter as string);
        }
    }
}
