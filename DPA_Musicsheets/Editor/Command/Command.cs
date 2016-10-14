using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;

namespace DPA_Musicsheets.Editor.Command
{
    public abstract class Command : ICommand
    {
        public readonly IEditable Editable;

        public Command(IEditable editable)
        {
            if (editable == null)
            {
                throw new ArgumentNullException(nameof(editable));
            }
            Editable = editable;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public abstract void Execute();

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            Execute();
        }
    }
}
