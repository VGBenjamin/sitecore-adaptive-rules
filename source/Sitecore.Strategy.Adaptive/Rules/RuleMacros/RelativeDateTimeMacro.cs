using Sitecore.Diagnostics;
using Sitecore.Rules.RuleMacros;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Sitecore.Strategy.Adaptive.Rules.RuleMacros
{
    public class RelativeDateTimeMacro : IRuleMacro
    {
        public void Execute(XElement element, string name, UrlString parameters, string value)
        {
            Assert.ArgumentNotNull(element, nameof(element));
            Assert.ArgumentNotNull(name, nameof(name));
            Assert.ArgumentNotNull(parameters, nameof(parameters)); 

            Assert.ArgumentNotNull(value, nameof(value));
            var url = new UrlString(UIUtil.GetUri("control:Sitecore.Shell.Applications.Dialogs.RelativeDateTimeDialog"));
            url.Add("default", value);
            SheerResponse.ShowModalDialog(url.ToString(), "580px", "475px", value, true);

        }

    }
}