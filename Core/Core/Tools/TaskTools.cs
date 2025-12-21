using Models.TechnicalModels.Exceptions;

namespace Core.Technicals.Tools
{
    public static class TaskTools
    {
        public static async Task<T> Retry<T>(Func<Task<T>> task, int attempts = 1, int delay = 0)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (attempts < 0)
                throw new TaskFailedException();

            try
            {
                return await task().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await Task.Delay(delay).ConfigureAwait(false);
                return await Retry(task, attempts - 1, delay).ConfigureAwait(false);
            }
        }
    }
}
