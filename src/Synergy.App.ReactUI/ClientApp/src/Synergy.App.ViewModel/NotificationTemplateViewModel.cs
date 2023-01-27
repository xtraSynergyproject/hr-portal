using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.ViewModel
{
    public class NotificationTemplateViewModel : NotificationTemplate
    {
        public string ActionStatusCode { get; set; }
    }
}
