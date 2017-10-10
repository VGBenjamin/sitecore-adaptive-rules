using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Strategy.Adaptive.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sitecore.ContentSearch.Analytics.Models;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Strategy.Adaptive.ConditionSelectors.Utility;

namespace Sitecore.Strategy.Adaptive.ConditionSelectors.TypeBased
{
    public class IdConditionSelector : ConditionSelectorForTypeBase
    {
        public IdConditionSelector()
            : base(typeof(ID))
        {
        }

        public override RuleCondition<T> GetCondition<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext) 
        {
            var condition = new IdCompareCondition<T>();
            var left = adaptiveCondition.GetLeftValue(ruleContext);
            if (left != null)
            {
                condition.LeftValue = left.ToString();
            }
            var right = adaptiveCondition.GetRightValue(ruleContext);
            if (right != null)
            {
                condition.RightValue = right.ToString();
            }
            condition.OperatorId = adaptiveCondition.Operator.ToString();
            return condition;
        }

        public override Expression<Func<IndexedContact, bool>> GetPredicate<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext)
        {
            var leftFacetName = adaptiveCondition.GetLeftValue(ruleContext).ToString();

            object right = adaptiveCondition.GetRightValue(ruleContext);
            if (right == null)
                return null;
            Guid rightGuid;
            if (!Guid.TryParse(right.ToString(), out rightGuid))
            {
                return null;
            }

            var conditionOperator = ConditionUtility.GetConditionOperatorById(adaptiveCondition.Operator.ToString());
            return GetCompareExpression<Guid>(conditionOperator, c => (Guid)c[(ObjectIndexerKey)leftFacetName], rightGuid);
        }
    }
}