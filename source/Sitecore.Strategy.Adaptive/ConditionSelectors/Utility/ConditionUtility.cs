using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.Diagnostics;
using Sitecore.Rules.Conditions;

namespace Sitecore.Strategy.Adaptive.ConditionSelectors.Utility
{
    public static class ConditionUtility
    {
        private static Type baseConditionUtilityType;
        private static MethodInfo getStringConditionOperatorByIdMethodInfo;
        private static MethodInfo getConditionOperatorByIdMethodInfo;
        static ConditionUtility()
        {
            baseConditionUtilityType = typeof(ConditionOperator).Assembly.GetType("Sitecore.Rules.Conditions.ConditionsUtility");
            if(baseConditionUtilityType == null)
                Log.Error("Cannot load the type: 'Sitecore.Rules.Conditions.ConditionsUtility'", typeof(ConditionUtility));

            getStringConditionOperatorByIdMethodInfo = baseConditionUtilityType.GetMethod("GetStringConditionOperatorById", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (getStringConditionOperatorByIdMethodInfo == null)
                Log.Error("Cannot find the method: 'GetStringConditionOperatorById' in the type 'Sitecore.Rules.Conditions.ConditionsUtility'", typeof(ConditionUtility));

            getConditionOperatorByIdMethodInfo = baseConditionUtilityType.GetMethod("GetConditionOperatorById", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (getConditionOperatorByIdMethodInfo == null)
                Log.Error("Cannot find the method: 'GetConditionOperatorById' in the type 'Sitecore.Rules.Conditions.ConditionsUtility'", typeof(ConditionUtility));
        }

        public static StringConditionOperator GetStringConditionOperatorById(string conditionOperatorId) 
            => (StringConditionOperator)getStringConditionOperatorByIdMethodInfo.Invoke(null, new object[] { conditionOperatorId });

        public static ConditionOperator GetConditionOperatorById(string conditionOperatorId) 
            => (ConditionOperator)getConditionOperatorByIdMethodInfo.Invoke(null, new object[] { conditionOperatorId });
    }
}