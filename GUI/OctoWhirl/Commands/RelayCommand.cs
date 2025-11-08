using System.Windows.Input;

namespace OctoWhirl.GUI.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        private readonly Func<object, Task> _executeAsync;
        private bool _isExecuting;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = _ => execute();
            if (canExecute != null)
                _canExecute = _ => canExecute();
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
        {
            _executeAsync = _ => executeAsync();
            if (canExecute != null)
                _canExecute = _ => canExecute();
        }

        public RelayCommand(Func<object, Task> executeAsync, Func<object, bool> canExecute = null)
        {
            _executeAsync = executeAsync;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_isExecuting) return false; // empêche double clic pendant exécution
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public async void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }
            else if (_executeAsync != null)
            {
                try
                {
                    _isExecuting = true;
                    RaiseCanExecuteChanged();
                    await _executeAsync(parameter);
                }
                finally
                {
                    _isExecuting = false;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
