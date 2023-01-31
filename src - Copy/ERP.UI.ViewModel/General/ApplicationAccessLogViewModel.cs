using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class ApplicationAccessLogViewModel : ViewModelBase
    {
        public long? UserId { get; set; }
        public string Email { get; set; }
        public string IqamaNo { get; set; }
        public string MobileNo { get; set;}
        public AccessLogTypeEnum AccessType { get; set; }
        public string Url { get; set; }
        public string PersonFullName { get; set;}
        public string SessionId { get; set; }
        public string ClientIP { get; set;}
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public DateTime LogDate { get; set; }
        public ModuleEnum? ModuleName { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}
