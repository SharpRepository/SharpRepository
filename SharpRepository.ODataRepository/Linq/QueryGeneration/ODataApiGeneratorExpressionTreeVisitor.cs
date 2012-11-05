using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;

namespace SharpRepository.ODataRepository.Linq.QueryGeneration
{
    public class ODataApiGeneratorExpressionTreeVisitor : ThrowingExpressionTreeVisitor
    {
        public static string GetODataApiExpression (Expression linqExpression)
        {
          var visitor = new ODataApiGeneratorExpressionTreeVisitor ();
          visitor.VisitExpression (linqExpression);
          return visitor.GetODataApiExpression();
        }

        private readonly StringBuilder _expression = new StringBuilder ();

        private ODataApiGeneratorExpressionTreeVisitor()
        {
        }

        public string GetODataApiExpression()
        {
          return _expression.ToString ();
        }

        protected override Expression VisitQuerySourceReferenceExpression (QuerySourceReferenceExpression expression)
        {
            _expression.Append("doc"); // not sure if this will always be static like this, need to check for joins
            //_expression.Append (expression.ReferencedQuerySource.ItemName);
            return expression;
        }

        protected override Expression VisitBinaryExpression (BinaryExpression expression)
        {
            _expression.Append("(");

            VisitExpression (expression.Left);

            // In production code, handle this via lookup tables.
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    _expression.Append (" eq ");
                    break;

                case ExpressionType.NotEqual:
                    _expression.Append (" ne ");
                    break;

                case ExpressionType.GreaterThan:
                    _expression.Append (" gt ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    _expression.Append (" ge ");
                    break;

                case ExpressionType.LessThan:
                    _expression.Append (" lt ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    _expression.Append (" le ");
                    break;

                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    _expression.Append (" and ");
                    break;
        
                case ExpressionType.OrElse:
                case ExpressionType.Or:
                    _expression.Append (" or ");
                    break;

                case ExpressionType.Not:
                    _expression.Append(" not ");
                    break;

                case ExpressionType.Add:
                    _expression.Append (" add ");
                    break;

                case ExpressionType.Subtract:
                    _expression.Append (" aub ");
                    break;

                case ExpressionType.Multiply:
                    _expression.Append (" mul ");
                    break;

                case ExpressionType.Divide:
                    _expression.Append (" div ");
                    break;

                case ExpressionType.Modulo:
                    _expression.Append(" mod ");
                    break;

                default:
                    base.VisitBinaryExpression (expression);
                    break;
            }

            VisitExpression (expression.Right);

            _expression.Append(")");

            return expression;
        }

        protected override Expression VisitMemberExpression (MemberExpression expression)
        {
            VisitExpression (expression.Expression);
            _expression.AppendFormat(".{0}", expression.Member.Name);

          return expression;
        }

        protected override Expression VisitConstantExpression (ConstantExpression expression)
        {           
            // check to see if we don't need the quotes
//            var quotes = "'";
//            var value = expression.Value.ToString();
//            if (
//                expression.Type == typeof(Int32)
//                || expression.Type == typeof(Int16)
//                || expression.Type == typeof(Int64)
//                || expression.Type == typeof(Decimal)
//                || expression.Type == typeof(Double)
//                || expression.Type == typeof(Boolean)
//                )
//            {
//                quotes = "";
//            }
//            else if (expression.Type == typeof(DateTime) || expression.Type == typeof(DateTime?))
//            {
//                quotes = "";
//                value = String.Format("new Date('{0}')", value);
//            }
//
//             _expression.AppendFormat("{1}{0}{1}", value, quotes);

            _expression.Append(expression.Value.ToString());

          return expression;
        }

        protected override Expression VisitNewExpression(NewExpression expression)
        {
            // for new { c.Name, c.Title, c.TItle.Length }

            // expression.Members has all the property names of the anonymous type
            //  e.g. String Name, String Title, Int32 Length

            // expression.Arguments has the expressions for getting the value, so these need to be run through the Visit stuff to get their 
            //  e.g. [10001].Name, [10001].Title, [10001].Title.Length

            _expression.Append("{");

            var i = 0;
            foreach (var arg in expression.Arguments)
            {
                if (i != 0)
                    _expression.Append(",");

                _expression.AppendFormat("{0}: ", expression.Members[i].Name);
                VisitExpression(arg);
                i++;
            }
            _expression.Append("}");


            return expression;

            return base.VisitNewExpression(expression);
        }

        protected override Expression VisitMethodCallExpression (MethodCallExpression expression)
        {
          // In production code, handle this via method lookup tables.

            if (expression.Method.Name == "Contains")
            {
                _expression.Append(String.Format("substringof('{1}',{0}) eq true", 
                        VisitExpression(expression.Object),
                        VisitExpression(expression.Arguments[0])));
                return expression;
            }

            if (expression.Method.Name == "StartsWith")
            {
                _expression.Append(String.Format("startswith({0},'{1}') eq true",
                        VisitExpression(expression.Object),
                        VisitExpression(expression.Arguments[0])));
                return expression;
            }

            if (expression.Method.Name == "EndsWith")
            {
                _expression.Append(String.Format("endswith({0},'{1}') eq true",
                        VisitExpression(expression.Object),
                        VisitExpression(expression.Arguments[0])));
                return expression;
            }

            if (expression.Method.Name == "Length")
            {
                _expression.Append(String.Format("length({0})",
                                                 VisitExpression(expression.Object)));
                return expression;
            }

            if (expression.Method.Name == "ToLower")
            {
                _expression.Append(String.Format("tolower({0})",
                                                 VisitExpression(expression.Object)));
                return expression;
            }
            if (expression.Method.Name == "ToUper")
            {
                _expression.Append(String.Format("touper({0})",
                                                 VisitExpression(expression.Object)));
                return expression;
            }

            return base.VisitMethodCallExpression (expression); // throws
        }

        // Called when a LINQ expression type is not handled above.
        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
          string itemText = FormatUnhandledItem(unhandledItem);
          var message = string.Format ("The expression '{0}' (type: {1}) is not supported by this LINQ provider.", itemText, typeof (T));
          return new NotSupportedException (message);
        }

        private static string FormatUnhandledItem<T>(T unhandledItem)
        {
          var itemAsExpression = unhandledItem as Expression;
          return itemAsExpression != null ? FormattingExpressionTreeVisitor.Format (itemAsExpression) : unhandledItem.ToString ();
        }
    }
}
