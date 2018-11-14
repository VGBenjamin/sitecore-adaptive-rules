using Sitecore.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Strategy.Adaptive.ConditionSelectors.TypeBased
{
    public class NullableDateTimeConditionSelector : BaseDateTimeConditionSelector
    {
        public NullableDateTimeConditionSelector() 
            : base(typeof(Nullable<DateTime>))
        {
        }        
    }
}