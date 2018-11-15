using Sitecore.Rules.Conditions;
using Sitecore.Strategy.Adaptive.Rules.Conditions;
using System;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Analytics.Models;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Strategy.Adaptive.ConditionSelectors.Utility;
using Sitecore.Strategy.Adaptive.sitecore.shell.Applications.Dialogs.RelativeDateTimeDialog;
using System.Globalization;
using System.Text.RegularExpressions;
using Sitecore.Diagnostics;

namespace Sitecore.Strategy.Adaptive.ConditionSelectors.TypeBased
{
    public abstract class BaseDateTimeConditionSelector : ConditionSelectorForTypeBase
    {
        public BaseDateTimeConditionSelector(Type type) : base(type)
        {

        }

        public override RuleCondition<T> GetCondition<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext)
        {
            var condition = new DateTimeCompareCondition<T>();
            var left = adaptiveCondition.GetLeftValue(ruleContext);
            if (left != null)
            {
                condition.LeftValue = DateUtil.ParseDateTime(left.ToString(), DateTime.MinValue);
            }
            var right = adaptiveCondition.GetRightValue(ruleContext);
            if (right != null)
            {
                condition.RightValue = DateUtil.ParseDateTime(right.ToString(), DateTime.MinValue);
            }
            condition.OperatorId = adaptiveCondition.Operator.ToString();
            return condition;
        }

        public override Expression<Func<IndexedContact, bool>> GetPredicate<T>(Type type, BaseAdaptiveConditionBase<T> adaptiveCondition, T ruleContext)
        {
            var leftFacetName = adaptiveCondition.GetLeftValue(ruleContext).ToString();

            object right = adaptiveCondition.GetRightValue(ruleContext);
            if (right == null)
                return null;
            if (string.IsNullOrEmpty(right.ToString()))
            {
                return null;
            }

            DateTime rightDateTime = RelativeDateTimeAsDateTime(right.ToString());
            if (rightDateTime == DateTime.MinValue)
            {
                return null;
            }

            var conditionOperator = ConditionUtility.GetConditionOperatorById(adaptiveCondition.Operator.ToString());
            return GetCompareExpression<DateTime>(conditionOperator, c => (DateTime)c[(ObjectIndexerKey)leftFacetName], rightDateTime);
        }

        internal DateTime RelativeDateTimeAsDateTime(string relative)
        {
            var date = DateTime.MinValue;
            if (string.IsNullOrEmpty(relative))
                return date;

            if (relative.StartsWith("$now")) //Fixed date
            {
                var regex = new Regex("^\\$now(?<sign>[+-])(?<number>[0-9]+)(?<unit>[dhmy])$");
                var match = regex.Match(relative);
                if (match == null)
                {
                    Log.Error($"The relative date doesn't match a relative datetime. Value: {relative}", this);
                }
                else
                {
                    var sign = match.Groups["sign"].Value == "-" ? -1 : 1;
                    var number = int.Parse(match.Groups["number"].Value);
                    var unit = match.Groups["unit"].Value[0];

                    switch (unit)
                    {
                        case 'h':
                            date = DateTime.Now.AddHours(sign * number);
                            break;
                        case 'd':
                            date = DateTime.Now.AddDays(sign * number);
                            break;
                        case 'm':
                            date = DateTime.Now.AddMonths(sign * number);
                            break;
                        case 'y':
                            date = DateTime.Now.AddYears(sign * number);
                            break;
                    }
                }
            }            
            else //Fixed date
            {
                if (!DateTime.TryParseExact(relative, RelativeDateTimeDialog.SYSTEM_DATETIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    Log.Error($"The fix date format is not correct: {relative}", this);
            }
            return date;
        }
    }    
}