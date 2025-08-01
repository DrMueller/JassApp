using JassApp.Common.LanguageExtensions.Types.Eithers.Implementation;
using JetBrains.Annotations;

namespace JassApp.Common.LanguageExtensions.Types.Eithers
{
    [PublicAPI]
    public static class EitherAdapters
    {
        public static async Task<Either<TLeft, TNewRight>> BindRightAsync<TLeft, TRight, TNewRight>(
            this Task<Either<TLeft, TRight>> eitherTask, Func<TRight, Either<TLeft, TNewRight>> map)
        {
#pragma warning disable VSTHRD003
            var either = await eitherTask;
#pragma warning restore VSTHRD003
            if (either is Left<TLeft, TRight> left)
            {
                return (TLeft)left;
            }

            var right = (Right<TLeft, TRight>)either;
            var mappedRight = map(right);
            return mappedRight;
        }

        public static Either<TLeft, TNewRight> MapRight<TLeft, TRight, TNewRight>(
            this Either<TLeft, TRight> either, Func<TRight, TNewRight> map)
        {
            return either is Right<TLeft, TRight> right
                ? map(right)
                : (TLeft)(Left<TLeft, TRight>)either;
        }

        public static async Task<Either<TLeft, TNewRight>> MapRightAsync<TLeft, TRight, TNewRight>(
            this Either<TLeft, TRight> either, Func<TRight, Task<TNewRight>> map)
        {
            return either is Right<TLeft, TRight> right
                ? await map(right)
                : (TLeft)(Left<TLeft, TRight>)either;
        }

        public static async Task<Either<TLeft, TNewRight>> MapRightAsync<TLeft, TRight, TNewRight>(
            this Task<Either<TLeft, TRight>> eitherTask, Func<TRight, Task<TNewRight>> map)
        {
#pragma warning disable VSTHRD003
            var either = await eitherTask;
#pragma warning restore VSTHRD003
            return either is Right<TLeft, TRight> right
                ? await map(right)
                : (TLeft)(Left<TLeft, TRight>)either;
        }

        public static async Task<Either<TLeft, TNewRight>> MapRightAsync<TLeft, TRight, TNewRight>(
            this Task<Either<TLeft, TRight>> eitherTask, Func<TRight, TNewRight> map)
        {
#pragma warning disable VSTHRD003
            var either = await eitherTask;
#pragma warning restore VSTHRD003
            return either is Right<TLeft, TRight> right
                ? map(right)
                : (TLeft)(Left<TLeft, TRight>)either;
        }

        public static TRight Reduce<TLeft, TRight>(
            this Either<TLeft, TRight> either, Func<TLeft, TRight> map)
        {
            return either is Left<TLeft, TRight> left
                ? map(left)
                : (Right<TLeft, TRight>)either;
        }

        public static T Reduce<T>(
            this Either<T, T> either)
        {
            if (either is Left<T, T> left)
            {
                return left;
            }

            return (Right<T, T>)either;
        }

        public static async Task<T> ReduceAsync<T>(
            this Task<Either<T, T>> eitherTask)
        {
#pragma warning disable VSTHRD003
            var either = await eitherTask;
#pragma warning restore VSTHRD003
            if (either is Left<T, T> left)
            {
                return left;
            }

            return (Right<T, T>)either;
        }
    }
}