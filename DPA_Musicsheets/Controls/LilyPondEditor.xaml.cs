using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace DPA_Musicsheets.Controls
{
    public partial class LilyPondEditor : UserControl
    {
        public static DependencyProperty ValidProperty
            = DependencyProperty.Register("Valid", typeof(bool), typeof(LilyPondEditor)
                , new PropertyMetadata(false));

        public static DependencyProperty LilyPondProperty
            = DependencyProperty.Register("LilyPond", typeof(string), typeof(LilyPondEditor)
                , new PropertyMetadata("", OnLilyPondPropertyChanged));

        private static void OnLilyPondPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var lilyPondEditor = sender as LilyPondEditor;
            if (sender != null)
            {
                lilyPondEditor.InvalidateLilyPond();
            }
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

        public void InvalidateLilyPond()
        {
            Debug.WriteLine("I AM HAS ALL YOUR L I L Y P O N D S");
        }
    }
}
