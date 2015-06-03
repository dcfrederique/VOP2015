using System.Windows;
using MahApps.Metro.Controls;

namespace Carcassonne_Desktop.Views
{
    public partial class MainMenuView : MetroWindow
    {
        public MainMenuView()
        {
            InitializeComponent();
        }

        private void MainMenuViewWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
