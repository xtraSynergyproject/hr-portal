using CMS.Data.Model;
using System;
using System.Collections.Generic;
using CMS.Common;

namespace CMS.UI.ViewModel
{
   public class UserGroupViewModel:UserGroup
    {

        public List<string> UserIds { get; set; }

        public IList<string> Portal { get; set; }
        public string PortalName { get; set; }

    }
}
