using MvvmHelpers;

namespace OctoWhirl.GUI.ViewModels.Technical
{
    public class TabItemViewModel : BaseViewModel
    {
        private string _header;
        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }

        private object _content;
        public object Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
    }
}
