
using System.ComponentModel.DataAnnotations;


namespace ERP.UI.ViewModel
{
    public class ListOfValueViewModel : ViewModelBase
    {
        public long ListOfValueId { get; set; }
        [Required]
        public string LOVTypeCode { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string Description { get; set; }
        public string ParentCode { get; set; }
        public long? ParentId { get; set; }
        public string ParentName { get; set; }
        public long? SequenceNo { get; set; }
        public string LOVCategory { get; set; }
        public bool? IsUserListOfValue { get; set; }
    }
}
