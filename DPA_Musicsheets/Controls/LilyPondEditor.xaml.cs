using System.Windows;
using System.Windows.Controls;

namespace DPA_Musicsheets.Controls
{
    public partial class LilyPondEditor : UserControl
    {
        public static DependencyProperty ValidProperty
            = DependencyProperty.Register("Valid", typeof(bool), typeof(LilyPondEditor)
                , new PropertyMetadata(false));

        public bool Valid
        {
            get { return (bool)GetValue(ValidProperty); }
            private set { SetValue(ValidProperty, value); }
        }

        public LilyPondEditor()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
