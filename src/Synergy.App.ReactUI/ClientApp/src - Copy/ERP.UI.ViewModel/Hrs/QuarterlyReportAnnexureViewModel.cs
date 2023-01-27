using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class QuarterlyReportAnnexureViewModel : QuarterlyReportBaseViewModel
    {
        public QuarterlyReportAnnexureViewModel()
        {
            Vacancy = new QuarterlyReportAnnexureVacancyViewModel();
            GrowthTrend = new QuarterlyReportAnnexureGrowthTrendViewModel();
        }
        public QuarterlyReportAnnexureViewModel(QuarterlyReportBaseViewModel baseModel)
        {

            this.MapValue(baseModel);

            Vacancy = new QuarterlyReportAnnexureVacancyViewModel();
            GrowthTrend = new QuarterlyReportAnnexureGrowthTrendViewModel(baseModel);
        }
        public QuarterlyReportAnnexureVacancyViewModel Vacancy { get; set; }
        public QuarterlyReportAnnexureGrowthTrendViewModel GrowthTrend { get; set; }


    }
    public class QuarterlyReportAnnexureVacancyViewModel : QuarterlyReportBaseViewModel
    {

        public List<QuarterlyReportNewVacancyViewModel> NewVacancyList { get; set; }
        public string VacancyTitle { get; set; }
        public string VacancyComment { get; set; }


        public List<QuarterlyReportBudgetedVacancyViewModel> BudgetedVacancyList { get; set; }
        public string BudgetTitle { get; set; }
        public string BudgetComment { get; set; }
        public int TotalVacancyCount { get; set; }
    }
    public class QuarterlyReportNewVacancyViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Group")]
        public string Group { get; set; }
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }
        [Display(Name = "Position")]
        public string PositionName { get; set; }
        [Display(Name = "Grade")]
        public string GradeName { get; set; }
        [Display(Name = "DXB")]
        public int Unit_DXB { get; set; }
        [Display(Name = "DWC")]
        public int Unit_DWC { get; set; }
        [Display(Name = "Total")]
        public int Total { get; set; }
    }
    public class QuarterlyReportBudgetedVacancyViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Group")]
        public string Group { get; set; }
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }
        [Display(Name = "Position")]
        public string PositionName { get; set; }
        [Display(Name = "Grade")]
        public string GradeName { get; set; }
        [Display(Name = "DXB")]
        public int Unit_DXB { get; set; }
        [Display(Name = "DWC")]
        public int Unit_DWC { get; set; }
        [Display(Name = "Total")]
        public int Total { get; set; }
    }
    public class QuarterlyReportAnnexureGrowthTrendViewModel : QuarterlyReportBaseViewModel
    {
        public QuarterlyReportAnnexureGrowthTrendViewModel()
        {

        }
        public QuarterlyReportAnnexureGrowthTrendViewModel(QuarterlyReportBaseViewModel baseModel)
        {

            this.MapValue(baseModel);

        }
        public List<QuarterlyReportGrowthViewModel> GrowthList { get; set; }
        public string GrowthTitle { get; set; }
        public string GrowthComment { get; set; }
        public string Note { get; set; }

        public string AsteriskComment
        {
            get
            {
                if (RptCategoryId == 4 || RptCategoryId == 3){
                    return string.Concat("*", (GrowthYear - 1).ToSafeString(), " figures are based on budget numbers.");
                }
                else{
                    return "";
                }
            }
        }


        public string Previous1Column
        {
            get
            {
                return (RptCategoryId == 4 || RptCategoryId == 3) ? String.Concat((GrowthYear - 1).ToSafeString(), "*") : (GrowthYear - 1).ToSafeString();
            }
        }
        public string Previous2Column
        {
            get
            {
                return (GrowthYear - 2).ToSafeString();
            }
        }
        public string Previous3Column
        {
            get
            {
                return (GrowthYear - 3).ToSafeString();
            }
        }
        public string Previous4Column
        {
            get
            {
                return (GrowthYear - 4).ToSafeString();
            }
        }
        public string Previous5Column
        {
            get
            {
                return (GrowthYear - 5).ToSafeString();
            }
        }
    }
    public class QuarterlyReportGrowthViewModel : QuarterlyReportBaseViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Previous1 { get; set; }
        public decimal? Previous2 { get; set; }
        public decimal? Previous3 { get; set; }
        public decimal? Previous4 { get; set; }
        public decimal? Previous5 { get; set; }

        public string Previous1Display
        {
            get
            {
                return Display(Previous1);
            }
        }
        public string Previous2Display
        {
            get
            {
                return Display(Previous2);
            }
        }
        public string Previous3Display
        {
            get
            {
                return Display(Previous3);
            }
        }
        public string Previous4Display
        {
            get
            {
                return Display(Previous4);
            }
        }
        public string Previous5Display
        {
            get
            {
                return Display(Previous5);
            }
        }

        private string Display(decimal? value)
        {
            if (Id == 2 || Id == 4 || Id == 5)
            {
                return value.HasValue ? string.Concat(value.SafeValue().ToString().Replace(".00", "").Replace(".0", ""), "%") : string.Empty;
            }
            else if (Id == 1 || Id == 3)
            {
                return value.HasValue ? value.ToCurrency().Replace(".00", "").Replace(".0", "") : string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        public List<QuarterlyReportGrowthTrendViewModel> TrendList
        {
            get
            {
                if (Id == 2 || Id == 4)
                {
                    return null;
                }
                var i = RptCategoryId == 4 || RptCategoryId == 3 ? 0 : 1;
                var list = new List<QuarterlyReportGrowthTrendViewModel>();
                list.Add(new QuarterlyReportGrowthTrendViewModel { Year = GrowthYear - 5, Value = Previous5 });
                list.Add(new QuarterlyReportGrowthTrendViewModel { Year = GrowthYear - 4, Value = Previous4 });
                list.Add(new QuarterlyReportGrowthTrendViewModel { Year = GrowthYear - 3, Value = Previous3 });
                list.Add(new QuarterlyReportGrowthTrendViewModel { Year = GrowthYear - 2, Value = Previous2 });
                list.Add(new QuarterlyReportGrowthTrendViewModel { Year = GrowthYear - 1, Value = Previous1 });
                return list;
            }
        }
    }
    public class QuarterlyReportGrowthTrendViewModel
    {
        public int Year { get; set; }
        public decimal? Value { get; set; }
    }
}
