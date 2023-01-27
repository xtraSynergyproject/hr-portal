using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class ProjectGanttTaskViewModel : Kendo.Mvc.UI.IGanttTask
    {
        public long Id { get; set; }
        public string ProjectName { get; set; }
        public string UserName { get; set; }
        public string OwnerName { get; set; }
        public bool IsManager { get; set; }
        public string TaskNo { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime DueDate { get; set; }
        public long? ParentId { get; set; }
        public long? UserId { get; set; }
       
        public string Title { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime Start { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime End { get; set; }
        public decimal PercentComplete { get; set; }
        public bool Summary { get; set; }
        public bool Expanded { get; set; }
        public int OrderId { get; set; }

        public NtsActionEnum TemplateAction { get; set; }
        public NtsUserTypeEnum? TemplateUserType { get; set; }
        public string TaskType { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public long? PredecessorId { get; set; }
        public long? TaskGroupId { get; set; }

        public string StatusStyle
        {
            get
            {
                switch (TemplateAction)
                {
                    case NtsActionEnum.Draft:
                        return "Color:#5bc0de";
                    case NtsActionEnum.Submit:
                        return "Color:#f0ad4e";
                    case NtsActionEnum.Complete:
                        return "Color:#5cb85c";
                    case NtsActionEnum.Cancel:
                        return "Color:#999";
                    case NtsActionEnum.Overdue:
                        return "Color:#d9534f";
                    default:
                        return "Color:#999";
                }
            }
        }

        public string TaskStyle
        {
            get
            {
                switch (TaskType)
                {
                    case "Main":
                        return "Color:#5bc0de";
                    case "SubTask":
                        return "Color:#f0ad4e";
                    case "Email":
                        return "Color:#5cb85c";
                    default:
                        return "Color:#999";
                }
            }
        }
    }
}
