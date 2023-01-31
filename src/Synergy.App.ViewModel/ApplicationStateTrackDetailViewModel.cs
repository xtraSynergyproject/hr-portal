using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class ApplicationStateTrackDetailViewModel
    {
        public string ApplicationId { get; set; }
        public string ApplicationStateId { get; set; }
        public double UniqueNumber { get; set; }
        public string Number { get; set; }
        public string SubNumber { get; set; }
        public string Stage { get; set; }
        public string ActionName { get; set; }

        private string _ActionStatus;
        public string ActionStatus
        {
            get
            {
                if (_ActionStatus.IsNullOrEmpty())
                {
                    if (TaskNo.IsNotNullAndNotEmpty())
                    {
                        return $"{TaskNo} - {TaskStatusName.Coalesce(TaskStatusCode)}";
                    }
                    return "";
                }
                else
                {
                    return _ActionStatus;
                }

            }
            set
            {
                _ActionStatus = value;
            }
        }
        public string StateCode { get; set; }

        public string StateName { get; set; }
        public string StatusCode { get; set; }
        public string ApplicationStatusCode { get; set; }
        public string StatusName { get; set; }
        public DateTime? ChangedDate { get; set; }
        public string ChangedByName { get; set; }
        public string CandidateName { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? AppliedDate { get; set; }

        private string _ActionSLAPlanned;
        public string ActionSLAPlanned
        {
            get
            {
                if (_ActionSLAPlanned.IsNotNullAndNotEmpty())
                {
                    return _ActionSLAPlanned;
                }
                if (TaskSLA.IsNotNullAndNotEmpty())
                {
                    return TaskSLA;
                }
                return string.Empty;
            }
            set
            {
                _ActionSLAPlanned = value;
            }
        }
        private string _ActionSLAActual;
        public string ActionSLAActual
        {
            get
            {
                if (_ActionSLAActual.IsNotNullAndNotEmpty())
                {
                    return _ActionSLAActual;
                }

                if (TaskSubmittedDate != null)
                {
                    var endDate = TaskCompletionDate;
                    if (endDate == null)
                    {
                        endDate = TaskRejectedDate;
                    }
                    if (endDate != null)
                    {
                        return (endDate.Value - TaskSubmittedDate.Value).Days.ToString();
                    }
                }
                return string.Empty;
            }
            set
            {
                _ActionSLAActual = value;
            }
        }
        public double? DaysElapsed { get; set; }

        public string TaskId { get; set; }
        public string TaskNo { get; set; }
        public string TaskSubject { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public string TaskSLA { get; set; }
        public DateTime? TaskSubmittedDate { get; set; }
        public DateTime? TaskCompletionDate { get; set; }
        public DateTime? TaskRejectedDate { get; set; }
        public string TaskAssignedToUserId { get; set; }
        public string TaskAssignedToEmail { get; set; }
        public string TaskStatusCode { get; set; }
        public string TaskStatusName { get; set; }
        public string TaskTemplateSubject { get; set; }

        public string AssigneeName { get; set; }
        public string TextValue1 { get; set; }
        public string TextValue2 { get; set; }
        public string TextValue3 { get; set; }
        public string TextValue4 { get; set; }
        public string TextValue5 { get; set; }
        public string TextValue6 { get; set; }
        public string TextValue7 { get; set; }
        public string TextValue8 { get; set; }
        public string TextValue9 { get; set; }
        public string TextValue10 { get; set; }
        public string JobName { get; set; }
        public string OrgName { get; set; }
        public string ApplicationStateName { get; set; }
        public string ApplicationStatusName { get; set; }
        public string BatchName { get; set; }
        public string ShortlistByHMComment { get; set; }
        public string TemplateCode { get; set; }
    }

}
