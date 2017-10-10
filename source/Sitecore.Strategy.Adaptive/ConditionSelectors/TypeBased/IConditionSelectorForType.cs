using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Strategy.Adaptive.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch.Analytics.Models;

namespace Sitecore.Strategy.Adaptive.ConditionSelectors.TypeBased
{
    public interface IConditionSelectorForType
    {
        bool DoesApplyToType(Type type);
        HashSet<Type> ApplicableTypes { get; }
        RuleCondition<T> GetCondition<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext) where T:RuleContext;
        Expression<Func<IndexedContact, bool>> GetPredicate<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext) where T : RuleContext;
    }
}
