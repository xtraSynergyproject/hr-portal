using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class QuarterlyReportManpowerBudgetViewModel : BaseViewModel
    {

        public List<QuarterlyReportBudgetedPositionViewModel> Data { get; set; }

        public string OrganizationName { get; set; }

        public int BudgetYear { get; set; }

        public string Comments { get; set; }

        public float PositionDXBTotal { get; set; }

        public float PositionDWCTotal { get; set; }

        public float PositionTotalOrganizationTotal { get; set; }

        public float PositionTotalOrganizationPercentageVsDATotal { get; set; }

        public float PositionDXBPercentage { get; set; }

        public float PositionDWCPercentage { get; set; }

        public float PositionTotalOrganizationPercentage { get; set; }

        public float PositionTotalOrganizationPercentageVsDAPercentage { get; set; }

        public float CostDXBTotal { get; set; }

        public float CostDWCTotal { get; set; }

        public float CostTotalOrganizationTotal { get; set; }

        public float CostTotalOrganizationPercentageVsDATotal { get; set; }

        public float CostDXBPercentage { get; set; }

        public float CostDWCPercentage { get; set; }

        public float CostTotalOrganizationPercentage { get; set; }

        public float CostTotalOrganizationPercentageVsDAPercentage { get; set; }

        public QuarterlyReportManpowerBudgetViewModel()
        {
            Data = new List<QuarterlyReportBudgetedPositionViewModel>();
        }
    }


    public class QuarterlyReportBudgetedPositionViewModel : BaseViewModel
    {

        //Budgeted Position/Cost
        [Required]
        [StringLength(200)]
        public string BudgetedCategory { get; set; }

        [Required]
        [StringLength(200)]
        public string BudgetName { get; set; }

        public float DXB { get; set; }

        public float DWC { get; set; }

        public float TotalOrganization { get; set; }

        public float TotalOrganizationPercentageVsDA { get; set; }

    }



    public class YearTillDateActualViewModel : BaseViewModel
    {
        public List<YearTillDateActualPositionViewModel> Data { get; set; }

        public string OrganizationName { get; set; }

        public int BudgetYear { get; set; }

        public string ReportGenerationMonth { get; set; }

        public string Comments { get; set; }

        public float PositionDXBTotal { get; set; }

        public float PositionDWCTotal { get; set; }

        public float PositionTotalOrganizationTotal { get; set; }

        public string CostDXBVarianceBudgetVsActual { get; set; }

        public string CostDWCVarianceBudgetVsActual { get; set; }

        public string CostTotalOrganizationVarianceBudgetVsActual { get; set; }

        public YearTillDateActualViewModel()
        {
            Data = new List<YearTillDateActualPositionViewModel>();
        }
    }

    public class YearTillDateActualPositionViewModel : BaseViewModel
    {
        [Required]
        [StringLength(200)]
        public string BudgetedCategory { get; set; }

        [Required]
        [StringLength(200)]
        public string BudgetName { get; set; }

        public float DXB { get; set; }

        public float DWC { get; set; }

        public float TotalOrganization { get; set; }

    }


    public class GradeWiseStatisticsViewModel : BaseViewModel
    {

        public string OrganizationName { get; set; }
        public List<HeadedComment> CommentData { get; set; }
        public List<GradewiseDistribution> DistributionData { get; set; }

        public List<GradewiseAverageCost> GradewiseAverageCostData { get; set; }
        public List<HeadedComment> GradewiseAverageCostComment { get; set; }


        public List<GradewiseAverageCostTable> GradewiseAverageCostTableList { get; set; }

        public GradeWiseStatisticsViewModel()
        {
            CommentData = new List<HeadedComment>();
            DistributionData = new List<GradewiseDistribution>();
            GradewiseAverageCostData = new List<GradewiseAverageCost>();
            GradewiseAverageCostComment = new List<HeadedComment>();
            GradewiseAverageCostTableList = new List<GradewiseAverageCostTable>();
        }
    }



    public class HeadedComment : BaseViewModel
    {
        public string Heading { get; set; }

        public string Comment { get; set; }
    }

    public class GradewiseDistribution : BaseViewModel
    {
        public string Location { get; set; }

        public string GradeName { get; set; }

        public int HeadCount { get; set; }
    }


    public class GradewiseAverageCost : BaseViewModel
    {
        public string GradeName { get; set; }

        public int HeadCount { get; set; }

        public string CountCategory { get; set; }
    }

    public class GradewiseAverageCostTable : BaseViewModel
    {
        [Display(Name = "Average Annual Cost")]
        public string CostCategory { get; set; }

        public int OrganizationCount { get; set; }

        [Display(Name = "DA")]
        public int DACount { get; set; }

        public string Variance { get; set; }

        public string Status { get; set; }
    }


    public class MiscellanousManpowerSummary : BaseViewModel
    {
        public string OrganizationName { get; set; }
        public int BudgetYear { get; set; }
        public string QuarterName { get; set; }

        public List<MiscellanousManpowerSummaryDetails> Nationals { get; set; }
        public List<MiscellanousManpowerSummaryDetails> Females { get; set; }

        public int NationalDATotal { get; set; }
        public int NationalDWCTotal { get; set; }
        public int NationalRowTotal { get; set; }
        public int NationalTotalWithinLayer { get; set; }

        public int FemaleDATotal { get; set; }
        public int FemaleDWCTotal { get; set; }
        public int FemaleRowTotal { get; set; }
        public int FemaleTotalWithinLayer { get; set; }

        public List<HeadedComment> NationalComment { get; set; }
        public List<HeadedComment> FemaleComment { get; set; }
        public List<HeadedComment> AdditionalComment { get; set; }

        public MiscellanousManpowerSummary()
        {
            Nationals = new List<MiscellanousManpowerSummaryDetails>();
            Females = new List<MiscellanousManpowerSummaryDetails>();
            NationalComment = new List<HeadedComment>();
            FemaleComment = new List<HeadedComment>();
            AdditionalComment = new List<HeadedComment>();
        }
    }


    public class MiscellanousManpowerSummaryDetails : BaseViewModel
    {
        [Display(Name = "Average Annual Cost")]
        public string Category { get; set; }

        public int DA { get; set; }

        public int DWC { get; set; }

        public int RowTotal { get; set; }

        [Display(Name = "Total % within DA Layer")]
        public float TotalPercentage { get; set; }
    }


    public class QuarterlyManpowerTrendViewModel : BaseViewModel
    {
        public List<QuarterlyManpowerHeadcountTrendViewModel> HeacountTrend { get; set; }
        public List<QuarterlyManpowerCostTrendViewModel> CostTrend { get; set; }
        public List<HeadedComment> HeacountComment { get; set; }
        public List<HeadedComment> CostComment { get; set; }

        public QuarterlyManpowerTrendViewModel()
        {
            HeacountTrend = new List<QuarterlyManpowerHeadcountTrendViewModel>();
            CostTrend = new List<QuarterlyManpowerCostTrendViewModel>();
            HeacountComment = new List<HeadedComment>();
            CostComment = new List<HeadedComment>();
        }
    }


    public class QuarterlyManpowerHeadcountTrendViewModel : BaseViewModel
    {
        [Display(Name = "Quarters")]
        public string Quarter { get; set; }

        public string Location { get; set; }

        public int HeadCount { get; set; }
        //public int DA { get; set; }

        //public int DWC { get; set; }

        //public int Total { get; set; }
    }

    public class QuarterlyManpowerCostTrendViewModel : BaseViewModel
    {
        [Display(Name = "Quarters")]
        public string Quarter { get; set; }

        public string Location { get; set; }

        public float Cost { get; set; }
    }





    public class YearlyManpowerTrendViewModel : BaseViewModel
    {
        public List<YearlyManpowerTrendHeadcountViewModel> HeacountTrend { get; set; }
        public List<YearlyManpowerTrendCostViewModel> CostTrend { get; set; }
        public List<HeadedComment> YearlyComment { get; set; }

        public YearlyManpowerTrendViewModel()
        {
            HeacountTrend = new List<YearlyManpowerTrendHeadcountViewModel>();
            CostTrend = new List<YearlyManpowerTrendCostViewModel>();
            YearlyComment = new List<HeadedComment>();
        }
    }


    public class YearlyManpowerTrendHeadcountViewModel : BaseViewModel
    {
        [Display(Name = "Year")]
        public int Year { get; set; }

        public string Location { get; set; }

        public int HeadCount { get; set; }
    }

    public class YearlyManpowerTrendCostViewModel : BaseViewModel
    {
        [Display(Name = "Year")]
        public int Year { get; set; }

        public string Location { get; set; }

        public float Cost { get; set; }
    }

}
