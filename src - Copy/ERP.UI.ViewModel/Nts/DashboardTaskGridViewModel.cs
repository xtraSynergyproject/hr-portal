using ERP.Utility;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DashboardTaskGridViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long HistoryCount { get; set; }
        public List<DashboardTaskGridItemViewModel> HistoryList { get; set; }
        public long ToDoCount { get; set; }
        public List<DashboardTaskGridItemViewModel> ToDoList { get; set; }
        public long TodayCount { get; set; }
        public List<DashboardTaskGridItemViewModel> TodayList { get; set; }
        public long TomorrowCount { get; set; }
        public List<DashboardTaskGridItemViewModel> TomorrowList { get; set; }
        public long FutureCount { get; set; }
        public List<DashboardTaskGridItemViewModel> FutureList { get; set; }
        public long RemindMe { get; set; }
        public List<DashboardTaskGridItemViewModel> Allresult { get; set; }


        public List<DashboardTaskGridItemViewModel> PendingList { get; set; }
        public long PendingCount { get; set; }
        public List<DashboardTaskGridItemViewModel> OverdueList { get; set; }
        public long OverdueCount { get; set; }
        public List<DashboardTaskGridItemViewModel> CompletedList { get; set; }
        public long CompletedCount { get; set; }
        public List<DashboardTaskGridItemViewModel> ArchivedList { get; set; }
        public long ArchiedCount { get; set; }
        public List<DashboardTaskGridItemViewModel> CenceledList { get; set; }
        public long CanceledCount { get; set; }
    }

    public class DashboardTaskGridItemViewModel : ISchedulerEvent
    {
        public long ServiceId { get; set; }
        public string ServiceNo { get; set; }
        public string ServiceName { get; set; }
        public long TaskId { get; set; }
        public string TaskNo { get; set; }
        public long AssigneeId { get; set; }
        public string AssigneeName { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public NtsActionEnum TemplateAction { get; set; }
        public long HolderUserId { get; set; }
        public string HolderUserName { get; set; }
        public long? HolderUserPhotoId { get; set; }
        public long? HolderUserPhotoVersionId { get; set; }
        public string Subject { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]       
        [Display(Name = "DueDate", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public DateTime? DueDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public DateTime? StartDate { get; set; }
        public DateTime? PlanDate { get; set; }
        public NtsPriorityEnum? Priority { get; set; }
        public ModuleEnum? Module { get; set; }
        public long TemplateId { get; set; }
        public string PlannedBy { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string SubjectDisplay
        {
            get
            {
                return Subject.LimitTo(30);

            }
        }
        public string HolderUserNameDisplay
        {
            get
            {
                return HolderUserName.LimitTo(30);

            }
        }
        public string DueDateDisplay
        {
            get
            {
                if (TemplateAction == NtsActionEnum.Complete)
                {
                    var d = Humanizer.DateHumanizeExtensions.Humanize(CompletionDate);
                    return d;
                }
                else if (TemplateAction == NtsActionEnum.Cancel)
                {
                    var d = Humanizer.DateHumanizeExtensions.Humanize(CanceledDate);
                    return d;
                }
                else
                {
                    var d = Humanizer.DateHumanizeExtensions.Humanize(DueDate);
                    return d;
                }

            }
        }
        public string DueDateText
        {
            get
            {
                if (TemplateAction == NtsActionEnum.Complete)
                {
                    return CompletionDate.ToDefaultDateTimeFormat();
                }
                else if (TemplateAction == NtsActionEnum.Cancel)
                {
                    return CanceledDate.ToDefaultDateTimeFormat();
                }
                else
                {
                    return DueDate.ToDefaultDateTimeFormat();
                }

            }
        }
        public string StatusClass
        {
            get
            {
                switch (TemplateAction)
                {
                    case NtsActionEnum.Draft:
                        return "label-info";
                    case NtsActionEnum.Submit:
                        return "label-warning";
                    case NtsActionEnum.Complete:
                        return "label-success";
                    case NtsActionEnum.Cancel:
                        return "label-default";
                    case NtsActionEnum.Overdue:
                        return "label-danger";
                    default:
                        return "label-default";
                }
            }
        }
        public string PriorityIconText
        {
            get
            {
                return Convert.ToString(Priority).FirstLetterUpperCase();
            }
        }
        public string PriorityText
        {
            get
            {
                return Convert.ToString(Priority);
            }
        }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DayTimeFormat)]
        [Display(Name = "SLA", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public TimeSpan? SLA { get; set; }

        [Display(Name = "SLA", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string SLAText
        {
            get
            {
                if (SLA != null)
                {
                    //TimeSpan t = TimeSpan.Parse(SLA.ToString());
                    //return string.Format("{0}.{1}:{2}", t.Days,t.Hours, t.Minutes);
                    return SLA.Value.ToString(@"d\.hh\:mm");
                }
                return "";
            }

        }

        [Display(Name = "OwnerName", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string OwnerName { get; set; }
        public string ServiceOwner { get; set; }



        public long Id { get; set; }

        public long ParentID { get; set; }

        public string Title { get; set; }
        public string TitleLimited { get { return Title.LimitTo(120); } }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public string NtsId { get; set; }
       
        public long UserId { get; set; }

        public int? RecurrId { get; set; }

    }
}
