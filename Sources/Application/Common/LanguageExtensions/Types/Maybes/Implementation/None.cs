using System.Diagnostics.CodeAnalysis;

namespace JassApp.Common.LanguageExtensions.Types.Maybes.Implementation
{
    [SuppressMessage(
        "Design",
        "CA1052:Static holder types should be Static or NotInheritable",
        Justification = "Helper to avoid generic parsing")]
    public class None
    {
        private None()
        {
        }

        public static None Value { get; } = new();
    }

    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "It makes sense to keep these Classes together")]
    public sealed class None<T> : Maybe<T>
    {
        public override bool Equals(Maybe<T>? other)
        {
            return other is None<T>;
        }

        public override bool Equals(T? other)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}