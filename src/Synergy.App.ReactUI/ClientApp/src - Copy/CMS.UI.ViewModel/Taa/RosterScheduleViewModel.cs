using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class RosterScheduleViewModel: NoteTemplateViewModel
    {
        public string LegalEntityCode { get; set; }
        
        public DateTime RosterDate { get; set; }
        public string WeekDateString { get; set; }
        public RosterDutyTypeEnum RosterDutyType { get; set; }
        public bool Duty1Enabled { get; set; }
   
        public TimeSpan? Duty1StartTime { get; set; }
  
        public TimeSpan? Duty1EndTime { get; set; }
        public bool Duty1FallsNextDay { get; set; }
        public bool Duty2Enabled { get; set; }
   
        public TimeSpan? Duty2StartTime { get; set; }

        public TimeSpan? Duty2EndTime { get; set; }
        public bool Duty2FallsNextDay { get; set; }
        public bool Duty3Enabled { get; set; }
        
        public TimeSpan? Duty3StartTime { get; set; }
      
        public TimeSpan? Duty3EndTime { get; set; }
        public bool Duty3FallsNextDay { get; set; }
        public TimeSpan? TotalHours { get; set; }

        public RosterDutyTypeEnum DraftRosterDutyType { get; set; }
        public bool DraftDuty1Enabled { get; set; }

        public TimeSpan? DraftDuty1StartTime { get; set; }

        public TimeSpan? DraftDuty1EndTime { get; set; }
        public bool DraftDuty1FallsNextDay { get; set; }
        public bool DraftDuty2Enabled { get; set; }

        public TimeSpan? DraftDuty2StartTime { get; set; }

        public TimeSpan? DraftDuty2EndTime { get; set; }
        public bool DraftDuty2FallsNextDay { get; set; }
        public bool DraftDuty3Enabled { get; set; }
      
        public TimeSpan? DraftDuty3StartTime { get; set; }
        public TimeSpan? DraftDuty3EndTime { get; set; }
        public bool DraftDuty3FallsNextDay { get; set; }
        public TimeSpan? DraftTotalHours { get; set; }

        public DocumentStatusEnum? PublishStatus { get; set; }
        public bool? IsDraft { get; set; }
  
        [Display(Name = "Last Published Date")]
        public DateTime? PublishDate { get; set; }

        [UIHint("PatternEditor")]
        public string Sunday { get; set; }
        [UIHint("PatternEditor")]
        public string Monday { get; set; }
        [UIHint("PatternEditor")]
        public string Tuesday { get; set; }
        [UIHint("PatternEditor")]
        public string Wednesday { get; set; }
        [UIHint("PatternEditor")]
        public string Thursday { get; set; }
        [UIHint("PatternEditor")]
        public string Friday { get; set; }
        [UIHint("PatternEditor")]
        public string Saturday { get; set; }

        public string SundayHours { get; set; }
        public string MondayHours { get; set; }
        public string TuesdayHours { get; set; }
        public string WednesdayHours { get; set; }
        public string ThursdayHours { get; set; }
        public string FridayHours { get; set; }
        public string SaturdayHours { get; set; }

        public string EmployeeNo { get; set; }
        public string PersonId { get; set; }
        public bool? IsAttendanceCalculated { get; set; }
        public string BiometricId { get; set; }
        public string EmployeeName { get; set; }
        public string UserNameWithEmail { get; set; }
        public string Email { get; set; }
        public string IqamahNo { get; set; }
        public string DisplayName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        [Display(Name = "Job Title")]
        public string JobName { get; set; }
        public string GradeName { get; set; }
        public string Nationality { get; set; }

        [Display(Name = "Department Name")]
        public string OrganizationId { get; set; }
        [UIHint("TypeEditor1")]
        public string SectionId { get; set; }
        [UIHint("TypeEditor1")]
        public string SectionName { get; set; }

        [UIHint("SubSectionEditor")]
        public string SubSectionId { get; set; }
        [UIHint("SubSectionEditor")]
        public string SubSectionName { get; set; }
        public string UserId { get; set; }
        public string AssignmentId { get; set; }

        public string UserIds { get; set; }
        public string RosterDates { get; set; }

        public IList<RosterScheduleLeaveViewModel> leaveList { get; set; }
        public string SundayText { get; set; }

        public List<string> CopyToWeeks { get; set; }

        public string MondayText { get; set; }
        public string TuesdayText { get; set; }
        public string WednesdayText { get; set; }
        public string ThursdayText { get; set; }
        public string FridayText { get; set; }
        public string SaturdayText { get; set; }

        public TimeSpan? SundayTotalHours { get; set; }
        public TimeSpan? MondayTotalHours { get; set; }
        public TimeSpan? TuesdayTotalHours { get; set; }
        public TimeSpan? WednesdayTotalHours { get; set; }
        public TimeSpan? ThursdayTotalHours { get; set; }
        public TimeSpan? FridayTotalHours { get; set; }
        public TimeSpan? SaturdayTotalHours { get; set; }

        [Display(Name = "Total Hours")]
        public string SumOfWeekHours { get; set; }
        [Display(Name = "Week")]
        public string WeekDisplayName { get; set; }

        public bool PublishWhileCopying { get; set; }

        [Display(Name = "Contract End Date")]      
        public DateTime? ContractEndDate { get; set; }
        [Display(Name = "Contract Renewed")]
        public string ContractRenewable { get; set; }
        [Display(Name = "Sponsor")]
        public string Sponsor { get; set; }

        public string DayOff1 { get; set; }
        public string DayOff2 { get; set; }
        public string DayOff3 { get; set; }
        public string DayOff4 { get; set; }
        public string DayOff5 { get; set; }
        public string DayOff6 { get; set; }
        public string DayOff7 { get; set; }

        public bool? Draft1 { get; set; }
        public bool? Draft2 { get; set; }
        public bool? Draft3 { get; set; }
        public bool? Draft4 { get; set; }
        public bool? Draft5 { get; set; }
        public bool? Draft6 { get; set; }
        public bool? Draft7 { get; set; }

        [UIHint("PatternEditor")]
        public int? Count1 { get; set; }
        [UIHint("PatternEditor")]
        public int? Count2 { get; set; }
        [UIHint("PatternEditor")]
        public int? Count3 { get; set; }
        [UIHint("PatternEditor")]
        public int? Count4 { get; set; }
        [UIHint("PatternEditor")]
        public int? Count5 { get; set; }
        [UIHint("PatternEditor")]
        public int? Count6 { get; set; }
        [UIHint("PatternEditor")]
        public int? Count7 { get; set; }

        public List<IdNameViewModel> userList { get; set; }

        public string SundayReporting
        {
            get
            {
                string template = "";
                template = SundayText;
                var hours = (DayOff1 == "DayOff") ? "OFF" : ("Total Hours : " + SundayHours);
                var act = Sunday ?? "";
                return SundayHours != null ? template + "\r\n" + hours + "\r\n" + act : act;
            }

        }

        public string MondayReporting
        {
            get
            {
                string template = "";
                template = MondayText;
                var hours = (DayOff2 == "DayOff") ? "OFF" : ("Total Hours : " + MondayHours);
                var act = Monday ?? "";
                return MondayHours != null ? template + "\r\n" + hours + "\r\n" + act : act;
            }

        }

        public string TuesdayReporting
        {
            get
            {
                string template = "";
                template = TuesdayText;
                var hours = (DayOff3 == "DayOff") ? "OFF" : ("Total Hours : " + TuesdayHours);
                var act = Tuesday ?? "";
                return TuesdayHours != null ? template + "\r\n" + hours + "\r\n" + act : act;
            }

        }

        public string WednesdayReporting
        {
            get
            {
                string template = "";
                template = WednesdayText;
                var hours = (DayOff4 == "DayOff") ? "OFF" : ("Total Hours : " + WednesdayHours);
                var act = Wednesday ?? "";
                return WednesdayHours != null ? template + "\r\n" + hours + "\r\n" + act : act;
            }

        }

        public string ThursdayReporting
        {
            get
            {
                string template = "";
                template = ThursdayText;
                var hours = (DayOff5 == "DayOff") ? "OFF" : ("Total Hours : " + ThursdayHours);
                var act = Thursday ?? "";
                return ThursdayHours != null ? template + "\r\n" + hours + "\r\n" + act : act;
            }

        }

        public string FridayReporting
        {
            get
            {
                string template = "";
                template = FridayText;
                var hours = (DayOff6 == "DayOff") ? "OFF" : ("Total Hours : " + FridayHours);
                var act = Friday ?? "";
                return FridayHours != null ? template + "\r\n" + hours + "\r\n" + act : act;
            }

        }

        public string SaturdayReporting
        {
            get
            {
                string template = "";
                template = SaturdayText;
                var hours = (DayOff7 == "DayOff") ? "OFF" : ("Total Hours : " + SaturdayHours);
                var act = Saturday ?? "";
                return SaturdayHours != null ? template + "\r\n" + hours + "\r\n" + act : act;
            }

        }

        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }

        public string ShiftPatternName { get; set; }

        public string RostarStatus1 { get; set; }
        public string RostarStatus2 { get; set; }
        public string RostarStatus3 { get; set; }
        public string RostarStatus4 { get; set; }
        public string RostarStatus5 { get; set; }
        public string RostarStatus6 { get; set; }
        public string RostarStatus7 { get; set; }

    }
}
