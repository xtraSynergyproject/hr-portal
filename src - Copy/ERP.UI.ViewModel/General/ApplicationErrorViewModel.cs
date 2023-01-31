using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ApplicationErrorViewModel : ViewModelBase
    {
        public int? HttpCode { get; set; }
        public string Exception { get; set; }
        public string ErrorMessage { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public string Url { get; set; }
        public bool IsAjaxCall { get; set; }
        public string CustomErrorMessage { get; set; }
        
    }
}
