using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace ERP.UI.ViewModel
    {
    public class MaintenancesViewModel : ViewModelBase
    {

        public long? ServiceId { get; set; }
        public int MaintenanceNumber { get; set; }

        [Display(Name = "Tenant Name")]
        public string TenantName { get; set; }

        //public string Project { get; set; }

        public string Vendor { get; set; }

        [Required]
        public long? VendorId { get; set; }
        [Display(Name = "Service No.")]
        public string ServiceNo { get; set; }
        public string Unit { get; set; }
        [Required]
        public long? UnitId { get; set; }
        [Required]
        [Display(Name = "Maintenance Category")]
        public string MaintenanceCategory { get; set; }

        public string MaintenanceCategoryName { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        public long? PropertyId { get; set; }

        [Display(Name = "Project Name")]
        public string PropertyName { get; set; }

        [Required]

        [Display(Name = "Title")]
        public string TitleName { get; set; }

        public string Details { get; set; }

        [Required]
        [Display(Name = "Availability Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AvailabilityDate { get; set; }
        

        [Display(Name = "Closed Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CompletionDate { get; set; }

        public TimeSlotTypeEnum? TimeSlot { get; set; }
        public MaintenanceStatusEnum? MaintenanceStatus { get; set; }
        public IList<FileViewModel> BeforePhoto { get; set; }
        public IList<FileViewModel> AfterPhoto { get; set; }
        public IList<FileViewModel> AdditionalDoc { get; set; }


        [Display(Name = "Before Maintenance Photo")]
        public string BeforePhotoAttachmentId { get; set; }
        [Display(Name = "Before Maintenance Photo")]
        public FileViewModel BeforePhotoSelectedFile { get; set; }


        [Display(Name = "After Maintenance Photo")]
        public string AfterPhotoAttachmentId { get; set; }

        [Display(Name = "After Maintenance Photo")]
        public FileViewModel AfterPhotoSelectedFile { get; set; }


        [Display(Name = "Additional Attchment")]
        public string AdditionalDocAttachmentId { get; set; }
        [Display(Name = "Additional Attchment")]
        public FileViewModel AdditionalDocSelectedFile { get; set; }

        [Display(Name = " Attachment Description")]
        public string BeforePhotoDescription { get; set; }

        [Display(Name = "Attachment Description")]
        public string AfterPhotoDescription { get; set; }

        [Display(Name = "Attachment Description")]
        public string AdditionalDocDescription { get; set; }
        [Display(Name = "Work Order No")]
        public string WorkOrderNo { get; set; }    
        public string MaintenanceCost { get; set; }
    }
}
