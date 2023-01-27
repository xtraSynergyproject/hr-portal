using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class QuarterlyReportBudgetViewModel : QuarterlyReportBaseViewModel
    {
        public QuarterlyReportBudgetViewModel()
        {
            ManpowerBudget = new ManpowerBudgetViewModel();
            GradewiseStatistics = new QuarterlyReportGradewiseStatisticsViewModel();
            ManpowerActual = new QuarterlyReportManpowerActualViewModel();
            Miscellaneous = new QuarterlyReportMiscellaneousViewModel();
            HeadCountComparison = new QuarterlyReportHeadCountCompariosnViewModel();
            Miscellaneous = new QuarterlyReportMiscellaneousViewModel();
        }
        public QuarterlyReportBudgetViewModel(QuarterlyReportBaseViewModel baseModel)
        {
            this.MapValue(baseModel);
            ManpowerBudget = new ManpowerBudgetViewModel();
            GradewiseStatistics = new QuarterlyReportGradewiseStatisticsViewModel(baseModel);
            ManpowerActual = new QuarterlyReportManpowerActualViewModel();
            HeadCountComparison = new QuarterlyReportHeadCountCompariosnViewModel(baseModel);
            Miscellaneous = new QuarterlyReportMiscellaneousViewModel();
        }
        public ManpowerBudgetViewModel ManpowerBudget { get; set; }
        public QuarterlyReportManpowerActualViewModel ManpowerActual { get; set; }
        public QuarterlyReportGradewiseStatisticsViewModel GradewiseStatistics { get; set; }
        public QuarterlyReportMiscellaneousViewModel Miscellaneous { get; set; }

        public QuarterlyReportHeadCountCompariosnViewModel HeadCountComparison { get; set; }


        public string Html { get; set; }
        public int PageWidth { get; set; }
        public int PageHeight { get; set; }

    }
    public class QuarterlyReportHeadCountCompariosnViewModel : QuarterlyReportBaseViewModel
    {
        public QuarterlyReportHeadCountCompariosnViewModel()
        {
        }
        public QuarterlyReportHeadCountCompariosnViewModel(QuarterlyReportBaseViewModel baseModel)
        {
            this.MapValue(baseModel);
        }
        public List<QuarterlyReportBudgetHeadCountComparisonViewModel> HeadCountComparisonList { get; set; }
    }
    public class QuarterlyReportBudgetHeadCountComparisonViewModel
    {

        public int OrganizationId { get; set; }
        [Display(Name = "Organization")]
        public string Organization { get; set; }

        [Display(Name = "DXB")]
        public int BudgetedHC_DXB { get; set; }
        [Display(Name = "DWC")]
        public int BudgetedHC_DWC { get; set; }
        [Display(Name = "Total")]
        public int BudgetedHC_Total { get; set; }

        [Display(Name = "DXB")]
        public int ActualHC_DXB { get; set; }
        [Display(Name = "DWC")]
        public int ActualHC_DWC { get; set; }
        [Display(Name = "Total")]
        public int ActualHC_Total { get; set; }

        [Display(Name = "DXB")]
        public int RRHC_DXB { get; set; }
        [Display(Name = "DWC")]
        public int RRHC_DWC { get; set; }
        [Display(Name = "Total")]
        public int RRHC_Total { get; set; }

        [Display(Name = "Existing Vs Budgeted")]
        public int ExistingVsBudgeted { get; set; }

        [Display(Name = "DXB")]
        public int NewHC_DXB { get; set; }
        [Display(Name = "DWC")]
        public int NewHC_DWC { get; set; }
        [Display(Name = "Total")]
        public int NewHC_Total { get; set; }

        [Display(Name = "Headcount")]
        public int NationalCount { get; set; }
        [Display(Name = "% Org")]
        public decimal? NationalPerc { get; set; }

        [Display(Name = "Headcount")]
        public int FemaleCount { get; set; }
        [Display(Name = "% Org")]
        public decimal? FemalePerc { get; set; }
    }
    public class ManpowerBudgetViewModel : QuarterlyReportBaseViewModel
    {
        public string PositionName { get; set; }
        public string CostName { get; set; }

        public List<QuarterlyReportManpowerBudgetPositionViewModel> ManpowerBudgetPositionList { get; set; }
        public string ManpowerBudgetPositionTitle { get; set; }
        public List<QuarterlyReportManpowerBudgetCostViewModel> ManpowerBudgetCostList { get; set; }

        public string ManpowerBudgetCostTitle { get; set; }
        public string ManpowerBudgetCostNote { get; set; }

    }
    public class QuarterlyReportManpowerActualViewModel : QuarterlyReportBaseViewModel
    {
        public string PositionName { get; set; }
        public string CostName { get; set; }
        public List<QuarterlyReportManpowerActualPositionViewModel> ActualPositionList { get; set; }
        public string ActualPositionTitle { get; set; }
        public List<QuarterlyReportManpowerActualCostViewModel> ActualCostList { get; set; }
        public string ActualCostTitle { get; set; }
        public string ActualeComment { get; set; }
        public string ActualCostNote { get; set; }
        public string ConditionNote { get; set; }
    }

    public class QuarterlyReportGradewiseStatisticsViewModel : QuarterlyReportBaseViewModel
    {
        public QuarterlyReportGradewiseStatisticsViewModel()
        {
        }
        public QuarterlyReportGradewiseStatisticsViewModel(QuarterlyReportBaseViewModel baseModel)
        {
            this.MapValue(baseModel);
        }
        public string HeadCountTitle { get; set; }
        public string HeadCountComment { get; set; }
        public string ManagementLayerHeadCountTitle { get; set; }
        public string HeadCountUnitPercentageName { get; set; }

        public string CostTitle { get; set; }
        public string CostComment { get; set; }
        public string ManagementLayerCostTitle { get; set; }

        public int HeadCountAvg
        {
            get
            {
                return GradewiseHeadCountList == null || GradewiseHeadCountList.Count == 0 ? 1 :
                    (GradewiseHeadCountList.Max(x => x.Unit_DXB) + GradewiseHeadCountList.Max(x => x.Unit_DWC)) / 4;
            }
        }
        public List<QuarterlyReportGradeWiseHeadCountDistributionViewModel> GradewiseHeadCountList { get; set; }
        public List<QuarterlyReportManagementLayerHeadCountViewModel> ManagementLayerwiseHeadCountList { get; set; }

        public List<QuarterlyReportGradeWiseAverageCostViewModel> GradewiseAverageCostList { get; set; }
        public List<QuarterlyReportManagementLayerAverageCostViewModel> ManagementLayerwiseAverageCostList { get; set; }
    }
    public class QuarterlyReportMiscellaneousViewModel : QuarterlyReportBaseViewModel
    {
        public List<QuarterlyReportManagementLayerNationalSummaryViewModel> ManagementLayerwiseNationalSummaryList { get; set; }
        public string NationalSummaryTitle { get; set; }
        public string NationalSummaryComment { get; set; }

        public List<QuarterlyReportManagementLayerFemaleSummaryViewModel> ManagementLayerwiseFemaleSummaryList { get; set; }
        public string FemaleSummaryTitle { get; set; }
        public string FemaleSummaryComment { get; set; }
        public string ShiftAllowanceTitle { get; set; }
        public string ShiftAllowanceComment { get; set; }
    }

    public class QuarterlyReportManpowerBudgetPositionViewModel : QuarterlyReportBaseViewModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string OrgShortName { get; set; }

        public int BudgetPositionStatusId { get; set; }

        public decimal Unit_DXB { get; set; }
        public decimal Unit_DWC { get; set; }
        public decimal Unit_Total { get; set; }
        public decimal? Unit_Per { get; set; }
        public string NameDisplay
        {
            get
            {
                if (BudgetPositionStatusId == -1)
                {
                    if (ReportMode == QuarterlyReportMode.Organization)
                    {
                        return String.Concat("%  Positions");
                    }
                    else
                    {
                        return String.Concat("Total ", BusinessUnit, " HC % vs.", Constant.DAName);
                    }

                }
                if (BudgetPositionStatusId == 0)
                {
                    return "Total";
                }
                else
                {
                    return Name;
                }
            }
        }
        public string Unit_DXB_Display
        {
            get
            {
                if (BudgetPositionStatusId == -1)
                {
                    return String.Concat(Unit_DXB == 0 ? "0" : String.Format("{0:0.0}", Unit_DXB).Replace(".0", ""), "%");
                }
                else
                {
                    return Convert.ToInt32(Unit_DXB).ToSafeString();
                }
            }
        }
        public string Unit_DWC_Display
        {
            get
            {
                if (BudgetPositionStatusId == -1)
                {
                    return String.Concat(Unit_DWC == 0 ? "0" : String.Format("{0:0.0}", Unit_DWC).Replace(".0", ""), "%");
                }
                else
                {
                    return Convert.ToInt32(Unit_DWC).ToSafeString();
                }
            }
        }

        public string Unit_Total_Display
        {
            get
            {
                if (BudgetPositionStatusId == -1)
                {
                    return String.Concat(Unit_Total == 0 ? "0" : String.Format("{0:0.0}", Unit_Total).Replace(".0", ""), "%");
                }
                else
                {
                    return Convert.ToInt32(Unit_Total).ToSafeString();
                }
            }
        }
        public string Unit_Per_Display
        {
            get
            {
                if (BudgetPositionStatusId == 0)
                {
                    return String.Empty;
                }
                else
                {
                    return Unit_Per.HasValue ? String.Concat(Unit_Per == 0 ? "0" : String.Format("{0:0.0}", Unit_Per).Replace(".0", ""), "%") : String.Empty;
                }
            }
        }

    }
    public class QuarterlyReportManpowerBudgetCostViewModel : QuarterlyReportBaseViewModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string OrgShortName { get; set; }
        public int BudgetPositionStatusId { get; set; }

        public decimal Unit_DXB { get; set; }
        public decimal Unit_DWC { get; set; }
        public decimal Unit_Total { get; set; }
        public decimal? Unit_Per { get; set; }

        public string NameDisplay
        {
            get
            {
                if (BudgetPositionStatusId == -1)
                {

                    if (ReportMode == QuarterlyReportMode.Organization)
                    {
                        return String.Concat("% Cost");
                    }
                    else
                    {
                        return String.Concat("Total ", BusinessUnit, " Cost % vs.", Constant.DAName);
                    }
                }
                if (BudgetPositionStatusId == 0)
                {
                    return "Total";
                }
                else
                {
                    return Name;
                }
            }
        }
        public string Unit_DXB_Display
        {
            get
            {
                if (BudgetPositionStatusId == -1)
                {
                    return String.Concat(Unit_DXB == 0 ? "0" : String.Format("{0:0.0}", Unit_DXB).Replace(".0", ""), "%");
                }
                else
                {
                    return Unit_DXB == 0 ? "0" : String.Format("{0:0.0}", Unit_DXB);
                }
            }
        }
        public string Unit_DWC_Display
        {
            get
            {
                if (BudgetPositionStatusId == -1)
                {
                    return String.Concat(Unit_DWC == 0 ? "0" : String.Format("{0:0.0}", Unit_DWC).Replace(".0", ""), "%");
                }
                else
                {
                    return Unit_DWC == 0 ? "0" : String.Format("{0:0.0}", Unit_DWC);
                }
            }
        }

        public string Unit_Total_Display
        {
            get
            {
                if (BudgetPositionStatusId == -1)
                {
                    return String.Concat(Unit_Total == 0 ? "0" : String.Format("{0:0.0}", Unit_Total).Replace(".0", ""), "%");
                }
                else
                {
                    return Unit_Total == 0 ? "0" : String.Format("{0:0.0}", Unit_Total).Replace(".0", "");
                }
            }
        }
        public string Unit_Per_Display
        {
            get
            {
                if (BudgetPositionStatusId == 0)
                {
                    return String.Empty;
                }
                else
                {
                    return Unit_Per.HasValue ? String.Concat(Unit_Per == 0 ? "0" : String.Format("{0:0.0}", Unit_Per), "%").Replace(".0", "") : String.Empty;
                }


            }
        }

    }

    public class QuarterlyReportManpowerActualPositionViewModel : QuarterlyReportBaseViewModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string OrgShortName { get; set; }

        public int ActualPositionStatusId { get; set; }

        public int Unit_DXB { get; set; }
        public int Unit_DWC { get; set; }
        public int Unit_Total { get; set; }
        public decimal? Unit_Per { get; set; }
        public string NameDisplay
        {
            get
            {
                if (ActualPositionStatusId == -1)
                {
                    return String.Concat("Total ", BusinessUnit, " HC % vs.", Constant.DAName);
                }
                if (ActualPositionStatusId == 0)
                {
                    return "Total";
                }
                else
                {
                    return Name;
                }
            }
        }
        public string Unit_DXB_Display
        {
            get
            {
                if (ActualPositionStatusId == -1)
                {
                    return String.Concat(Unit_DXB == 0 ? "0" : String.Format("{0:0.0}", Unit_DXB).Replace(".0", ""), "%");
                }
                else
                {
                    return Convert.ToInt32(Unit_DXB).ToSafeString();
                }
            }
        }
        public string Unit_DWC_Display
        {
            get
            {
                if (ActualPositionStatusId == -1)
                {
                    return String.Concat(Unit_DWC == 0 ? "0" : String.Format("{0:0.0}", Unit_DWC).Replace(".0", ""), "%");
                }
                else
                {
                    return Convert.ToInt32(Unit_DWC).ToSafeString();
                }
            }
        }

        public string Unit_Total_Display
        {
            get
            {
                if (ActualPositionStatusId == -1)
                {
                    return String.Concat(Unit_Total == 0 ? "0" : String.Format("{0:0.0}", Unit_Total).Replace(".0", ""), "%");
                }
                else
                {
                    return Convert.ToInt32(Unit_Total).ToSafeString();
                }
            }
        }
        public string Unit_Per_Display
        {
            get
            {

                return Unit_Per.HasValue ? String.Concat(Unit_Per == 0 ? "0" : String.Format("{0:0.0}", Unit_Per).Replace(".0", ""), "%") : String.Empty;

            }
        }
    }

    public class QuarterlyReportManpowerActualCostViewModel : QuarterlyReportBaseViewModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string OrgShortName { get; set; }
        public int ActualCostStatusId { get; set; }

        public decimal Unit_DXB { get; set; }
        public decimal Unit_DWC { get; set; }
        public decimal Unit_Total { get; set; }
        public decimal? Unit_Per { get; set; }

        public decimal? Unit_DXB_Per { get; set; }
        public decimal? Unit_DWC_Per { get; set; }
        public decimal? Unit_Total_Per { get; set; }

        public string NameDisplay
        {
            get
            {
                if (ActualCostStatusId == -1)
                {
                    return String.Concat("Total ", BusinessUnit, " Cost % vs.", Constant.DAName);
                }
                if (ActualCostStatusId == 0)
                {
                    return "Variance Budget vs Actual";
                }
                else
                {

                    return Name;
                }
            }
        }
        public string Unit_DXB_Display
        {
            get
            {
                if (ActualCostStatusId == 0)
                {
                    return String.Concat(Unit_DXB == 0 ? "0" : String.Format("{0:0.0}", Unit_DXB).Replace(".0", ""), " (", Unit_DXB_Per.SafeValue() == 0 ? "0" : String.Format("{0:0.0}", Unit_DXB_Per.Value).Replace(".0", ""), "%)");

                }
                else
                {
                    return Unit_DXB == 0 ? "0" : String.Format("{0:0.0}", Unit_DXB).Replace(".0", "");
                }
            }
        }
        public string Unit_DWC_Display
        {
            get
            {
                if (ActualCostStatusId == 0)
                {
                    return String.Concat(Unit_DWC == 0 ? "0" : String.Format("{0:0.0}", Unit_DWC).Replace(".0", ""), " (", Unit_DWC_Per.SafeValue() == 0 ? "0" : String.Format("{0:0.0}", Unit_DWC_Per.Value).Replace(".0", ""), "%)");
                }
                else
                {
                    return Unit_DWC == 0 ? "0" : String.Format("{0:0.0}", Unit_DWC).Replace(".0", "");
                }
            }
        }

        public string Unit_Total_Display
        {
            get
            {
                if (ActualCostStatusId == 0)
                {
                    return String.Concat(Unit_Total == 0 ? "0" : String.Format("{0:0.0}", Unit_Total).Replace(".0", ""), " (", Unit_Total_Per.SafeValue() == 0 ? "0" : String.Format("{0:0.0}", Unit_Total_Per.Value).Replace(".0", ""), "%)");
                }
                else
                {
                    return Unit_Total == 0 ? "0" : String.Format("{0:0.0}", Unit_Total).Replace(".0", "");
                }
            }
        }
        public string Unit_Per_Display
        {
            get
            {

                return Unit_Per.HasValue ? String.Concat(Unit_Per == 0 ? "0" : String.Format("{0:0.0}", Unit_Per), "%").Replace(".0", "") : String.Empty;

            }
        }

    }

    public class QuarterlyReportGradeWiseHeadCountDistributionViewModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int GradeId { get; set; }

        [Display(Name = Constant.DXBName)]
        public int Unit_DXB { get; set; }
        [Display(Name = Constant.DWCName)]
        public int Unit_DWC { get; set; }
        public int Unit_Total { get; set; }
        public decimal Unit_Per { get; set; }
    }

    public class QuarterlyReportManagementLayerHeadCountViewModel
    {
        [Display(Name = "Management Layer")]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string ManagementLayerId { get; set; }
        [Display(Name = Constant.DXBName)]
        public int Unit_DXB { get; set; }
        [Display(Name = Constant.DWCName)]
        public int Unit_DWC { get; set; }

        [Display(Name = "Total")]
        public int Unit_Total { get; set; }
        public decimal Unit_Per { get; set; }
        public string Unit_Per_Display
        {
            get
            {

                return String.Concat(Unit_Per == 0 ? "0" : String.Format("{0:0.0}", Unit_Per).Replace(".0", ""), "%");

            }
        }
    }

    public class QuarterlyReportGradeWiseAverageCostViewModel
    {
        [Display(Name = "Management Layer %")]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int GradeId { get; set; }
        [Display(Name = "Unit")]
        public decimal Unit_Amount { get; set; }
        [Display(Name = Constant.DAName)]
        public decimal DA_Amount { get; set; }
    }

    public class QuarterlyReportManagementLayerAverageCostViewModel
    {
        [Display(Name = "Management Layer")]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int ManagementLayerId { get; set; }
        public decimal Unit_Amount { get; set; }
        public decimal DA_Amount { get; set; }

        public string Unit_Amount_Display
        {
            get
            {
                return Unit_Amount.ToCurrency().Replace(".00", "");
            }
        }
        [Display(Name = Constant.DAName)]
        public string DA_Amount_Display
        {
            get
            {
                return DA_Amount.ToCurrency().Replace(".00", "");

            }
        }


        public decimal Variance { get; set; }
        [Display(Name = "Variance")]
        public string Variance_Display
        {
            get
            {

                return String.Concat(Variance == 0 ? "0" : String.Format("{0:0.0}", Variance).Replace(".0", ""), "%");

            }
        }
        public string Status
        {
            get
            {

                return Variance == 0 ? "<span >NA</span>" : (Variance > 0 ? "<span class=\"rpt-red\">Red</span>" : "<span class=\"rpt-green\">Green</span>");

            }
        }



    }

    public class QuarterlyReportManagementLayerNationalSummaryViewModel
    {
        [Display(Name = "Management Layer")]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int ManagementLayerId { get; set; }
        public int Unit_DXB { get; set; }
        public int Unit_DWC { get; set; }
        public int Unit_Total { get; set; }
        public decimal Unit_Per { get; set; }
        public string Unit_Per_Display
        {
            get
            {

                return ManagementLayerId == 0 ? String.Empty : String.Concat(Unit_Per == 0 ? "0" : String.Format("{0:0.0}", Unit_Per).Replace(".0", ""), "%");

            }
        }
    }
    public class QuarterlyReportManagementLayerFemaleSummaryViewModel
    {
        [Display(Name = "Management Layer")]
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int ManagementLayerId { get; set; }
        public int Unit_DXB { get; set; }
        public int Unit_DWC { get; set; }
        public int Unit_Total { get; set; }
        public decimal Unit_Per { get; set; }
        public string Unit_Per_Display
        {
            get
            {

                return ManagementLayerId == 0 ? String.Empty : String.Concat(Unit_Per == 0 ? "0" : String.Format("{0:0.0}", Unit_Per).Replace(".0", ""), "%");

            }
        }
    }
}
