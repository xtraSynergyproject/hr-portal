
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class NtsEmailViewModel : TreeViewViewModel
    {

        public string CategoryCode { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TargetId { get; set; }
        public NtsEmailTargetTypeEnum TargetType { get; set; }
        public string ServiceId { get; set; }
        public string ServicePlusId { get; set; }
        public string ServiceNo { get; set; }
        public string NtsId { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        public string NtsNo { get; set; }
        public bool CanReply { get; set; }
        public bool CanOpen
        {
            get
            {
                return true;
                //if (TargetType == NtsEmailTargetTypeEnum.Service || TargetType == NtsEmailTargetTypeEnum.StepTask)
                //{
                //    return true;
                //}
                //return false;
            }
        }

        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserEmail { get; set; }
        public string FromUserPhotoId { get; set; }
        public string FromUserJobTitle { get; set; }
        public string FromUserDepartmentName { get; set; }

        public string MessageUserName
        {
            get
            {
                if (EmailType == EmailTypeEnum.Inbox)
                {
                    return FromUserName;
                }
                return ToUserName;
            }
        }
        public string MessageUserPhotoId
        {
            get
            {
                if (EmailType == EmailTypeEnum.Inbox)
                {
                    return FromUserPhotoId;
                }
                return ToUserPhotoId;
            }
        }
        public string MessageUserJobTitle
        {
            get
            {
                if (EmailType == EmailTypeEnum.Inbox)
                {
                    return FromUserJobTitle;
                }
                return ToUserJobTitle;
            }
        }
        public string MessageUserDepartmentName
        {
            get
            {
                if (EmailType == EmailTypeEnum.Inbox)
                {
                    return FromUserDepartmentName;
                }
                return ToUserDepartmentName;
            }
        }
        public string DisplayCreatedDate
        {
            get
            {

                return CreatedDate.Humanize();
            }
        }
        public string ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string ToUserEmail { get; set; }
        public string ToUserPhotoId { get; set; }
        public string ToUserJobTitle { get; set; }
        public string ToUserDepartmentName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateStr { get { return CreatedDate.ToDefaultDateTimeFormat(); } }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int PendingDays
        {
            get
            {
                if (StartDate.HasValue)
                {
                    var cd = CompletedDate ?? DateTime.Today;
                    return cd.Subtract(StartDate.Value).Days;
                }
                return 0;
            }
        }
        public EmailTypeEnum EmailType { get; set; }
        public string ActualStatus { get; set; }
        public EmailInboxTypeEnum InboxStatus
        {
            get
            {
                if (ActualStatus.IsNullOrEmpty())
                {
                    return EmailInboxTypeEnum.Completed;
                }
                else if (ActualStatus.ToLower().Contains("draft"))
                {
                    return EmailInboxTypeEnum.Drafted;
                }
                else if (ActualStatus.ToLower().Contains("inprogress") || ActualStatus.ToLower().Contains("overdue")
                   || ActualStatus.ToLower().Contains("in_progress") || ActualStatus.ToLower().Contains("over_due"))
                {
                    return EmailInboxTypeEnum.Pending;
                }
                else
                {
                    return EmailInboxTypeEnum.Completed;
                }
            }
        }
        public string InboxStatusStr
        {
            get
            {
                return InboxStatus.ToString();
            }
        }
        public string OutboxStatus { get; set; }
        public NtsEmailViewModel GroupKey { get; set; }
        public List<string> Tos { get; set; }
        public List<string> CCs { get; set; }
        public string FromUserDepartmentId { get; set; }
        public string ToUserDepartmentId { get; set; }
        public string MessageUserDepartmentId
        {
            get
            {
                if (EmailType == EmailTypeEnum.Inbox)
                {
                    return FromUserDepartmentId;
                }
                return ToUserDepartmentId;
            }
        }
        public string Action { get; set; }
        public string WorkflowStatus { get; set; }
        public int PendingCount { get; set; }
        public int CompletedCount { get; set; }
        public string StatusColor { get; set; }
    }
}
