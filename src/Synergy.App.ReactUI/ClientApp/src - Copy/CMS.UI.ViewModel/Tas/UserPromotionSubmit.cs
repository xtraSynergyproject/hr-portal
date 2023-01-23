using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class UserPromotionSubmit
    {
        public List<UserPromotionViewModel> Created { get; set; }
        public List<UserPromotionViewModel> Updated { get; set; }
        public List<UserPromotionViewModel> Destroyed { get; set; }
    }
}
