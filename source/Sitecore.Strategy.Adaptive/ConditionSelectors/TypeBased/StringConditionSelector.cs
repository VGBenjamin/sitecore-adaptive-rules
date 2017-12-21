using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Strategy.Adaptive.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sitecore.Analytics.Rules.SegmentBuilder;
using Sitecore.ContentSearch.Analytics.Models;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Strategy.Adaptive.ConditionSelectors.Utility;

namespace Sitecore.Strategy.Adaptive.ConditionSelectors.TypeBased
{
    public class StringConditionSelector : ConditionSelectorForTypeBase
    {
        public StringConditionSelector() 
            :base(new HashSet<Type>()
            {
                typeof (string),
                typeof(bool)                
            })
        {
        }

        public override RuleCondition<T> GetCondition<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext) 
        {
            var condition = new StringCompareCondition<T>();
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
            if (adaptiveCondition.Operator == Sitecore.Strategy.Adaptive.Items.ItemIDs.BooleanOperatorTrue)
            {
                adaptiveCondition.Operator = Sitecore.Strategy.Adaptive.Items.ItemIDs.StringOperatorTrue;
            }
            else if (adaptiveCondition.Operator == Sitecore.Strategy.Adaptive.Items.ItemIDs.BooleanOperatorFalse)
            {
                adaptiveCondition.Operator = Sitecore.Strategy.Adaptive.Items.ItemIDs.StringOperatorFalse;
            }
            condition.OperatorId = adaptiveCondition.Operator.ToString();
            return condition;
        }

        public override Expression<Func<IndexedContact, bool>> GetPredicate<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext)
        {
            var leftFacetName = adaptiveCondition.GetLeftValue(ruleContext).ToString();

            var right = adaptiveCondition.GetRightValue(ruleContext)?.ToString();
            if (right == null)
            {
                return null;
            }

            if (adaptiveCondition.Operator == Sitecore.Strategy.Adaptive.Items.ItemIDs.BooleanOperatorTrue)
            {
                adaptiveCondition.Operator = Sitecore.Strategy.Adaptive.Items.ItemIDs.StringOperatorTrue;
                /*return StringConditionOperator.Contains.Compare<IndexedContact>(
                        c => (string) c[(ObjectIndexerKey)leftFacetName], right);*/
            }
            else if (adaptiveCondition.Operator == Sitecore.Strategy.Adaptive.Items.ItemIDs.BooleanOperatorFalse)
            {
                adaptiveCondition.Operator = Sitecore.Strategy.Adaptive.Items.ItemIDs.StringOperatorFalse;
                /*return StringConditionOperator.Contains.Compare<IndexedContact>(
                        c => (string)c[(ObjectIndexerKey)leftFacetName], right);*/
            }
            var conditionOperator = ConditionUtility.GetStringConditionOperatorById(adaptiveCondition.Operator.ToString());

            return conditionOperator.Compare<IndexedContact>(c => (string)c[(ObjectIndexerKey)leftFacetName], right);
        }
    }
}