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
    public class NumberConditionSelector : ConditionSelectorForTypeBase
    {

        public NumberConditionSelector() 
            : base (new HashSet<Type>() 
                    {
                        typeof(short) ,
                        typeof(ushort) , 
                        typeof(int) , 
                        typeof(uint) ,
                        typeof(long) ,
                        typeof(ulong) , 
                        typeof(float) ,
                        typeof(double)         
                    })
        {
        }

        public override RuleCondition<T> GetCondition<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext) 
        {
            var condition = new NumberCompareCondition<T>();
            var left = adaptiveCondition.GetLeftValue(ruleContext);
            if (left != null) 
            {
                condition.LeftValue = double.Parse(left.ToString());
            }
            var right = adaptiveCondition.GetRightValue(ruleContext);
            if (right != null) 
            {
                condition.RightValue= double.Parse(right.ToString());
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
            int rightInt;
            if (!int.TryParse(right.ToString(), out rightInt))
            {
                return null;
            }

            var conditionOperator = ConditionUtility.GetConditionOperatorById(adaptiveCondition.Operator.ToString());
            return GetCompareExpression<int>(conditionOperator, c => (int)c[(ObjectIndexerKey)leftFacetName], rightInt);
        }        
    }
}