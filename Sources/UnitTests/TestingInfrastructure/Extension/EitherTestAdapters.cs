using FluentAssertions;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.Common.LanguageExtensions.Types.Eithers.Implementation;

namespace JassApp.UnitTests.TestingInfrastructure.Extension
{
    // ATTENTION: Only for test use, never move this to the productive code
    public static class EitherTestAdapters
    {
        public static TRight ShouldBeRight<TLeft, TRight>(this Either<TLeft, TRight> either)
        {
            return either.Should().BeOfType<Right<TLeft, TRight>>().Subject;
        }

        public static TLeft ShouldBeLeft<TLeft, TRight>(this Either<TLeft, TRight> either)
        {
            return either.Should().BeOfType<Left<TLeft, TRight>>().Subject;
        }

        public static TRight ReduceRight<TLeft, TRight>(
            this Either<TLeft, TRight> either)
        {
            return (Right<TLeft, TRight>)either;
        }
    }
}