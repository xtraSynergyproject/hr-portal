using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class CompensationLevelViewModel
    {
        public string Level { get; set; }
        public string Department { get; set; }
        public string IndustryMaxSalary { get; set; }
        public string IndustryMinSalary { get; set; }
        public string AverageAnnualSalary { get; set; }
    }

    public class TopPerformaceViewModel
    {
        public string Employee { get; set; }
        public string IndustryMaxSalary { get; set; }
        public string IndustryMinSalary { get; set; }
        public string AverageAnnualSalary { get; set; }
    }

    public class KPISViewModel
    {
        public string KPIs { get; set; }
        public string Current { get; set; }
        public string Status { get; set; }
    }

    public class HrSurveyResult
    {
        public string SurveyField { get; set; }
        public string AvgScore { get; set; }
        public string ScoreVsGoals { get; set; }
    }

    public class DivisionScoreCard
    {
        public string Devision { get; set; }
        public string Trend { get; set; }
        public string VsPlan { get; set; }
        public string Total { get; set; }
        public string Fte { get; set; }
        public string HeadCount { get; set; }
    }

    public class GuageViewModel
    {
        public string Color { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Opacity { get; set; }
    }

    public class KPIHumanResourceViewModel
    {
        public string KPI { get; set; }
        public string Hash { get; set; }
        public string Percent { get; set; }
        public string Trend { get; set; }
    }

    public class DepartmentHeadCountViewModel
    {
        public string Department { get; set; }
        public string HeadCountFTE { get; set; }
        public string HeadcountEmployees { get; set; }
        public string HeadcountTotalPercent { get; set; }
        public string InHires { get; set; }
        public string InTransfers { get; set; }
        public string OutDepartures { get; set; }
        public string OutTranfers { get; set; }
        public string VarianceNet { get; set; }
        public string VariancePercent { get; set; }
    }

    public class RecruitmentHireViewModel
    {
        public string Division { get; set; }
        public string Employee { get; set; }
        public string Supervisor { get; set; }
        public string HireDate { get; set; }
    }

    public class RecruitmentHireByDepartmentViewModel
    {
        public string Department { get; set; }
        public string HiringRate { get; set; }
        public string Hires { get; set; }
        public string Employees { get; set; }
    }

    public class QuaterlyTrendViewModel
    {
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public ReasonForLeavingEnum? ReasonForLeaving { get; set; }
        public string Quarter { get; set; }
        public string DepQuarter { get; set; }
        public DateTime? ActualTerminationDate { get; set; }
        public string Department { get; set; }
        public string Supervisor { get; set; }
        public DateTime? EffectiveEndDate { get; set; }

    }

    public class AttritionScoreViewModel
    {
        public List<double> BarValues { get; set; }
        public List<string> Categories { get; set; }
        public int? Quarter1 { get; set; }
        public int? Quarter2 { get; set; }
        public int? Quarter3 { get; set; }
        public int? Quarter4 { get; set; }
        public int? VolDepartQuarter1 { get; set; }
        public int? VolDepartQuarter2 { get; set; }
        public int? VolDepartQuarter3 { get; set; }
        public int? VolDepartQuarter4 { get; set; }
        public int? AttritionRateQuarter1 { get; set; }
        public int? AttritionRateQuarter2 { get; set; }
        public int? AttritionRateQuarter3 { get; set; }
        public int? AttritionRateQuarter4 { get; set; }
        public long? Year { get; set; }

    }

    public class CompensationScoreViewModel
    {
        public string SalaryName { get; set; }
        public DateTime? PayrollRunDate { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }
        public DateTime DateOfJoin { get; set; }
        public string DateOfJoinText { get { return DateOfJoin.ToDefaultDateFormat(); } }
        public int? YearMonth { get; set; }
        public MonthEnum? Month { get; set; }
        public int? Year { get; set; }
        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }
        public double VacationBalance { get; set; }
        public double GrossSalary { get; set; }

        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public PaymentModeEnum PaymentMode { get; set; }
        public PayrollIntervalEnum? PayrollInterval { get; set; }
        public DocumentStatusEnum PublishStatus { get; set; }
        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }

        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }
        public long? PersonId { get; set; }
        public long? UserId { get; set; }
        public string PersonFullName { get; set; }
        public string SponsorshipNo { get; set; }


        public long? PayrollGroupId { get; set; }
        public long? PayrollCalendarId { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public long? JobId { get; set; }
        public string JobName { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public long? GradeId { get; set; }
        public string GradeName { get; set; }
        public long? LocationId { get; set; }
        public string LocationName { get; set; }
        public string PersonNo { get; set; }
        public string NetAmountInWords { get; set; }

        public string[] Elements { get; set; }

        public double ActualWorkingDays { get; set; }
        public double EmployeeWorkingDays { get; set; }
        public double AnnualLeaveDays { get; set; }
        public double SickLeaveDays { get; set; }
        public double UnpaidLeaveDays { get; set; }
        public double OtherLeaveDays { get; set; }
        public TimeSpan? UnderTime { get; set; }
        public TimeSpan? OverTime { get; set; }
        public String CurrencyCode { get; set; }
        public String CompanyNameBasedOnLegalEntity { get; set; }
        public string Quarter { get; set; }
        public double? Quarter1Salary { get; set; }
        public double? Quarter2Salary { get; set; }
        public double? Quarter3Salary { get; set; }
        public double? Quarter4Salary { get; set; }
        public double? Quarter1Bonus { get; set; }
        public double? Quarter2Bonus { get; set; }
        public double? Quarter3Bonus { get; set; }
        public double? Quarter4Bonus { get; set; }
        public double? AverageSalaryQ1 { get; set; }
        public double? AverageSalaryQ2 { get; set; }
        public double? AverageSalaryQ3 { get; set; }
        public double? AverageSalaryQ4 { get; set; }
        public List<double> DepartmentCompensation { get; set; }
        public List<string> Categories { get; set; }
        public string Department { get; set; }
        public List<double> QuarterList { get; set; }
    }

    public class SeriesViewModel
    {
        public string Column { get; set; }
        public string Field { get; set; }
        public string Name { get; set; }
        public string Stack { get; set; }
        public string Category { get; set; }
    }

    public class HrPerformanceScoreCardViewModel
    {
        public double? VacantPosition { get; set; }

        public double? NewEmployee { get; set; }
        public double? AttritionRate { get; set; }
        public double? OvertimeRate { get; set; }
        public double? InternalTransfer { get; set; }
        public double? CompensationRate { get; set; }
    }

    public class PositionDetailViewModel
    {
        public long? PositionRootId { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public DateTime? ParentPositionEffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public long? RelationshipId { get; set; }
        public string Status { get; set; }
        public long? ParentPositionId { get; set; } 
        public string ParentPositionName { get; set; }
        public long? HierarchyId { get; set; }
        public long? UserId { get; set; }
    }

    public class InternalTransferViewModel
    {
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string Department { get; set; }
        public DateTime? CurrectStartDate { get; set; }
        public DateTime? CurrentEndDate { get; set; }
        public DateTime? PreviousStartDate { get; set; }
        public DateTime? PreviousEndDate { get; set; }
    }

    public class HrSummaryScoreViewModel
    {
        public List<double> BarValues { get; set; }
        public List<string> Categories { get; set; }
        public int? Quarter1 { get; set; }
        public int? Quarter2 { get; set; }
        public int? Quarter3 { get; set; }
        public int? Quarter4 { get; set; }
        public int? EmpDepartQuarter1 { get; set; }
        public int? EmpDepartQuarter2 { get; set; }
        public int? EmpDepartQuarter3 { get; set; }
        public int? EmpDepartQuarter4 { get; set; }
        public int? EmpNewQuarter1 { get; set; }
        public int? EmpNewQuarter2 { get; set; }
        public int? EmpNewQuarter3 { get; set; }
        public int? EmpNewQuarter4 { get; set; }
        public double? AttritionRateQuarter1 { get; set; }
        public double? AttritionRateQuarter2 { get; set; }
        public double? AttritionRateQuarter3 { get; set; }
        public double? AttritionRateQuarter4 { get; set; }
        public double? AvgTenureQuarter1 { get; set; }
        public double? AvgTenureQuarter2 { get; set; }
        public double? AvgTenureQuarter3 { get; set; }
        public double? AvgTenureQuarter4 { get; set; }
        public double? HireRateQuarter1 { get; set; }
        public double? HireRateQuarter2 { get; set; }
        public double? HireRateQuarter3 { get; set; }
        public double? HireRateQuarter4 { get; set; }
        public double? TurnOverRateQuarter1 { get; set; }
        public double? TurnOverRateQuarter2 { get; set; }
        public double? TurnOverRateQuarter3 { get; set; }
        public double? TurnOverRateQuarter4 { get; set; }
        public int? Year { get; set; }
    }

    public class EmployeeQuaterScoreViewModel
    {
        public string Quarter { get; set; }
        public int Total { get; set; }
        public int New { get; set; }
        public int Departed { get; set; }
        public int AvgTenure { get; set; }
    }

}
