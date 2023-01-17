using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class PayrollBatchViewModel : NoteTemplateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LegalEntityId { get; set; }
        public string PayrollGroupId { get; set; }
        public PayrollRunTypeEnum RunType { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }
        public int? Year { get; set; }
        public string Month { get; set; }
        public int? YearMonth { get; set; }
        public DateTime? AttendanceStartDate { get; set; }
        public DateTime? AttendanceEndDate { get; set; }
        public PayrollStatusEnum PayrollStatus { get; set; }
        public PayrollExecutionStatusEnum ExecutionStatus { get; set; }  
        public string PayrollGroupName { get; set; }
        public string OrganizationName { get; set; }
        public PayrollStateEnum PayrollStateStart { get; set; }
        [Display(Name = "Payroll State")]
        public PayrollStateEnum PayrollStateEnd { get; set; }
        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }
        public DateTime PayrollRunDate { get; set; }
        [Display(Name = "No. Of Employees Processed")]
        public int TotalProcessed { get; set; }
        [Display(Name = "No. Of Employees Succeeded")]
        public int TotalSucceeded { get; set; }

    }
}
