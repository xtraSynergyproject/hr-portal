using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
   public class DashboardDocumentViewModel
    {

        public string Id { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentName { get; set; } 
        public string WorkflowStatus { get; set; }
        public string OwnerName { get; set; }
        public string DocumentType { get; set; }
        public string WorkflowId { get; set; }
        public string WorkflowName { get; set; }        
        public string WorkflowTemplateCode { get; set; }        
        public string FileId { get; set; }        
        public string FileName { get; set; }        
        public string FileExtension { get; set; }        
        public long ContentLength { get; set; }
        public string Size
        {
            get
            {
                var d = Humanizer.ByteSizeExtensions.Bytes(ContentLength);
                return d.ToString();
            }
        }
        public string CreatedDateDisplay
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(CreatedDate);
                return d;
            }
        }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string DueDateDisplay
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(DueDate);
                return d;
            }
        }
    }
}
