
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class HierarchyLevelViewModel : ViewModelBase
    {

        [Required]
        [Display(Name = "Hierarchy Level Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name = "Sequence No")]
        public int? SequenceNo { get; set; }

        [Display(Name = "Hierarchy Name")]
        public long HierarchyId { get; set; }

        [Display(Name = "Hierarchy Name")]
        public string HierarchyName { get; set; }

        [Display(Name = "User")]
        
        public long? UserId { get; set; }      

        [UIHint("TypeEditor1")]
        public long? Option1 { get; set; }
        [UIHint("TypeEditor2")]
        public long? Option2 { get; set; }
        [UIHint("TypeEditor3")]
        public long? Option3 { get; set; }

        public string OptionName1 { get; set; }
        public string OptionName2 { get; set; }
        public string OptionName3 { get; set; }
       
    }
}
