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
    public abstract class SegmentationAdaptiveConditionBase<T> : BaseAdaptiveConditionBase<T>, IQueryableRule<IndexedContact> where T : RuleContext
    {
      
        public override void Evaluate(T ruleContext, RuleStack stack)
        {
            var predicate = GetPredicate(ruleContext);
            if (predicate == null)
            {
                this.ApplyFilter(ruleContext, (Expression<Func<IndexedContact, bool>>)(c => false));
                stack.Push(false);
                return;
            }

            this.ApplyFilter(ruleContext, predicate);
            stack.Push(true);
        }

        private Expression<Func<IndexedContact, bool>> GetPredicate(T ruleContext)
        {
            var type = GetDataType(ruleContext);
            if (type == null)
            {
                return null;
            }
            return AdaptiveManager.Provider.GetRulePredicate<T>(type, this, ruleContext);
        }

        private void ApplyFilter(T ruleContext, Expression<Func<IndexedContact, bool>> expression)
        {
            var castedRuleContext = ruleContext as QueryableRuleContext<IndexedContact>;

            var predicate = (InitPredicate ?? castedRuleContext.Where);
            this.ResultPredicate = Sitecore.Analytics.Rules.SegmentBuilder.PredicateBuilder.And<IndexedContact>(predicate, expression);
            if (this.InitPredicate != null)
                return;
            castedRuleContext.Where = this.ResultPredicate;
        }

        public Expression<Func<IndexedContact, bool>> InitPredicate
        {
            protected get;
            set;
        }

        public Expression<Func<IndexedContact, bool>> ResultPredicate
        {
            get;
            protected set;
        }
    }
}