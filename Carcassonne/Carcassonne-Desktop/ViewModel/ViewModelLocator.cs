
using Carcassonne_Desktop.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Carcassonne_Desktop.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainMenuViewModel>();
            SimpleIoc.Default.Register<OnlineBoardViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<TileViewerModel>();
            SimpleIoc.Default.Register<ScoreBoardModel>();
            SimpleIoc.Default.Register<GameSettingsViewModel>();
        }

        public MainMenuViewModel MainMenuViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainMenuViewModel>(); }
        }

        public GameSettingsViewModel GameSettingsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<GameSettingsViewModel>(); }
        }

        public OnlineBoardViewModel OnlineBoardViewModel
        {
            get { return ServiceLocator.Current.GetInstance<OnlineBoardViewModel>(); }
        }

        public OfflineBoardViewModel OfflineBoardViewModel
        {
            get { return ServiceLocator.Current.GetInstance<OfflineBoardViewModel>(); }
        }

        public LoginViewModel LoginViewModel
        {
            get { return ServiceLocator.Current.GetInstance<LoginViewModel>(); }
        }

        public TileViewerModel TileViewerModel
        {
            get { return ServiceLocator.Current.GetInstance<TileViewerModel>(); }
        }

        public ScoreBoardModel ScoreBoardModel
        {
            get { return ServiceLocator.Current.GetInstance<ScoreBoardModel>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}