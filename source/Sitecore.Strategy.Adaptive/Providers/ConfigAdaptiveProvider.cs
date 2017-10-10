using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Rules.Conditions;
using Sitecore.Rules.RuleMacros;
using Sitecore.Strategy.Adaptive.Config;
using Sitecore.Strategy.Adaptive.MacroSelectors;
using Sitecore.Strategy.Adaptive.Rules;
using Sitecore.Strategy.Adaptive.Rules.Conditions;
using Sitecore.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Xml.Linq;
using Sitecore.ContentSearch.Analytics.Models;
using Sitecore.Rules;

namespace Sitecore.Strategy.Adaptive.Providers
{
    public class ConfigAdaptiveProvider : AdaptiveProviderBase
    {
        public ConfigAdaptiveProvider()
        {
            this.MacroConfiguration = Factory.CreateObject("adaptiveRules/macros", true) as MacroConfiguration;
        }
        protected MacroConfiguration MacroConfiguration { get; private set; }
        
        public override IRuleMacro GetTreeMacro(XElement element, string name, UrlString parameters)
        {
            var item = RuleHelper.GetItemFromParameters("dependency", element, parameters);
            Assert.IsNotNull(item, "The parameter 'dependency' was not specified or could not be resolved");

            var selectorConfig = this.MacroConfiguration.GetMacroSelectorConfig(item);
            IMacroSelectorForItem selector = null;
            if (selectorConfig != null) 
            {
                selector = selectorConfig.GetTreeSelector(item);
            }
            if (selector == null)
            {
                selector = this.MacroConfiguration.DefaultTreeSelector;
            }
            if (selector == null)
            {
                return null;
            }
            var macro = selector.GetMacro(item);
            return macro;
        }

        public override IRuleMacro GetOperatorMacro(XElement element, string name, UrlString parameters)
        {
            var item = RuleHelper.GetItemFromParameters("dependency", element, parameters);
            Assert.IsNotNull(item, "The parameter 'dependency' was not specified or could not be resolved");

            var selectorConfig = this.MacroConfiguration.GetMacroSelectorConfig(item);
            Assert.IsNotNull(selectorConfig, "selectorConfig");

            var selector = selectorConfig.GetOperatorSelector(item);
            if (selector == null)
            {
                return null;
            }
            var macro = selector.GetMacro(item);
            return macro;
        }
        
        public override IRuleMacro GetValueMacro(XElement element, string name, UrlString parameters)
        {
            var item = RuleHelper.GetItemFromParameters("dependency", element, parameters);
            Assert.IsNotNull(item, "The parameter 'dependency' was not specified or could not be resolved");

            var selectorConfig = this.MacroConfiguration.GetMacroSelectorConfig(item);
            Assert.IsNotNull(selectorConfig, "selectorConfig");

            var selector = selectorConfig.GetValueSelector(item);
            if (selector == null)
            {
                return null;
            }
            var macro = selector.GetMacro(item);
            return macro;
        }

        public override IRuleMacro GetOperatorMacro(Type type)
        {
            Assert.ArgumentNotNull(type, "type");
            var selectorConfig = this.MacroConfiguration.GetMacroSelectorConfig(type);
            Assert.IsNotNull(selectorConfig, "selectorConfig");
            var selector = selectorConfig.GetOperatorSelector(type);
            if (selector == null)
            {
                return null;
            }
            var macro = selector.GetMacro(type);
            return macro;
        }

        public override IRuleMacro GetValueMacro(Type type)
        {
            Assert.ArgumentNotNull(type, "type");
            var selectorConfig = this.MacroConfiguration.GetMacroSelectorConfig(type);
            Assert.IsNotNull(selectorConfig, "selectorConfig");
            var selector = selectorConfig.GetValueSelector(type);
            if (selector == null)
            {
                return null;
            }
            var macro = selector.GetMacro(type);
            return macro;
        }

        public override RuleCondition<T> GetRuleCondition<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext)
        {
            Assert.ArgumentNotNull(type, "type");
            var selectorConfig = this.MacroConfiguration.GetMacroSelectorConfig(type);
            Assert.IsNotNull(selectorConfig, "selectorConfig");
            var selector = selectorConfig.GetConditionSelector(type);
            if (selector == null)
            {
                return null;
            }
            var condition = selector.GetCondition<T>(type, adaptiveCondition, ruleContext);
            return condition;
        }

        public override Expression<Func<IndexedContact, bool>> GetRulePredicate<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext)
        {
            Assert.ArgumentNotNull(type, "type");
            var selectorConfig = this.MacroConfiguration.GetMacroSelectorConfig(type);
            Assert.IsNotNull(selectorConfig, "selectorConfig");
            var selector = selectorConfig.GetConditionSelector(type);
            if (selector == null)
            {
                return null;
            }
            return selector.GetPredicate<T>(type, adaptiveCondition, ruleContext);
        }
    }
}