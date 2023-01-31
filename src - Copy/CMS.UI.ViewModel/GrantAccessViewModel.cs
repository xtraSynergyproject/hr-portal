using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMS.Common;
using CMS.Data.Model;

namespace CMS.UI.ViewModel
{
    public class GrantAccessViewModel : GrantAccess
    {
        public string UserName { get; set; }
    }
}
