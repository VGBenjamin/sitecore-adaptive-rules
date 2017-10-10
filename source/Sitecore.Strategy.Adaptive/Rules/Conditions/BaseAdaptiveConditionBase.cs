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
    public abstract class BaseAdaptiveConditionBase<T> : RuleCondition<T> where T : RuleContext
    {
        /// <summary>
        /// The contact value
        /// </summary>
        /// <param name="ruleContext"></param>
        /// <returns></returns>
        public abstract object GetLeftValue(T ruleContext);

        /// <summary>
        /// The rule value to compare to
        /// </summary>
        /// <param name="ruleContext"></param>
        /// <returns></returns>
        public abstract object GetRightValue(T ruleContext);
        public abstract Type GetDataType(T ruleContext);

        public ID Operator { get; set; }
        public string Value { get; set; }

        protected virtual RuleCondition<T> GetCondition(T ruleContext, RuleStack stack)
        {
            var type = GetDataType(ruleContext);
            if (type == null)
            {
                return null;
            }
            var condition = AdaptiveManager.Provider.GetRuleCondition<T>(type, this, ruleContext);
            return condition;
        }
        
    }
}