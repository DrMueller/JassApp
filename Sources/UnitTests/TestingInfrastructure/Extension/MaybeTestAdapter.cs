using FluentAssertions;
using JassApp.Common.LanguageExtensions.Types.Maybes;
using JassApp.Common.LanguageExtensions.Types.Maybes.Implementation;

namespace JassApp.UnitTests.TestingInfrastructure.Extension
{
    public static class MaybeTestAdapter
    {
        public static void ShouldBeNone<T>(this Maybe<T> actualValue)
        {
            actualValue.Should().BeOfType<None<T>>();
        }

        public static T ShouldBeSome<T>(this Maybe<T> actualValue)
        {
            var some = actualValue.Should().BeOfType<Some<T>>().Subject;
            return (T)some;
        }
    }
}