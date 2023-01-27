

using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ProjectManagementViewModel : ViewModelBase
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        public long OwnerUserId { get; set; }
        public long? ServiceId { get; set; }
        public string Type { get; set; }
        public string ProjectStatus { get; set; }
        public int Count { get; set; }
        public NtsPriorityEnum Priority { get; set; }
    }
}
