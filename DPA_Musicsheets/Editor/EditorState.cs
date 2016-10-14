namespace DPA_Musicsheets.Editor
{
    public interface EditorState
    {
        bool CanEdit(EditorStateContext context);
    }
}
