using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Strategy.Adaptive.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Analytics.Models;
using Sitecore.ContentSearch.Rules;

namespace Sitecore.Strategy.Adaptive.Rules.Conditions
{
    public abstract class AdaptiveConditionBase<T> : BaseAdaptiveConditionBase<T> where T : RuleContext
    {      
        public override void Evaluate(T ruleContext, RuleStack stack)
        {
            var condition = GetCondition(ruleContext, stack);
            if (condition == null)
            {
                stack.Push(false);
                return;
            }
            condition.UniqueId = this.UniqueId;
            condition.Evaluate(ruleContext, stack);
        }
    }
}