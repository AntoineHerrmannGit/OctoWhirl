using System.ComponentModel;
using System.Runtime.CompilerServices;
using MvvmHelpers;

namespace OctoWhirl.GUI.ViewModels.Technical
{
    public class InstrumentItem : BaseViewModel, INotifyPropertyChanged
    {
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        string _name = string.Empty;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
        bool _isSelected;
    }

}
