using System.Windows;

namespace OctoWhirl.GUI.GUIHelpers
{
    public static class ThreadingHelper
    {
        public static void ExecuteInGUI(Action action)
        {
            // On UI thread ?
            if (Application.Current.Dispatcher.CheckAccess())
                action();
            else
                Application.Current.Dispatcher.Invoke(action);
        }

        public static async Task ExecuteInGUIAsync(Func<Task> asyncAction)
        {
            // On UI thread ?
            if (Application.Current.Dispatcher.CheckAccess())
                await asyncAction();
            else
                await Application.Current.Dispatcher.InvokeAsync(asyncAction).Task.Unwrap();
        }
    }
}
