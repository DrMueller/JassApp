using JassApp.Common.LanguageExtensions.Types.Eithers.Implementation;

namespace JassApp.Common.LanguageExtensions.Types.Eithers
{
    public abstract class Either<TLeft, TRight>
    {
        public static implicit operator Either<TLeft, TRight>(TLeft left)
        {
            return new Left<TLeft, TRight>(left);
        }

        public static implicit operator Either<TLeft, TRight>(TRight right)
        {
            return new Right<TLeft, TRight>(right);
        }
    }
}