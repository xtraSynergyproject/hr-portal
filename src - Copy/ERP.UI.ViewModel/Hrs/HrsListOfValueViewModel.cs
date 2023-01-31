using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class HrsListOfValueViewModel : ViewModelBase
    {
        [Required]
        public string LOVTypeCode { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string Description { get; set; }
        public string ParentCode { get; set; }
        public long? SequenceNo { get; set; }
    }
}
