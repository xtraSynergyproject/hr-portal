using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class WorkflowViewModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateCode { get; set; }
        public string StageId { get; set; }
        public string StepId { get; set; }
        public string StageName { get; set; }
        public string StepName { get; set; }
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
        public string Type { get; set; }
        public string ParentId { get; set; }
        public string ComponentId { get; set; }
        public List<String> ParentIdList
        {
            get
            {
                return ParentIds?.Split(',')?.ToList();
            }
        }
        public string ParentIds { get; set; }
        public double SequenceOrder { get; set; }

    }
}
