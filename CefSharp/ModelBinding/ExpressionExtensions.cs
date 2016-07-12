namespace CefSharp.ModelBinding
{
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Contains extension methods for the <see cref="Expression"/> type.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Retrieves the member that an expression is defined for.
        /// </summary>
        /// <param name="expression">The expression to retrieve the member from.</param>
        /// <returns>A <see cref="MemberInfo"/> instance if the member could be found; otherwise <see langword="null"/>.</returns>
        public static MemberInfo GetTargetMemberInfo(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return GetTargetMemberInfo(((UnaryExpression)expression).Operand);
                case ExpressionType.Lambda:
                    return GetTargetMemberInfo(((LambdaExpression)expression).Body);
                case ExpressionType.Call:
                    return ((MethodCallExpression)expression).Method;
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member;
                default:
                    return null;
            }
        }
    }
}