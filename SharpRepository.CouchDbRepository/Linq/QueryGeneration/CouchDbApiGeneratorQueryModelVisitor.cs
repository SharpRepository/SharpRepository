using System;
using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;

namespace SharpRepository.CouchDbRepository.Linq.QueryGeneration
{
    public class CouchDbApiGeneratorQueryModelVisitor : QueryModelVisitorBase
    {
        public static CommandData GenerateCouchDbApiQuery(QueryModel queryModel)
        {
            var visitor = new CouchDbApiGeneratorQueryModelVisitor();
            visitor.VisitQueryModel(queryModel);
            return visitor.GetCouchDbApiCommand();
        }

        // Instead of generating an HQL string, we could also use a NHibernate ASTFactory to generate IASTNodes.
        private readonly QueryPartsAggregator _queryParts = new QueryPartsAggregator();

        public CommandData GetCouchDbApiCommand()
        {
            return new CommandData(_queryParts);
        }

        public override void VisitQueryModel(QueryModel queryModel)
        {
            queryModel.SelectClause.Accept(this, queryModel);
            queryModel.MainFromClause.Accept(this, queryModel);
            VisitBodyClauses(queryModel.BodyClauses, queryModel);
            VisitResultOperators(queryModel.ResultOperators, queryModel);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            if (resultOperator is FirstResultOperator)
            {
                _queryParts.Take = 1;
                return;
            }

            if (resultOperator is CountResultOperator || resultOperator is LongCountResultOperator)
            {
                _queryParts.ReturnCount = true;
                return;
            }

            if (resultOperator is TakeResultOperator)
            {
                var exp = ((TakeResultOperator)resultOperator).Count;

                if (exp.NodeType == ExpressionType.Constant)
                {
                    _queryParts.Take = (int)((ConstantExpression)exp).Value;
                }
                else
                {
                    throw new NotSupportedException("Currently not supporting methods or variables in the Skip or Take clause.");
                }

                return;
            }

            if (resultOperator is SkipResultOperator)
            {
                var exp = ((SkipResultOperator) resultOperator).Count;

                if (exp.NodeType == ExpressionType.Constant)
                {
                    _queryParts.Skip = (int)((ConstantExpression)exp).Value;
                }
                else
                {
                    throw new NotSupportedException("Currently not supporting methods or variables in the Skip or Take clause.");
                }
                
                return;
            }

            // TODO: implement count by returning the total_rows value in the json returned from the query
//            if (resultOperator is CountResultOperator)
//                _queryParts.SelectPart = string.Format("cast(count({0}) as int)", _queryParts.SelectPart);
//            else
            //throw new NotSupportedException("No aggregates are supported in the select statement");
//                throw new NotSupportedException("Only Count() result operator is showcased in this sample. Adding Sum, Min, Max is left to the reader.");

            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {
            _queryParts.AddFromPart(fromClause);

            base.VisitMainFromClause(fromClause, queryModel);
        }

        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            _queryParts.SelectPart = GetCouchDbApiExpression(selectClause.Selector);

            base.VisitSelectClause(selectClause, queryModel);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            _queryParts.AddWherePart(GetCouchDbApiExpression(whereClause.Predicate));

            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            if (orderByClause.Orderings.Any())
            {
                _queryParts.AddOrderByPart(GetCouchDbApiExpression(orderByClause.Orderings[0].Expression), orderByClause.Orderings[0].OrderingDirection == OrderingDirection.Desc);
            }

            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
        {
            // HQL joins work differently, need to simulate using a cross join with a where condition

            _queryParts.AddFromPart(joinClause);
            _queryParts.AddWherePart(
                "({0} = {1})",
                GetCouchDbApiExpression(joinClause.OuterKeySelector),
                GetCouchDbApiExpression(joinClause.InnerKeySelector));

            base.VisitJoinClause(joinClause, queryModel, index);
        }

        public override void VisitAdditionalFromClause(AdditionalFromClause fromClause, QueryModel queryModel, int index)
        {
            _queryParts.AddFromPart(fromClause);

            base.VisitAdditionalFromClause(fromClause, queryModel, index);
        }

        public override void VisitGroupJoinClause(GroupJoinClause groupJoinClause, QueryModel queryModel, int index)
        {
            throw new NotSupportedException("Adding a join ... into ... implementation to the query provider is left to the reader for extra points.");
        }

        private string GetCouchDbApiExpression(Expression expression)
        {
            return CouchDbApiGeneratorExpressionTreeVisitor.GetCouchDbApiExpression(expression);
        }
    }
}
