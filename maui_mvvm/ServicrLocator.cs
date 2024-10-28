

using ABI.Windows.ApplicationModel.Activation;
using maui_mvvm.Services;
using maui_mvvm.ViewModels;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace maui_mvvm;

public class ServicrLocator
{
    private IServiceProvider _serviceProvider;
    public ResultPageViewModel ResultPageViewModel =>
        _serviceProvider.GetService<ResultPageViewModel>();
    public ServicrLocator()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IPoetryStorage,PoetryStorage>();
        serviceCollection.AddSingleton<IPreferencesStorage,PreferencesStorage>();
        serviceCollection.AddSingleton<ResultPageViewModel>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
