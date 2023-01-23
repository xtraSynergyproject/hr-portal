using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class PayrollTransactionViewModel : NtsNote
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime EndDate { get; set; }
        public double EarningAmount { get; set; }
        public double DeductionAmount { get; set; }
        public double TotalAmount { get; set; }
        public double Amount { get; set; }
        public double? OpeningBalance { get; set; }
        public double? ClosingBalance { get; set; }
        public DateTime PostedDate { get; set; }
        public PayrollPostedSourceEnum PostedSource { get; set; }
        public PayrollProcessStatusEnum ProcessStatus { get; set; }
        public string ProcessStatusId { get; set; }
        public string ProcessStatusCode { get; set; }
        public string ElementCode { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string PostedUserId { get; set; }

        public ElementTypeEnum ElementType { get; set; }
        public ElementCategoryEnum ElementCategory { get; set; }
        public ElementClassificationEnum ElementClassification { get; set; }
        public string PayrollOrganizationId { get; set; }
        public NodeEnum? ReferenceNode { get; set; }
        public string ReferenceId { get; set; }

        public string PayrollId { get; set; }
        public string PayrollRunId { get; set; }
        public string PayrollGroupId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public double? Rate { get; set; }
        public PayrollUomEnum? Uom { get; set; }

        //public TimeSpan? OverTime { get; set; }
        //public TimeSpan? DeductionTime { get; set; }

        public double? Quantity { get; set; }
        public double? EarningQuantity { get; set; }
        public double? DeductionQuantity { get; set; }
        public double? OpeningQuantity { get; set; }
        public double? ClosingQuantity { get; set; }
        public bool? IsTransactionClosed { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string IqamahNo { get; set; }
        public string JobName { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationId { get; set; }
        public string ElementName { get; set; }
        public string AttachmentId { get; set; }
        public DataOperation Operation { get; set; }
        public string ElementId { get; set; }
        public string PersonId { get; set; }
        public string NtsNoteId { get; set; }

        public double Day1Amount { get; set; }
        public double Day2Amount { get; set; }
        public double Day3Amount { get; set; }
        public double Day4Amount { get; set; }
        public double Day5Amount { get; set; }
        public double Day6Amount { get; set; }
        public double Day7Amount { get; set; }
        public double Day8Amount { get; set; }
        public double Day9Amount { get; set; }
        public double Day10Amount { get; set; }
        public double Day11Amount { get; set; }
        public double Day12Amount { get; set; }
        public double Day13Amount { get; set; }
        public double Day14Amount { get; set; }
        public double Day15Amount { get; set; }
        public double Day16Amount { get; set; }
        public double Day17Amount { get; set; }
        public double Day18Amount { get; set; }
        public double Day19Amount { get; set; }
        public double Day20Amount { get; set; }
        public double Day21Amount { get; set; }
        public double Day22Amount { get; set; }
        public double Day23Amount { get; set; }
        public double Day24Amount { get; set; }
        public double Day25Amount { get; set; }
        public double Day26Amount { get; set; }
        public double Day27Amount { get; set; }
        public double Day28Amount { get; set; }
        public double Day29Amount { get; set; }
        public double Day30Amount { get; set; }
        public double Day31Amount { get; set; }

        public PayrollProcessStatusEnum? Day1Status { get; set; }
        public PayrollProcessStatusEnum? Day2Status { get; set; }
        public PayrollProcessStatusEnum? Day3Status { get; set; }
        public PayrollProcessStatusEnum? Day4Status { get; set; }
        public PayrollProcessStatusEnum? Day5Status { get; set; }
        public PayrollProcessStatusEnum? Day6Status { get; set; }
        public PayrollProcessStatusEnum? Day7Status { get; set; }
        public PayrollProcessStatusEnum? Day8Status { get; set; }
        public PayrollProcessStatusEnum? Day9Status { get; set; }
        public PayrollProcessStatusEnum? Day10Status { get; set; }
        public PayrollProcessStatusEnum? Day11Status { get; set; }
        public PayrollProcessStatusEnum? Day12Status { get; set; }
        public PayrollProcessStatusEnum? Day13Status { get; set; }
        public PayrollProcessStatusEnum? Day14Status { get; set; }
        public PayrollProcessStatusEnum? Day15Status { get; set; }
        public PayrollProcessStatusEnum? Day16Status { get; set; }
        public PayrollProcessStatusEnum? Day17Status { get; set; }
        public PayrollProcessStatusEnum? Day18Status { get; set; }
        public PayrollProcessStatusEnum? Day19Status { get; set; }
        public PayrollProcessStatusEnum? Day20Status { get; set; }
        public PayrollProcessStatusEnum? Day21Status { get; set; }
        public PayrollProcessStatusEnum? Day22Status { get; set; }
        public PayrollProcessStatusEnum? Day23Status { get; set; }
        public PayrollProcessStatusEnum? Day24Status { get; set; }
        public PayrollProcessStatusEnum? Day25Status { get; set; }
        public PayrollProcessStatusEnum? Day26Status { get; set; }
        public PayrollProcessStatusEnum? Day27Status { get; set; }
        public PayrollProcessStatusEnum? Day28Status { get; set; }
        public PayrollProcessStatusEnum? Day29Status { get; set; }
        public PayrollProcessStatusEnum? Day30Status { get; set; }
        public PayrollProcessStatusEnum? Day31Status { get; set; }

        public List<ElementViewModel> ElementList { get; set; }
        public List<PayrollPersonViewModel> EmployeeList { get; set; }
        public List<SalaryElementInfoViewModel> EmployeeSalaryElementInfoList { get; set; }
        public List<PayrollTransactionViewModel> SalaryTransactionList { get; set; }

        public string PortalName { get; set; }
    }
}
