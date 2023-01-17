using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class UserActivityViewModel : ViewModelBase
    {
        public long? UserId { get; set; }
        public string EventBy { get; set; }
        public DataOperationEvent? EventType { get; set; }
        public string EventDescription { get; set; }
        //public NodeEnum? ReferenceNode { get; set; }
        public long? ReferenceId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        public string Hyperlink { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public string level { get; set; }
        public string color { get; set; }
        public List<DatesViewModel> DatesList { get; set; }
    
        public string ActivityType
        {
            get { return EventType.Description(); }
        }       
    }
   
}
