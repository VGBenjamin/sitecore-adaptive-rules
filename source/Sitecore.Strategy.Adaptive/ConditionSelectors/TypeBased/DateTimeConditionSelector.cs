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
    public class DateTimeConditionSelector : ConditionSelectorForTypeBase
    {
        public DateTimeConditionSelector() 
            : base(typeof(DateTime))
        {
        }

        public override RuleCondition<T> GetCondition<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext) 
        {
            var condition = new DateTimeCompareCondition<T>();
            var left = adaptiveCondition.GetLeftValue(ruleContext);
            if (left != null)
            {
                condition.LeftValue = DateUtil.ParseDateTime(left.ToString(), DateTime.MinValue);
            }
            var right = adaptiveCondition.GetRightValue(ruleContext);
            if (right != null)
            {
                condition.RightValue = DateUtil.ParseDateTime(right.ToString(), DateTime.MinValue);
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
            DateTime rightDateTime;
            rightDateTime = DateUtil.ParseDateTime(right.ToString(), DateTime.MinValue);
            if (rightDateTime == DateTime.MinValue)
            {
                return null;
            }

            var conditionOperator = ConditionUtility.GetConditionOperatorById(adaptiveCondition.Operator.ToString());
            return GetCompareExpression<DateTime>(conditionOperator, c => (DateTime)c[(ObjectIndexerKey)leftFacetName], rightDateTime);
        }
    }
}