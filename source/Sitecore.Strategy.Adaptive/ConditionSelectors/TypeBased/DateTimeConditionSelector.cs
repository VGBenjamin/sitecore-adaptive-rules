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
    public class DateTimeConditionSelector : BaseDateTimeConditionSelector
    {
        public DateTimeConditionSelector() 
            : base(typeof(DateTime))
        {
        }        
    }
}