using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class NtsListOfValueViewModel : ViewModelBase
    {
        public long ListOfValueId { get; set; }
        //[Required]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        public long LOVTypeId { get; set; }
        //[Required]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        public string Code { get; set; }
        //[Required]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string Description { get; set; }
        public long? ParentId { get; set; }
        public long? SequenceNo { get; set; }
    }
}
