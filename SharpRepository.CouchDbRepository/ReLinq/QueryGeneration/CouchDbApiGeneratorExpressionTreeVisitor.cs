using System;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;

namespace SharpRepository.CouchDbRepository.ReLinq.QueryGeneration
{
    public class CouchDbApiGeneratorExpressionTreeVisitor : ThrowingExpressionTreeVisitor
    {
        public static string GetCouchDbApiExpression (Expression linqExpression, ParameterAggregator parameterAggregator)
        {
          var visitor = new CouchDbApiGeneratorExpressionTreeVisitor (parameterAggregator);
          visitor.VisitExpression (linqExpression);
          return visitor.GetCouchDbApiExpression();
        }

        private readonly StringBuilder _expression = new StringBuilder ();
        private readonly ParameterAggregator _parameterAggregator;

        private CouchDbApiGeneratorExpressionTreeVisitor(ParameterAggregator parameterAggregator)
        {
          _parameterAggregator = parameterAggregator;
        }

        public string GetCouchDbApiExpression()
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
          _expression.Append ("(");

          VisitExpression (expression.Left);

          // In production code, handle this via lookup tables.
          switch (expression.NodeType)
          {
            case ExpressionType.Equal:
              _expression.Append (" == ");
              break;

            case ExpressionType.AndAlso:
            case ExpressionType.And:
              _expression.Append (" && ");
              break;
        
            case ExpressionType.OrElse:
            case ExpressionType.Or:
              _expression.Append (" || ");
              break;

            case ExpressionType.Add:
              _expression.Append (" + ");
              break;

            case ExpressionType.Subtract:
              _expression.Append (" - ");
              break;

            case ExpressionType.Multiply:
              _expression.Append (" * ");
              break;

            case ExpressionType.Divide:
              _expression.Append (" / ");
              break;

            default:
              base.VisitBinaryExpression (expression);
              break;
          }

          VisitExpression (expression.Right);
          _expression.Append (")");

          return expression;
        }

        protected override Expression VisitMemberExpression (MemberExpression expression)
        {
          VisitExpression (expression.Expression);
          _expression.AppendFormat (".{0}", expression.Member.Name);

          return expression;
        }

        protected override Expression VisitConstantExpression (ConstantExpression expression)
        {
          //var namedParameter = _parameterAggregator.AddParameter (expression.Value);
            
            // check to see if we don't need the quotes
            var value = expression.Value;
            var quotes = "'";
            if (
                expression.Type == typeof(Int32)
                || expression.Type == typeof(Int16)
                || expression.Type == typeof(Int64)
                || expression.Type == typeof(Decimal)
                || expression.Type == typeof(Double)
                || expression.Type == typeof(Boolean)
                )
            {
                quotes = "";
            }

                _expression.AppendFormat("{1}{0}{1}", expression.Value, quotes);

          return expression;
        }

        protected override Expression VisitMethodCallExpression (MethodCallExpression expression)
        {
          // In production code, handle this via method lookup tables.

            if (expression.Method.Name == "Contains")
            {
                _expression.Append ("(doc.");
                VisitExpression (expression.Object);
                _expression.Append (".indexOf('");
                VisitExpression (expression.Arguments[0]);
                _expression.Append ("') != -1)");
                return expression;
            }

            if (expression.Method.Name == "StartsWith")
            {
                _expression.Append("(doc.");
                VisitExpression(expression.Object);
                _expression.Append(".indexOf('");
                VisitExpression(expression.Arguments[0]);
                _expression.Append("') == 0)");
                return expression;
            }

            if (expression.Method.Name == "EndsWith")
            {
                _expression.Append("(doc.");
                VisitExpression(expression.Object);
                _expression.Append(".indexOf('");
                VisitExpression(expression.Arguments[0]);
                _expression.Append("', ");
                VisitExpression(expression.Object);
                _expression.Append(".length - '");
                VisitExpression(expression.Arguments[0]);
                _expression.Append("'.length) != -1)");
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
