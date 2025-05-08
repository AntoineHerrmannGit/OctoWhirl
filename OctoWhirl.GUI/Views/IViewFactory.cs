using System.Windows;

namespace OctoWhirl.GUI.Views
{
    public interface IViewFactory
    {
        T Create<T>() where T : FrameworkElement;
    }
}
