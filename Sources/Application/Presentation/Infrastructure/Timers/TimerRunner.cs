namespace JassApp.Presentation.Infrastructure.Timers
{
    public class RunningTask(Task task, CancellationTokenSource tokenSource)
    {
        public async Task DisposeAsync()
        {
            await tokenSource.CancelAsync();
            tokenSource.Dispose();

#pragma warning disable VSTHRD003
            await task;
#pragma warning restore VSTHRD003
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