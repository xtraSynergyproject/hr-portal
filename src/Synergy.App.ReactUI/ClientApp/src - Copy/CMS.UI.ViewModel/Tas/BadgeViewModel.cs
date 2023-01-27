using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class BadgeViewModel
    {
        public string BadgeName { get; set; }
        public string BadgeDescription { get; set; }
        public string BadgeImage { get; set; }

    }
}
