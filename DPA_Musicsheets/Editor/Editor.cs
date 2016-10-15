namespace DPA_Musicsheets.Editor
{
    public class Editor
    {
        public Command.Command NewCommand { get; private set; }
        public Command.Command OpenCommand { get; private set; }
        public Command.Command SaveCommand { get; private set; }
        public Command.Command ExportCommand { get; private set; }
        public Command.Command UndoCommand { get; private set; }
        public Command.Command InsertCommand { get; private set; }
        public Command.Command ExitCommand { get; private set; }
        public Command.Command PlayCommand { get; private set; }
        public Command.Command StopCommand { get; private set; }

        public Editor(Command.Command NewCommand, Command.Command OpenCommand, Command.Command SaveCommand, Command.Command ExportCommand, Command.Command UndoCommand, Command.Command InsertCommand, Command.Command ExitCommand, Command.Command PlayCommand, Command.Command StopCommand)
        {
            this.NewCommand = NewCommand;
            this.OpenCommand = OpenCommand;
            this.SaveCommand = SaveCommand;
            this.ExportCommand = ExportCommand;
            this.UndoCommand = UndoCommand;
            this.InsertCommand = InsertCommand;
            this.ExitCommand = ExitCommand;
            this.PlayCommand = PlayCommand;
            this.StopCommand = StopCommand;
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

        public void Play()
        {
            PlayCommand.Execute();
        }

        public void Stop()
        {
            StopCommand.Execute();
        }
    }
}
