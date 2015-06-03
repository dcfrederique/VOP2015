using System.Windows;
using MahApps.Metro.Controls;
using Carcassonne_Desktop.ViewModels;

namespace Carcassonne_Desktop.Views
{
    public partial class LoginView : MetroWindow
    {
        public LoginView()
        {
            InitializeComponent();

            Loaded += LoginView_Loaded;
        }

        void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).Window = this;
        }
    }
}
