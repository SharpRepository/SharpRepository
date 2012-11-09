using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching.Hash
{
    public class HashGenerator
    {
        public static string FromSpecification<T>(ISpecification<T> specification)
        {
            return FromPredicate<T>(specification.Predicate);
        }

        public static string FromPredicate<T>(Expression<Func<T,bool>> predicate)
        {
            return FromPredicate(predicate.Body);
        }

        public static string FromPredicate(Expression expression)
        {
            return new HashGeneratorHelper(expression).GetHash();
        }
    }

    internal class HashGeneratorHelper
    {
        private readonly Expression _expression;
        private string _values;

        public HashGeneratorHelper(Expression expression)
        {
            _expression = expression;
        }

        public string GetHash()
        {
            if (_expression == null)
                return null;

            var expression = _expression;

             // locally evaluate as much of the query as possible
            expression = Evaluator.PartialEval(expression);

            // support local collections
            expression = LocalCollectionExpander.Rewrite(expression);

            // use the string representation of the expression for the cache key
            string key = expression.ToString();

            // the key is potentially very long, so use an md5 fingerprint
            // (fine if the query result data isn't critically sensitive)
            //key = key.ToMd5Fingerprint();



            return key;
        }

           
    }

    internal class HashGeneratorHelper2<T>
    {
        private readonly ISpecification<T> _specification;
        private string _values;

        public HashGeneratorHelper2(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public string GetHash()
        {
            if (_specification == null || _specification.Predicate == null || _specification.Predicate.Body == null)
                return null;

            VisitExpression(_specification.Predicate.Body);

            var hash = _specification.ToString();

            if (!String.IsNullOrEmpty(_values))
            {
                hash += String.Format("values[{0}]", _values);
            }

            return hash;
        }
        private void VisitExpression(Expression expression)
        {
            if (expression == null)
                return;

            switch (expression.NodeType)
            {
//                case ExpressionType.Negate:
//                case ExpressionType.NegateChecked:
//                case ExpressionType.Not:
//                case ExpressionType.Convert:
//                case ExpressionType.ConvertChecked:
//                case ExpressionType.ArrayLength:
//                case ExpressionType.Quote:
//                case ExpressionType.TypeAs:
//                case ExpressionType.UnaryPlus:
//                    VisitUnary((UnaryExpression)expression);
//                    break;

                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.Power:
                    VisitBinary((BinaryExpression)expression);
                    break;

                case ExpressionType.MemberAccess:
                    VisitMemberAccess((MemberExpression)expression);
                    break;

                case ExpressionType.Constant:
                    VisitConstant((ConstantExpression)expression);
                    break;

                default:
                    VisitGeneric(expression);
                    break;
            }
        }

        private void VisitGeneric(Expression expression)
        {
            // nothing to do
        }

        private void VisitUnary(UnaryExpression u)
        {
            VisitExpression(u.Operand);
        }

        private void VisitBinary(BinaryExpression b)
        {
            VisitExpression(b.Left);
            VisitExpression(b.Right);
        }

        private void VisitConstant(ConstantExpression c)
        {
            if (!String.IsNullOrEmpty(_values))
                return; // only need to get this once, it has all the values

//            _values = c.Value.GetHashCode().ToString();
//            return;

            var type = c.Value.GetType();
            foreach (var fieldInfo in type.GetFields())
            {
                string value;

                var fieldValue = fieldInfo.GetValue(c.Value);

                if (fieldValue == null)
                {
                    value = "-null-";
                }
                else
                {
                    var fieldType = fieldValue.GetType();

                    if (fieldType != typeof(string) && GetEnumerableType(fieldType) != null)
                    {
                        value = "IENUMERABLE TODO";
                    }
                    else
                    {
                        value = fieldValue.ToString();
                    }
                }

                _values += fieldInfo.Name + "=" + value + ";";
            }
        }

        private void VisitMemberAccess(MemberExpression m)
        {
            VisitExpression(m.Expression);
        }

        static Type GetEnumerableType(Type type)
        {
            return (from intType in type.GetInterfaces() where intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof (IEnumerable<>) select intType.GetGenericArguments()[0]).FirstOrDefault();
        }
    }
}
