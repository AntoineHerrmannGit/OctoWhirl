using System.Windows.Controls;

namespace OctoWhirl.GUI.Helpers
{
    public static class ViewLocator
    {
        public static UserControl ResolveView(object viewModel)
        {
            var viewModelType = viewModel.GetType();
            var viewTypeName = viewModelType.FullName.Replace("ViewModel", "View");
            var viewAssembly = viewModelType.Assembly;

            var viewType = viewAssembly.GetType(viewTypeName);
            if (viewType == null)
                throw new InvalidOperationException($"Vue non trouvée pour {viewModelType.Name}");

            var view = Activator.CreateInstance(viewType) as UserControl;
            if (view != null)
                view.DataContext = viewModel;

            return view;
        }
    }
}