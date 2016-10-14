using System;

namespace DPA_Musicsheets.Editor.State
{
    public class MidiState : EditorState
    {
        public bool CanEdit(EditorStateContext context)
        {
            return false;
        }
    }
}
