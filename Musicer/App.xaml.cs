using System.Windows;
using Musicer.ViewModels;
using Musicer.Views;
using Prism.Ioc;

namespace Musicer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<SettingPage, SettingPageViewModel>();
            containerRegistry.RegisterDialog<HistoryPage, HistoryPageViewModel>();
        }
    }
}