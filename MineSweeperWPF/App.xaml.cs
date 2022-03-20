using MineSweeperWPF.Models;
using MineSweeperWPF.ViewModels;
using MineSweeperWPF.Views;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;

namespace MineSweeperWPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override Window CreateShell() => Container.Resolve<MainView>();

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<IDataGenerator, DataGenerator>();
        containerRegistry.Register<IMineSweeper, MineSweeper>();
    }

    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();
        ViewModelLocationProvider.Register<MainView, MainViewModel>();
    }
}
