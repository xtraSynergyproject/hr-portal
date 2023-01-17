using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class UserPromotionSubmit
    {
        public List<UserPromotionViewModel> Created { get; set; }
        public List<UserPromotionViewModel> Updated { get; set; }
        public List<UserPromotionViewModel> Destroyed { get; set; }
    }
}
