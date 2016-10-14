namespace DPA_Musicsheets.Editor
{
    public interface IEditable
    {
        void New();
        void Open();
        void Save();
        void Export();
        void Undo();
        void Redo();
        void Insert(string content);
        void Exit();
    }
}
