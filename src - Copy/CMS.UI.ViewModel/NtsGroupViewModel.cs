using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NtsGroupViewModel : NtsGroup
    {
        public List<string> TemplateIds { get; set; }
        public List<string> UserGroupIds { get; set; }
    }
}
