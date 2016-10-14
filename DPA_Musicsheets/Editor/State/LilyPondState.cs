namespace DPA_Musicsheets.Editor.State
{
    public class LilyPondState : EditorState
    {
        public bool CanEdit(EditorStateContext context)
        {
            return true;
        }
    }
}
