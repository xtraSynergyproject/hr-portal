using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class AvailableTimeRequest
    {
        public long? ServiceId { get; set; }
        public long? TaskId { get; set; }
        public long? UserId { get; set; }
        public string JobTitle { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString  = Constant.Annotation.DefaultDateTimeFormatOnly)]
        public DateTime? AvailableTime { get; set; }
        public AssessmentScheduleTypeEnum? AssessmentType { get; set; }
    }
}
