using OctoWhirl.Exceptions;

namespace AsyncTools
{
    public static class TaskTools
    {
        private static int _maxRecursionDepth = -1000;

        /// <summary>
        /// Retries a task after a certain delay
        /// If attempts is negative or zero, retries until a maximum recursion depth.
        /// <exception cref="TaskFailedException"></exception>
        public static async Task<T> Retry<T>(Func<Task<T>> task, int attempts = 1, int delay = 0)
        {
            if (task == null) 
                throw new ArgumentNullException(nameof(task));

            int retryLimit = attempts > 0 ? 0 : _maxRecursionDepth;

            attempts = new int[] { attempts, 0, _maxRecursionDepth }.Max();
            delay = new int[] { 0, delay }.Max();

            var exception = new Exception();
            while (attempts > retryLimit)
            {
                try
                {
                    return await task();
                }
                catch (Exception ex)
                {
                    exception = ex;
                    attempts--;
                    await Task.Delay(delay);
                }
            }

            throw new TaskFailedException(exception);
        }

        /// <summary>
        /// Retries an action after a certain delay
        /// If attempts is negative or zero, retries until a maximum recursion depth.
        /// <exception cref="TaskFailedException"></exception>
        public static void Retry(Action action, int attempts = 0, int delay = 0)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            int retryLimit = attempts > 0 ? 0 : _maxRecursionDepth;

            attempts = new int[] { attempts, 0, _maxRecursionDepth }.Max();
            delay = new int[] { 0, delay }.Max();

            var exception = new Exception();
            while (attempts > retryLimit)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    attempts--;
                    Task.Delay(delay).Wait();
                }
            }

            throw new TaskFailedException(exception);
        }

        /// <summary>
        /// Retries a certain function after a certain delay
        /// If attempts is negative or zero, retries until a maximum recursion depth.
        /// <exception cref="TaskFailedException"></exception>
        public static T Retry<T>(Func<T> function, int attempts = 0, int delay = 0)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            int retryLimit = attempts > 0 ? 0 : _maxRecursionDepth;

            attempts = new int[] { attempts, 0, _maxRecursionDepth }.Max();
            delay = new int[] { 0, delay }.Max();

            var exception = new Exception();
            while (attempts > retryLimit)
            {
                try
                {
                    return function();
                }
                catch (Exception ex)
                {
                    exception = ex;
                    attempts--;
                    Task.Delay(delay).Wait();
                }
            }

            throw new TaskFailedException(exception);
        }
    }
}
