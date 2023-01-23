using System;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        public string Name { get; set; }
    }
}
