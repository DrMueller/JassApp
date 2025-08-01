namespace JassApp.Common.LanguageExtensions.Types.Eithers.Implementation
{
    public class Right<TLeft, TRight>(TRight content) : Either<TLeft, TRight>
    {
        private TRight Content { get; } = content;

        public static implicit operator TRight(Right<TLeft, TRight> right)
        {
            return right.Content;
        }
    }
}