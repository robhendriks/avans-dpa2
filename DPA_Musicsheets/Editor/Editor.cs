namespace DPA_Musicsheets.Editor
{
    public class Editor
    {
        Command.Command NewCommand;
        Command.Command OpenCommand;
        Command.Command SaveCommand;
        Command.Command ExportCommand;
        Command.Command UndoCommand;
        Command.Command InsertCommand;
        Command.Command ExitCommand;

        public Editor(Command.Command NewCommand, Command.Command OpenCommand, Command.Command SaveCommand, Command.Command ExportCommand, Command.Command UndoCommand, Command.Command InsertCommand, Command.Command ExitCommand)
        {
            this.NewCommand = NewCommand;
            this.OpenCommand = OpenCommand;
            this.SaveCommand = SaveCommand;
            this.ExportCommand = ExportCommand;
            this.UndoCommand = UndoCommand;
            this.InsertCommand = InsertCommand;
            this.ExitCommand = ExitCommand;
        }

        public void New()
        {
            NewCommand.Execute();
        }

        public void Open()
        {
            OpenCommand.Execute();
        }

        public void Save()
        {
            SaveCommand.Execute();
        }

        public void Export()
        {
            ExportCommand.Execute();
        }

        public void Undo()
        {
            UndoCommand.Execute();
        }

        public void Insert()
        {
            InsertCommand.Execute();
        }

        public void Exit()
        {
            ExitCommand.Execute();
        }
    }
}
