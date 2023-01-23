using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class UserPromotionSubmit
    {
        public  List<UserPromotionViewModel> Created { get; set; }
        public  List<UserPromotionViewModel> Updated { get; set; }
        public  List<UserPromotionViewModel> Destroyed { get; set; }
    }
}
