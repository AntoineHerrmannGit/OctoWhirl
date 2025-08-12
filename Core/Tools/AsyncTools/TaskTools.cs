namespace AsyncTools
{
    public static class TaskTools
    {
        public static Task<T> Retry<T>(Task<T> task, int attempts = 1, int delay = 0)
        {
            if (task == null) 
                throw new ArgumentNullException(nameof(task));

            if (delay > 0)
                Task.WaitAll(Task.Delay(delay));

            if (attempts <= 0)
                return task;

            attempts--;
            return Retry(task);
        }
    }
}
