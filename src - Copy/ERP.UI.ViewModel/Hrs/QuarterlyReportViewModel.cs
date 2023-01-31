using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.Globalization;

namespace ERP.UI.ViewModel
{
    public class QuarterlyReportViewModel : QuarterlyReportBaseViewModel
    {
        public string Html { get; set; }
        public int PageNo { get; set; }
        public int PageWidth { get; set; }
        public int PageHeight { get; set; }
        public string Description { get; set; }

        public QuarterlyReportFrontPageViewModel FrontPage { get; set; }
        public QuarterlyReportBudgetViewModel BudgetPage { get; set; }
        public QuarterlyReportTrendViewModel TrendPage { get; set; }
        public QuarterlyReportAnnexureViewModel Annexure { get; set; }
    }

    public class QuarterlyReportBaseViewModel
    {

        public int Year { get; set; }
        public int GrowthYear
        {
            get
            {
                return RptCategoryId == 4 || RptCategoryId == 3 ? Year + 1 : Year;
            }
        }


        public QuarterlyReportMode ReportMode { get; set; }
        public string UnitTotalName { get; set; }
        public string UnitPercentageName { get; set; }

        public string Title { get; set; }
        public string Comments { get; set; }
        public string FooterMessage { get; set; }
        public string PageNoText { get; set; }
        public DateTime ReportFromDate { get; set; }
        public DateTime ReportToDate { get; set; }
        public DateTime ReportDate { get; set; }
        public int RptGenerateId { get; set; }
        public int RptCategoryId { get; set; }
        public int OrganizationId { get; set; }

        public int OwnerId { get; set; }
        public int PositionId { get; set; }
        public string PositionTitle { get; set; }
        public string OrgName { get; set; }
        public string OrgShortName { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public string Salutation { get; set; }

        public string QuarterName { get; set; }
        public string QuarterShortName { get; set; }

        public string DAName { get { return Constant.DAName; } }
        public string DXBName { get { return Constant.DXBName; } }
        public string DWCName { get { return Constant.DWCName; } }
        public string QuarterRangeName
        {
            get
            {
                return String.Concat(ReportFromDate.ToString("MMM", CultureInfo.InvariantCulture), "-", ReportDate.ToString("MMM", CultureInfo.InvariantCulture), " ", Year);
            }
        }
        public string OrganizationFullName { get { return OrgName.ToUpper(); } }
        public string QuarterFullName
        {
            get
            {
                return string.Concat(QuarterName, " (", QuarterRangeName, ")");
            }
        }
        public string QuarterEndtMonthAndYear { get { return ReportToDate.ToString("MMM-yyyy", CultureInfo.InvariantCulture); } }

        public string Font { get { return "13px Tahoma,Verdana,Helvetica,Sans-Serif"; } }
        public string FontSmall { get { return "12px Tahoma,Verdana,Helvetica,Sans-Serif"; } }
        public string FontMedium { get { return "14px Tahoma,Verdana,Helvetica,Sans-Serif"; } }
        public string UnitColor { get { return "#3399FF"; } }
        public string TotalColor { get { return "#7E8083"; } }
        public string DAColor { get { return "#002D5B"; } }
        public string DXBColor { get { return "#3399FF"; } }
        public string DWCColor { get { return "#002D5B"; } }
        public string BlueColor { get { return "#36B3DF"; } }
        public string LimeColor { get { return "#F57128"; } }
        public string ReportBorderColor { get { return "silver"; } }
        public string BusinessUnit { get { return "BU"; } }
        public bool CanEditComment { get; set; }
        public void MapValue(QuarterlyReportBaseViewModel baseModel)
        {
            Year = baseModel.Year;
            QuarterName = baseModel.QuarterName;
            ReportFromDate = baseModel.ReportFromDate;
            ReportToDate = baseModel.ReportToDate;
            RptGenerateId = baseModel.RptGenerateId;
            ReportDate = baseModel.ReportDate;
            OwnerId = baseModel.OwnerId;
            PositionId = baseModel.PositionId;
            PositionTitle = baseModel.PositionTitle;
            OrgName = baseModel.OrgName;
            OrgShortName = baseModel.OrgShortName;
            EmployeeName = baseModel.EmployeeName;
            Gender = baseModel.Gender;
            CanEditComment = baseModel.CanEditComment;
            OrganizationId = baseModel.OrganizationId;
            Salutation = baseModel.Salutation;
            ReportMode = baseModel.ReportMode;
            RptCategoryId = baseModel.RptCategoryId;
        }

    }

}
