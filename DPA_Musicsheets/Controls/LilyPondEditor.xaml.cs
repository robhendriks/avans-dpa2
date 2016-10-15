using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DPA_Musicsheets.Controls
{
    public partial class LilyPondEditor : UserControl
    {
        public static DependencyProperty LilyPondProperty
            = DependencyProperty.Register("LilyPond", typeof(string), typeof(LilyPondEditor) ,
                new PropertyMetadata("", OnLilyPondPropertyChanged));

        public static DependencyProperty SelectionStartProperty
           = DependencyProperty.Register("SelectionStart", typeof(int), typeof(LilyPondEditor),
               new PropertyMetadata(0));

        public static DependencyProperty SelectionLengthProperty
           = DependencyProperty.Register("SelectionLength", typeof(int), typeof(LilyPondEditor),
               new PropertyMetadata(0));

        private static void OnLilyPondPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var lilyPondEditor = sender as LilyPondEditor;
            if (sender != null)
            {
                lilyPondEditor.InvalidateEditor();
            }
        }

        public int SelectionStart
        {
            get { return (int)GetValue(SelectionStartProperty); }
            set { SetValue(SelectionStartProperty, value); }
        }

        public int SelectionLength
        {
            get { return (int)GetValue(SelectionLengthProperty); }
            set { SetValue(SelectionLengthProperty, value); }
        }

        public string LilyPond
        {
            get { return (string)GetValue(LilyPondProperty); }
            set { SetValue(LilyPondProperty, value); }
        }

        public LilyPondEditor()
        {
            InitializeComponent();
        }

        private void InvalidateEditor()
        {


            SetLines();
        }

        private int CountLines()
        {
            if (string.IsNullOrEmpty(LilyPond)) return 1;
            int n = 0;
            foreach (var c in LilyPond)
            {
                if (c == '\n') n++;
            }
            return n + 1;
        }

        private void SetLines()
        {
            StringBuilder sb = new StringBuilder();

            int lines = CountLines();
            for (int i = 0; i < lines; i++)
            {
                sb.Append(i + 1);
                if (i < lines - 1)
                {
                    sb.Append("\n");
                }
            }

            txtLines.Text = sb.ToString();
        }

        private void txtLilyPond_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (SelectionStart != txtLilyPond.SelectionStart)
            {
                SelectionStart = txtLilyPond.SelectionStart;
            }
            if (SelectionLength != txtLilyPond.SelectionLength)
            {
                SelectionLength = txtLilyPond.SelectionLength;
            }
        }
    }
}
