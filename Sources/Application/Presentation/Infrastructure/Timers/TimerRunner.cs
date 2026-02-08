namespace JassApp.Presentation.Infrastructure.Timers
{
    public record RunningTask(Task Task, CancellationTokenSource TokenSource)
    {
        public async Task DisposeAsync()
        {
            await TokenSource.CancelAsync();
            TokenSource.Dispose();
        }
    }

    public static class TimerRunner
    {
        public static RunningTask Run(Func<Task> callback, TimeSpan callbackSpan)
        {
            var timer = new PeriodicTimer(callbackSpan);
            var ct = new CancellationTokenSource();

            var task = RunAsync(timer, ct.Token, callback);

            return new RunningTask(task, ct);
        }

        private static async Task RunAsync(
            PeriodicTimer timer,
            CancellationToken ct,
            Func<Task> callback)
        {
            try
            {
                while (await timer.WaitForNextTickAsync(ct))
                {
                    await callback();
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                timer.Dispose();
            }
        }
    }
}