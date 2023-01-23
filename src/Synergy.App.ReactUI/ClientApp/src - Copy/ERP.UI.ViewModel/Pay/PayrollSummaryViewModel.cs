using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PayrollSummaryViewModel : ViewModelBase
    {
        public string PayrollNo { get; set; }
        public long? PendingEmpTransaction { get; set; }
        [Display(Name = "Payroll Department Name")]
        public string PayrollOrganization { get; set; }
        [Display(Name = "Payroll Department Name")]
        public long? PayrollOrganizationId { get; set; }
        public int? YearMonth { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        [Display(Name = "Included")]
        public int? IncludedPersonsCount { get; set; }
        [Display(Name = "Excluded")]
        public int? ExcludedPersonsCount { get; set; }
        [Display(Name = "Total")]
        public int? TotalPersonsCount { get; set; }
        [Display(Name = "CurrentMonth")]
        public int? CurrentMonthCount { get; set; }
        [Display(Name = "PreviousMonth")]
        public int? PreviousMonthCount { get; set; }
        public string[] PayRunList { get; set; }

        public string PayrollNo1 { get; set; }
        public int? CurrentMonthCount1 { get; set; }        
        public int? PreviousMonthCount1 { get; set; }

        public string PayrollNo2 { get; set; }
        public int? CurrentMonthCount2 { get; set; }
        public int? PreviousMonthCount2 { get; set; }

        public string PayrollNo3 { get; set; }
        public int? CurrentMonthCount3 { get; set; }
        public int? PreviousMonthCount3 { get; set; }

        public string PayrollNo4 { get; set; }
        public int? CurrentMonthCount4 { get; set; }
        public int? PreviousMonthCount4 { get; set; }

        public string PayrollNo5 { get; set; }
        public int? CurrentMonthCount5 { get; set; }
        public int? PreviousMonthCount5 { get; set; }

        public string PayrollNo6 { get; set; }
        public int? CurrentMonthCount6 { get; set; }
        public int? PreviousMonthCount6 { get; set; }

        public string PayrollNo7 { get; set; }
        public int? CurrentMonthCount7 { get; set; }
        public int? PreviousMonthCount7 { get; set; }

        public string PayrollNo8 { get; set; }
        public int? CurrentMonthCount8 { get; set; }
        public int? PreviousMonthCount8 { get; set; }

        public string PayrollNo9 { get; set; }
        public int? CurrentMonthCount9 { get; set; }
        public int? PreviousMonthCount9 { get; set; }

        public string PayrollNo10 { get; set; }
        public int? CurrentMonthCount10 { get; set; }
        public int? PreviousMonthCount10 { get; set; }

        public string PayrollNo11 { get; set; }
        public int? CurrentMonthCount11{ get; set; }
        public string PayrollNo12 { get; set; }
        public int? CurrentMonthCount12 { get; set; }
        public string PayrollNo13 { get; set; }
        public int? CurrentMonthCount13 { get; set; }
        public string PayrollNo14 { get; set; }
        public int? CurrentMonthCount14 { get; set; }
        public string PayrollNo15 { get; set; }
        public int? CurrentMonthCount15 { get; set; }
        public string PayrollNo16 { get; set; }
        public int? CurrentMonthCount16 { get; set; }
        public string PayrollNo17 { get; set; }
        public int? CurrentMonthCount17 { get; set; }
        public string PayrollNo18 { get; set; }
        public int? CurrentMonthCount18 { get; set; }
        public string PayrollNo19 { get; set; }
        public int? CurrentMonthCount19 { get; set; }
        public string PayrollNo20 { get; set; }
        public int? CurrentMonthCount20 { get; set; }
        public string PayrollNo21 { get; set; }
        public int? CurrentMonthCount21 { get; set; }
        public string PayrollNo22 { get; set; }
        public int? CurrentMonthCount22 { get; set; }
        public string PayrollNo23 { get; set; }
        public int? CurrentMonthCount23 { get; set; }
        public string PayrollNo24 { get; set; }
        public int? CurrentMonthCount24 { get; set; }
        public string PayrollNo25 { get; set; }
        public int? CurrentMonthCount25 { get; set; }
        public string PayrollNo26 { get; set; }
        public int? CurrentMonthCount26 { get; set; }
        public string PayrollNo27{ get; set; }
        public int? CurrentMonthCount27 { get; set; }
        public string PayrollNo28 { get; set; }
        public int? CurrentMonthCount28 { get; set; }
        public string PayrollNo29 { get; set; }
        public int? CurrentMonthCount29 { get; set; }
        public string PayrollNo30 { get; set; }
        public int? CurrentMonthCount30 { get; set; }


        public string Company { get; set; }

    }
}
