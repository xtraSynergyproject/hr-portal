using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class QuarterlyReportTrendViewModel : QuarterlyReportBaseViewModel
    {
        public QuarterlyReportTrendViewModel()
        {
            QuarterlyTrend = new QuarterlyTrendViewModel();
            YearlyTrend = new YearlyTrendViewModel();
            CostStatus = new QuarterlyReportCostStatusViewModel();
        }
        public QuarterlyReportTrendViewModel(QuarterlyReportBaseViewModel baseModel)
        {
            this.MapValue(baseModel);
            QuarterlyTrend = new QuarterlyTrendViewModel(baseModel);
            YearlyTrend = new YearlyTrendViewModel(baseModel);
            CostStatus = new QuarterlyReportCostStatusViewModel(baseModel);
        }

        public QuarterlyTrendViewModel QuarterlyTrend { get; set; }
        public YearlyTrendViewModel YearlyTrend { get; set; }
        public QuarterlyReportCostStatusViewModel CostStatus { get; set; }
    }
    public class QuarterlyReportCostStatusViewModel : QuarterlyReportBaseViewModel
    {
        public QuarterlyReportCostStatusViewModel(QuarterlyReportBaseViewModel baseModel)
        {
            this.MapValue(baseModel);
        }
        public QuarterlyReportCostStatusViewModel()
        {

        }
        public List<QuarterlyReportTrendCostStatusViewModel> CostList { get; set; }
    }
    public class QuarterlyReportTrendCostStatusViewModel
    {

        public int OrganizationId { get; set; }
        [Display(Name = "Organization")]
        public string Organization { get; set; }

        [Display(Name = "DXB")]
        public int BudgetedCost_DXB { get; set; }
        [Display(Name = "DWC")]
        public int BudgetedCost_DWC { get; set; }
        [Display(Name = "Total")]
        public int BudgetedCost_Total { get; set; }


        [Display(Name = "DXB")]
        public int ActualCost_DXB { get; set; }
        [Display(Name = "DWC")]
        public int ActualCost_DWC { get; set; }
        [Display(Name = "Total")]
        public int ActualCost_Total { get; set; }

        [Display(Name = "YTD Cost Variance")]
        public int BudgetedCostVariance { get; set; }





        [Display(Name = "Headcount")]
        public int MiscShiftHC { get; set; }
        [Display(Name = "Cost")]
        public int MiscShiftCost { get; set; }
        [Display(Name = "% in DA")]
        public int MiscShiftPerc { get; set; }


        [Display(Name = "Headcount")]
        public int MiscOverTimeHC { get; set; }
        [Display(Name = "Cost")]
        public int MiscOverTimeCost { get; set; }
        [Display(Name = "% in DA")]
        public int MiscOverTimePerc { get; set; }

        [Display(Name = "Headcount")]
        public int MiscPPAHC { get; set; }
        [Display(Name = "Cost")]
        public int MiscPPACost { get; set; }
        [Display(Name = "% in DA")]
        public int MiscPPAPerc { get; set; }


        [Display(Name = "Junior")]
        public int ManagementLayerJunior_HC { get; set; }
        [Display(Name = "Middle")]
        public int ManagementLayerMiddle_HC { get; set; }
        [Display(Name = "Senior")]
        public int ManagementLayerSenior_HC { get; set; }


        [Display(Name = "Middle: Junior")]
        public int SoCMidddleToJunior { get; set; }
        [Display(Name = "Senior: Middle")]
        public int SoCSeniorToMiddle { get; set; }


        [Display(Name = "Junior")]
        public int CostPerHeadPerYear_Junior { get; set; }
        [Display(Name = "Middle")]
        public int CostPerHeadPerYear_Middle { get; set; }
        [Display(Name = "Senior")]
        public int CostPerHeadPerYear_Senior { get; set; }
  
  
    }
    public class DemographicComparisonViewModel : QuarterlyReportBaseViewModel
    {
        public DemographicComparisonViewModel()
        {

        }
        public DemographicComparisonViewModel(QuarterlyReportBaseViewModel baseModel)
        {
            this.MapValue(baseModel);
        }
        public List<DemographicDetailViewModel> DemographicDetailList { get; set; }
    }
    public class QuarterlyTrendViewModel : QuarterlyReportBaseViewModel
    {
        public QuarterlyTrendViewModel()
        {

        }
        public QuarterlyTrendViewModel(QuarterlyReportBaseViewModel baseModel)
        {
            this.MapValue(baseModel);
        }
        public List<QuarterlyHeadCountTrendViewModel> HeadCountTrendList { get; set; }
        public string HeadCountTrendTitle { get; set; }
        public string HeadCountTrendComment { get; set; }

        public List<QuarterlyCostTrendViewModel> CostTrendList { get; set; }
        public string CostTrendTitle { get; set; }
        public string CostTrendComment { get; set; }
        public string SalaryNote { get; set; }

    }
    public class DemographicDetailViewModel
    {
        public DateTime QuarterDate { get; set; }
        public string QuarterName
        {
            get
            {
                var month = QuarterDate.Month;
                if (month == 3)
                {
                    return String.Concat("Q1-", QuarterDate.Year);
                }
                else if (month == 6)
                {
                    return String.Concat("Q2-", QuarterDate.Year);
                }
                else if (month == 9)
                {
                    return String.Concat("Q3-", QuarterDate.Year);
                }

                {
                    return String.Concat("Q4-", QuarterDate.Year);
                }


            }
        }
        [Display(Name = Constant.DXBName)]
        public int Unit_DXB { get; set; }
        [Display(Name = Constant.DWCName)]
        public int Unit_DWC { get; set; }
        [Display(Name = "Total")]
        public int Unit_Total { get; set; }
        public int DA_DXB { get; set; }
        public int DA_DWC { get; set; }
        public int DA_Total { get; set; }
    }
    public class QuarterlyHeadCountTrendViewModel
    {
        public DateTime QuarterDate { get; set; }
        public string QuarterName
        {
            get
            {
                var month = QuarterDate.Month;
                if (month == 3)
                {
                    return String.Concat("Q1-", QuarterDate.Year);
                }
                else if (month == 6)
                {
                    return String.Concat("Q2-", QuarterDate.Year);
                }
                else if (month == 9)
                {
                    return String.Concat("Q3-", QuarterDate.Year);
                }

                {
                    return String.Concat("Q4-", QuarterDate.Year);
                }


            }
        }
        [Display(Name = Constant.DXBName)]
        public int Unit_DXB { get; set; }
        [Display(Name = Constant.DWCName)]
        public int Unit_DWC { get; set; }
        [Display(Name = "Total")]
        public int Unit_Total { get; set; }
        public int DA_DXB { get; set; }
        public int DA_DWC { get; set; }
        public int DA_Total { get; set; }
    }
    public class QuarterlyCostTrendViewModel
    {
        public DateTime QuarterDate { get; set; }
        [Display(Name = Constant.DXBName)]
        public decimal Unit_DXB { get; set; }
        [Display(Name = Constant.DWCName)]
        public decimal Unit_DWC { get; set; }
        [Display(Name = "Total")]
        public decimal Unit_Total { get; set; }
        public decimal DA_DXB { get; set; }
        public decimal DA_DWC { get; set; }
        public decimal DA_Total { get; set; }
        public string QuarterName
        {
            get
            {
                var month = QuarterDate.Month;
                if (month == 3)
                {
                    return String.Concat("Q1-", QuarterDate.Year);
                }
                else if (month == 6)
                {
                    return String.Concat("Q2-", QuarterDate.Year);
                }
                else if (month == 9)
                {
                    return String.Concat("Q3-", QuarterDate.Year);
                }

                {
                    return String.Concat("Q4-", QuarterDate.Year);
                }


            }
        }
    }
    public class YearlyTrendViewModel : QuarterlyReportBaseViewModel
    {
        public YearlyTrendViewModel()
        {

        }
        public YearlyTrendViewModel(QuarterlyReportBaseViewModel baseModel)
        {
            this.MapValue(baseModel);
        }
        public List<YearlyHeadCountTrendViewModel> HeadCountTrendList { get; set; }
        public string HeadCountTrendTitle { get; set; }

        public List<YearlyCostTrendViewModel> CostTrendList { get; set; }
        public string CostTrendTitle { get; set; }
        public string YearlyTrendComment { get; set; }
        public string SalaryNote { get; set; }
    }
    public class YearlyHeadCountTrendViewModel
    {
        public int Year { get; set; }
        [Display(Name = Constant.DXBName)]
        public int Unit_DXB { get; set; }
        [Display(Name = Constant.DWCName)]
        public int Unit_DWC { get; set; }
        [Display(Name = "Total")]
        public int Unit_Total { get; set; }
        public int DA_DXB { get; set; }
        public int DA_DWC { get; set; }
        public int DA_Total { get; set; }
    }
    public class YearlyCostTrendViewModel
    {
        public int Year { get; set; }
        [Display(Name = Constant.DXBName)]
        public decimal Unit_DXB { get; set; }
        [Display(Name = Constant.DWCName)]
        public decimal Unit_DWC { get; set; }
        [Display(Name = "Total")]
        public decimal Unit_Total { get; set; }
        public decimal DA_DXB { get; set; }
        public decimal DA_DWC { get; set; }
        public decimal DA_Total { get; set; }
    }
}
