using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.UI.ViewModel
{
    public class NotificationTemplateViewModel : NotificationTemplate
    {
        public string ActionStatusCode { get; set; }
    }
}
