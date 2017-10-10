using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sitecore.Analytics.Rules.SegmentBuilder;
using Sitecore.ContentSearch.Analytics.Models;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Strategy.Adaptive.Rules.Conditions;

namespace Sitecore.Strategy.Adaptive.ConditionSelectors.TypeBased
{
    public abstract class ConditionSelectorForTypeBase : IConditionSelectorForType
    {
        protected ConditionSelectorForTypeBase () : this(new HashSet<Type>())
        { 
        }

        protected ConditionSelectorForTypeBase(Type type)
            : this(new HashSet<Type>(){type})
        {
        }

        protected ConditionSelectorForTypeBase(HashSet<Type> types)
        {
            ApplicableTypes = types;
        }

        public abstract RuleCondition<T> GetCondition<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition,
            T ruleContext) where T : RuleContext;

        public abstract Expression<Func<IndexedContact, bool>> GetPredicate<T>(Type type,
            BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext) where T : RuleContext;

        public virtual bool DoesApplyToType(Type type)
        {
            return this.ApplicableTypes.Contains(type);
        }

        public HashSet<Type> ApplicableTypes
        {
            get; 
            protected set;
        }

        protected Expression<Func<IndexedContact, bool>> GetCompareExpression<TField>(ConditionOperator conditionOperator, Expression<Func<IndexedContact, TField>> leftExpression, TField value)
            => conditionOperator.Compare(leftExpression, value);

    }
}