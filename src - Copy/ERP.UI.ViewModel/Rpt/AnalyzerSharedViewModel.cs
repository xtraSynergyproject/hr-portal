using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class AnalyzerSharedViewModel: ViewModelBase
    {
      
        public long AnalyzerSharedId { get; set; }
        public long AnalyzerId { get; set; }
        [Display(Name = "SharedType")]
        public AssignToTypeEnum? SharedType { get; set; }
        public long? SharedTo { get; set; }
        [Display(Name = "SharedToUserName")]
        public string SharedToUserName { get; set; }
        public string SharedTypeDisplay
        {
            get
            {
                return SharedType?.ToString();
            }
        }
       
    }
}
