using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Strategy.Adaptive.sitecore.shell.Applications.Dialogs.RelativeDateTimeDialog
{
    public class RelativeDateTimeDialog : DialogForm
    {
        public const string SYSTEM_DATE_FORMAT = "yyyyMMdd";
        public const string SYSTEM_TIME_FORMAT = "hhmm";
        public static readonly string SYSTEM_DATETIME_FORMAT = $"{SYSTEM_DATE_FORMAT} {SYSTEM_TIME_FORMAT}";

        public static Func<Type, object> DependencyResolver { private get; set; }

        /// <summary>Gets or sets the time.</summary>
        /// <value>The time.</value>
        protected DatePicker AbsoluteDate { get; set; }
        protected DateTime AbsoluteDateAsDateTime => DateUtil.ParseDateTime(AbsoluteDate.Value, DateTime.MinValue);
        protected TimePicker AbsoluteTime { get; set; }
        protected TimeSpan AbsoluteTimeAsTimeSpan => DateUtil.ParseTimeSpan(AbsoluteTime.Value, TimeSpan.MinValue);
        protected Checkbox UseAbsoluteHours { get; set; }
        protected Combobox Sign { get; set; }
        protected Combobox Units { get; set; }

        protected ListItem Add { get; set; }
        protected ListItem Substract { get; set; }

        protected ListItem Hours { get; set; }
        protected ListItem Days { get; set; }
        protected ListItem Months { get; set; }
        protected ListItem Years { get; set; }

        protected Edit Number { get; set; }

        public Radiobutton Computed { get; set; }
        public Radiobutton Fix { get; set; }

        public bool IsFix => HttpContext.Current.Request.Form["TypeOfDate"] == "Fix";
        public bool IsComputed => HttpContext.Current.Request.Form["TypeOfDate"] == "Computed";


        /// <summary>Raises the load event.</summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            
            if (Context.ClientPage.IsEvent)
                return;

            var defaultValue = HttpContext.Current.Request.QueryString["default"];
            if(!string.IsNullOrEmpty(defaultValue))
            {
                if (defaultValue.StartsWith("$now")) //Fixed date
                {
                    var regex = new Regex("^\\$now(?<sign>[+-])(?<number>[0-9]+)(?<unit>[dhmy])$");
                    var match = regex.Match(defaultValue);
                    if (match == null)
                    {
                        Log.Error($"The default value of the relative date doesn't match a relative datetime. Value: {defaultValue}", this);
                    }
                    else
                    {
                        Sign.Value = match.Groups["sign"].Value;
                        Number.Value = match.Groups["number"].Value;
                        Units.Value = match.Groups["unit"].Value;
                        Fix.Checked = false;
                        Computed.Checked = true;
                    }
                }
                else //Fixed date
                {
                    DateTime date;
                    if(!DateTime.TryParseExact(defaultValue, RelativeDateTimeDialog.SYSTEM_DATETIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        Log.Error($"The default value for the fix date format is not correct: {defaultValue}", this);
                    }
                    else
                    {
                        AbsoluteDate.Value = DateUtil.ToIsoDate(date);
                        AbsoluteTime.Value = DateUtil.ToIsoTime(date);
                        Computed.Checked = false;
                        Fix.Checked = true;
                        //new TimeSpan(date.Hour, date.Minute, 0).ToString();
                    }
                }
            }
        }

        /// <summary>Handles a click on the OK button.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        protected override void OnOK(object sender, EventArgs args)
        {
            SheerResponse.SetDialogValue(GetDialogResponse());
            base.OnOK(sender, args);
        }

        private string GetDialogResponse()
        {            
            if(IsFix)
            {
                return $"{AbsoluteDateAsDateTime.ToString(SYSTEM_DATE_FORMAT)} {AbsoluteTimeAsTimeSpan.ToString(SYSTEM_TIME_FORMAT)}";
            }
            else if(IsComputed)
            {
                return $"$now{Sign.Value}{Number.Value}{Units.Value}";
            }
            else
            {
                Log.Error("A date type need to be selected (computed or fix)", this);
                return string.Empty;
            }
            
        }
    }
}
