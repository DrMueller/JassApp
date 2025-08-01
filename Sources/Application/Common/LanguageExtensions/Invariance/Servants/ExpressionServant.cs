using System.Linq.Expressions;

namespace JassApp.Common.LanguageExtensions.Invariance.Servants
{
    internal static class ExpressionServant
    {
        internal static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (!(propertyExpression.Body is MemberExpression memberExpression))
            {
                throw new ArgumentException(
                    "You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return memberExpression.Member.Name;
        }
    }
}