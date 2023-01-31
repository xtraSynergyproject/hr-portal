using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class TeamUserViewModel : TeamUser
    {
        public DataActionEnum DataAction { get; set; }
        public string UserName { get; set; }


    }
}
