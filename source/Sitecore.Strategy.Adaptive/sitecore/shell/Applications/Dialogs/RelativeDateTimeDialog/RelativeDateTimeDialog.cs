using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Strategy.Adaptive.sitecore.shell.Applications.Dialogs.RelativeDateTimeDialog
{
    public class RelativeDateTimeDialog : DialogForm
    {
        public const string SYSTEM_DATE_FORMAT = "yyyyMMdd";
        public const string SYSTEM_TIME_FORMAT = "hh:mm";
        public static readonly string SYSTEM_DATETIME_FORMAT = $"{SYSTEM_DATE_FORMAT} {SYSTEM_TIME_FORMAT}";

        public static Func<Type, object> DependencyResolver { private get; set; }

        /// <summary>Gets or sets the time.</summary>
        /// <value>The time.</value>
        protected DatePicker AbsoluteDate { get; set; }
        protected DateTime AbsoluteDateAsDateTime => DateTime.ParseExact(AbsoluteDate.Value, AbsoluteDate.Format, CultureInfo.InvariantCulture);
        protected TimePicker AbsoluteTime { get; set; }
        protected TimeSpan AbsoluteTimeAsTimeSpan => TimeSpan.ParseExact(AbsoluteTime.Value, AbsoluteTime.Format, CultureInfo.InvariantCulture);
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

        public bool IsFix => WebUtil.GetFormValue("TypeOfFDate") == "Fix";


        /// <summary>Raises the load event.</summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;            
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
            else
            {
                return $"$now{Sign.Value}{Number.Value}{Units.Value}";
            }
            
        }
    }
}
