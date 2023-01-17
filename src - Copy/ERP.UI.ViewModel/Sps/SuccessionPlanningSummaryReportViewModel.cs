using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel.Sps
{
    public class SuccessionPlanningSummaryReportViewModel : ViewModelBase
    {
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public int Cycle { get; set; }
        public int Step1Inprogress { get; set; }
        public int Step1Overdue { get; set; }
        public int Step1Complete { get; set; }
        public int Step2Inprogress { get; set; }
        public int Step2Overdue { get; set; }
        public int Step2Complete { get; set; }
        public int Step3Inprogress { get; set; }
        public int Step3Overdue { get; set; }
        public int Step3Complete { get; set; }
      


    }
}
