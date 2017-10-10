using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Rules.RuleMacros;
using Sitecore.Strategy.Adaptive.Rules.Conditions;
using Sitecore.Text;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Sitecore.ContentSearch.Analytics.Models;

namespace Sitecore.Strategy.Adaptive.Providers
{
    public abstract class AdaptiveProviderBase : ProviderBase
    {
        public abstract IRuleMacro GetTreeMacro(XElement element, string name, UrlString parameters);
        public abstract IRuleMacro GetOperatorMacro(XElement element, string name, UrlString parameters);
        public abstract IRuleMacro GetOperatorMacro(Type type);
        public abstract IRuleMacro GetValueMacro(XElement element, string name, UrlString parameters);
        public abstract IRuleMacro GetValueMacro(Type type);
        public abstract RuleCondition<T> GetRuleCondition<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext) where T : RuleContext;
        public abstract Expression<Func<IndexedContact, bool>> GetRulePredicate<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext) where T : RuleContext;
    }
}
