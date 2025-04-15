using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace OctoWhirl.GUI.Views
{
    public class ViewFactory : IViewFactory
    {
        private readonly IServiceProvider _provider;

        public ViewFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public T Create<T>() where T : FrameworkElement
        {
            return _provider.GetRequiredService<T>();
        }
    }
}
