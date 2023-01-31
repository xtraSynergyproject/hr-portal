using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DocumentRequestViewModel:ViewModelBase
    {
        public long UserId { get; set; }
        public long? ServiceId { get; set; }
        public long DocumentId { get; set; }
        public string ServiceNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AppliedDate { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? RequestStartDate { get; set; }
        [Display(Name = "Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? RequestEndDate { get; set; }
        //[Display(Name = "Created Date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? CreatedDate { get; set; }
        [Display(Name = "Last Updated Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Request Status")]
        public string RequestStatus { get; set; }
        public string DocumentName { get; set; }
        public string RequestType { get; set; }
        public string RequestName { get; set; }
        public string Description { get; set; }
        public string ServiceType { get; set; }
        public string ServiceName { get; set; }
        public string RequestStatusCode { get; set; }


    }
}
