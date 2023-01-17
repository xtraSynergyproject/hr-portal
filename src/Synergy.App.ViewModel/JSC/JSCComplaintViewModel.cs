using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCComplaintViewModel : ServiceTemplateViewModel
    {
        public string DepartmentId { get; set; }    
        public bool IsComplaintResolver { get; set; }    
        public bool IsFlag { get; set; }    
        public string Level1User { get; set; }    
        public string Level2User { get; set; }    
        public string Level3User { get; set; }    
        public string Level4User { get; set; }    
        public string Level1UserId { get; set; }    
        public bool IsEdit { get; set; }    
        public bool IsLevelUser { get; set; }    
        public bool IsFlagByLevel1 { get; set; }    
        public bool IsFlagByLevel2 { get; set; }    
        public bool IsFlagByLevel3 { get; set; }    
        public bool IsFlagByLevel4 { get; set; }    
        public string LevelUserRole { get; set; }    
        public string ComplaintId { get; set; }    
        public string GrvStatusId { get; set; }    
        public string Ward { get; set; }    
        public string Department { get; set; }
        public string GrievanceType { get; set; }
        public string GrievanceTypeId { get; set; }
        public string Name { get; set; }
        public string EventDate { get; set; }
        public string DocumentId { get; set; }
        public string Details { get; set; }
        public string Option { get; set; }
        public string Address { get; set; }
        public string DDN { get; set; }
        public string Map { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MapArea { get; set; }
        public string GrvStatus { get; set; }
        public string Level1Remarks { get; set; }
        public string PhotoFile { get; set; }
        public string ReopenId { get; set; }
        public string ReopenDateTime { get; set; }
        public int ReopenCount { get; set; }
        public string PhotoFileAfterReopen { get; set; }
        public string FlagId { get; set; }
        public string FlagDateTime { get; set; }
        public string LevelJobTitle { get; set; }
        public List<JSCComplaintViewModel> FlagDetails { get; set; }
        public List<JSCComplaintViewModel> ReopenDetails { get; set; }
        public bool IsComplaintOperator { get; set; }
        public string GrvStatusCode { get; set; }
        public string ReopenByName { get; set; }
        public string ReopenById { get; set; }
        public string ReopenByJobTitle { get; set; }
        public string ReopenByEmail { get; set; }
        public string FileName { get; set; }
        public string DDNLat { get; set; }
        public string DDNLong { get; set; }
        public string StatusList { get; set; }
        public string ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string WardIds { get; set; }
        public string ComplaintNoText { get; set; }
        public long ComplaintCount { get; set; }
        public long NoOfDaysPending { get; set; }
        public long NoOfDaysDisposed { get; set; }
        public long MaxDays { get; set; }
        public long MinDays { get; set; }
        public long AverageDays { get; set; }
        public string TrendDateText { get; set; }
        public long SequenceNo { get; set; }
        public string CreatedDateText { get; set; }
    }
}
