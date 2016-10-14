using System;
using System.Diagnostics;
using System.Windows.Input;

namespace DPA_Musicsheets.Editor
{
    public class EditorStateContext
    {
        public readonly EditableBase Editable;

        private EditorState _state;

        public EditorState State
        {
            get { return _state; }
            set
            {
                _state = value;
                Editable.ViewModel.RaisePropertyChanged(() => Editable.ViewModel.CanEdit);
            }
        }

        public EditorStateContext(EditableBase editable)
        {
            if (editable == null)
            {
                throw new ArgumentNullException(nameof(editable));
            }
            Editable = editable;
        }

        public bool CanEdit()
        {
            return State?.CanEdit(this) ?? false;
        }
    }
}
