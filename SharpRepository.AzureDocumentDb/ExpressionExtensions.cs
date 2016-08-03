using System;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using SharpRepository.Repository.Helpers;

namespace SharpRepository.AzureDocumentDb
{
    public static class ExpressionExtensions
    {
        public static string ToMSSqlString(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    var add = expression as BinaryExpression;
                    return add.Left.ToMSSqlString() + " + " + add.Right.ToMSSqlString();

                case ExpressionType.Constant:
                    var constant = expression as ConstantExpression;
                    if (constant.Type == typeof(string))
                        return "'" + constant.Value.ToString().Replace("'", "''") + "'";
                    return constant.Value.ToString();

                case ExpressionType.Equal:
                    var equal = expression as BinaryExpression;
                    return equal.Left.ToMSSqlString() + " = " +
                           equal.Right.ToMSSqlString();

                case ExpressionType.Lambda:
                    var lambda = expression as LambdaExpression;
                    return lambda.Body.ToMSSqlString();

                case ExpressionType.MemberAccess:
                    var memberaccess = expression as MemberExpression;

                    return GetJsonPropNameAttribute(memberaccess.Member) ?? memberaccess.Member.Name;
            }

            throw new NotImplementedException(
                expression.GetType() + " " +
                expression.NodeType);
        }

        private static string GetJsonPropNameAttribute(MemberInfo memberInfo)
        {
            var jsonProp = memberInfo.GetOneAttribute<JsonPropertyAttribute>();

            if (jsonProp != null)
                return jsonProp.PropertyName;
            return null;
        }
    }
}