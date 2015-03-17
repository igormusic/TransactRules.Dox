using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TransactRules.Core.Utility
{
  

        /// <summary>
        /// A variety of extensions to assist in using or manipulating expression trees
        /// </summary>
        public static class ExpressionExtensions
        {
            /// <summary>
            /// Given a simple expression tree ending in a property, returns the PropertyInfo for that property.
            /// </summary>
            public static PropertyInfo GetPropertyInfo(this LambdaExpression expr, bool mustBeSimple = true)
            {
                PropertyInfo propInfo = TryGetPropertyInfo(expr, mustBeSimple);
                if (propInfo == null)
                {
                    throw new NotSupportedException("Expression is not a property reference or is too complex: " + expr.ToString());
                }
                return propInfo;
            }

            public static PropertyInfo TryGetPropertyInfo(this LambdaExpression expr, bool mustBeSimple = true)
            {
                if (expr == null)
                {
                    throw new ArgumentNullException("expr");
                }

                var memberExpression = expr.Body as MemberExpression;
                if (memberExpression == null)
                {
                    return null;
                }

                var prop = memberExpression.Member as PropertyInfo;
                if (prop == null)
                {
                    return null;
                }

                if (mustBeSimple && memberExpression.Expression != expr.Parameters[0])
                {
                    return null;
                }

                return prop;
            }

            /// <summary>
            /// Combines two separate binary expression using the '||' (OrElse) operator.
            /// </summary>
            public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
            {
                b = new ParameterReplacementVisitor(b.Parameters[0], a.Parameters[0]).VisitAndConvert(b, "ExpressionExtensions.OrElse()");

                var c = (Expression<Func<T, bool>>)Expression.Lambda(Expression.OrElse(a.Body, b.Body), a.Parameters[0]);
                return c;
            }

            /// <summary>
            /// Traverses a given lambda expression, replacing references from one parameter to another.
            /// </summary>
            private class ParameterReplacementVisitor : ExpressionVisitor
            {
                private readonly ParameterExpression fromExpr;
                private readonly ParameterExpression toExpr;
                public ParameterReplacementVisitor(ParameterExpression fromExpr, ParameterExpression toExpr)
                {
                    this.fromExpr = fromExpr;
                    this.toExpr = toExpr;
                }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    return (node == this.fromExpr) ? this.toExpr : node;
                }
            }
        }
}
