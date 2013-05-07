using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

// Reference: http://www.codetuning.net/blog/post/Entity-Framework-compile-safe-Includes.aspx
// Thank you to Rudi Grobler at codetuning.net for this blog post that I reworked slightly, but am using their basic code to allow for a multiple level LINQ expression
//  to get translated into a string representation that can be used for the EF Include statement
namespace SharpRepository.Repository.Helpers
{
    internal static class ExpressionHelper
    {
        internal static void CollectRelationalMembers(Expression exp, IList<PropertyInfo> members)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Lambda:
                    CollectRelationalMembers(((LambdaExpression)exp).Body, members);
                    break;
                case ExpressionType.MemberAccess:
                    var mexp = (MemberExpression)exp;
                    CollectRelationalMembers(mexp.Expression, members);
                    members.Add((PropertyInfo)mexp.Member);

                    break;
                case ExpressionType.Call:
                    var cexp = (MethodCallExpression)exp;

                    if (cexp.Method.IsStatic == false)
                        throw new InvalidOperationException("Invalid type of expression.");

                    foreach (var arg in cexp.Arguments)
                        CollectRelationalMembers(arg, members);

                    break;
                case ExpressionType.Parameter:
                    return;
                default:
                    throw new InvalidOperationException("Invalid type of expression.");
            }
        }
    }
}
