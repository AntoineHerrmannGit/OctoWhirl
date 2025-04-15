using System.Windows;
using System.Windows.Controls;

namespace OctoWhirl.GUI.Helpers
{
    public class ViewModelTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            var view = ViewLocator.ResolveView(item);
            var contentPresenter = container as ContentPresenter;

            return new DataTemplate
            {
                VisualTree = new FrameworkElementFactory(view.GetType())
            };
        }
    }
}