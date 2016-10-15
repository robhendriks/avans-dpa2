using DPA_Musicsheets.ViewModel;
using System.Windows;

namespace MidiPlayerTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            var mainViewModel = (Current.Resources["Locator"] as ViewModelLocator).Main;
            mainViewModel?.ExitCommand.Execute();
        }
    }
}
