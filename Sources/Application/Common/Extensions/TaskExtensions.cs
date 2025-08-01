using System.Diagnostics.CodeAnalysis;

namespace JassApp.Common.Extensions
{
    public static class TaskExtensions
    {
        [SuppressMessage("Usage", "VSTHRD003:Avoid awaiting foreign Tasks", Justification = "Dont have AwaitableTaskFactory at the moment")]
        public static async Task<IReadOnlyCollection<TResult>> SelectListAsync<T, TResult>(
            this Task<IReadOnlyCollection<T>> task,
            Func<T, TResult> selector)
        {
            var data = await task;

            return data
                .Select(selector)
                .ToList();
        }

        [SuppressMessage("Usage", "VSTHRD003:Avoid awaiting foreign Tasks", Justification = "Dont have AwaitableTaskFactory at the moment")]
        public static async Task<TResult> MapAsync<T, TResult>(
            this Task<T> task,
            Func<T, TResult> selector)
        {
            var data = await task;
            return selector(data);
        }
    }
}