using System.ComponentModel;
using System.Runtime.CompilerServices;
using MvvmHelpers;

namespace OctoWhirl.GUI.ViewModels.Technical
{
    public class InstrumentItem : BaseViewModel, INotifyPropertyChanged
    {
        private string _name;
        private bool _isSelected;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public InstrumentItem()
        {
            Name = string.Empty;
            IsSelected = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
