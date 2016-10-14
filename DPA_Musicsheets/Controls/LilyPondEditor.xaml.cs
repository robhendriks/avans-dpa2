using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace DPA_Musicsheets.Controls
{
    public partial class LilyPondEditor : UserControl
    {
        public static DependencyProperty ValidProperty
            = DependencyProperty.Register("Valid", typeof(bool), typeof(LilyPondEditor),
                new PropertyMetadata(false));

        public static DependencyProperty LilyPondProperty
            = DependencyProperty.Register("LilyPond", typeof(string), typeof(LilyPondEditor) ,
                new PropertyMetadata(""));

        public static DependencyProperty SelectionStartProperty
           = DependencyProperty.Register("SelectionStart", typeof(int), typeof(LilyPondEditor),
               new PropertyMetadata(0));

        public static DependencyProperty SelectionLengthProperty
           = DependencyProperty.Register("SelectionLength", typeof(int), typeof(LilyPondEditor),
               new PropertyMetadata(0));

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

        public bool Valid
        {
            get { return (bool)GetValue(ValidProperty); }
            set { SetValue(ValidProperty, value); }
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
