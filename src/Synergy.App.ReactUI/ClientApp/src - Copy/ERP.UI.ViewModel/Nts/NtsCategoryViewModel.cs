using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class NtsCategoryViewModel : ViewModelBase
    {
        public long NtsCategoryId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string Name { get; set; }
        public string Description { get; set; }
        public long? Level { get; set; }
        public long? ParentId { get; set; }
    }
}
