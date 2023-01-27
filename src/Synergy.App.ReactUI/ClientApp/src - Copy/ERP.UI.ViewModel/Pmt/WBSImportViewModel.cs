using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{  
    public class WBSImportViewModel : ViewModelBase
    {
        public string SequenceNo { get; set; }
        public string QpRefernce { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDescription { get; set; }
        public string WorkHours { get; set; }
        public string Priorty { get; set; }
        public string PlanStartDate { get; set; }
        public string PlanEndDate { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualEndDate { get; set; }
        public string ForcastStartDate { get; set; }
        public string ForcastEndDate { get; set; }
        public string ErrorMessage { get; set; }
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public long FolderId { get; set; }
        public string FolderLoaction { get; set; }

    }
}
