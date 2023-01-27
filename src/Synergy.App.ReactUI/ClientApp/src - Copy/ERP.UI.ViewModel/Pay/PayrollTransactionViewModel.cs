using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PayrollTransactionViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Person")]
        public long PersonId { get; set; }
        public long? UserId { get; set; }
        [Required]
        [Display(Name = "Element")]
        public long ElementId { get; set; }
        public long PostedUserId { get; set; }

        [Display(Name = "Payroll Group")]
        public long? PayrollGroupId { get; set; }
        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }
        public long? PayrollRunRelationshipId { get; set; }
        [Display(Name = "Payroll Department Name")]
        public long? PayrollOrganizationId { get; set; }

        public string Name { get; set; }
        public string ElementName { get; set; }
        public string ElementCode { get; set; }
        public string Description { get; set; }
        public double EarningAmount { get; set; }
        public double DeductionAmount { get; set; }
        public double TotalAmount { get; set; }
        [Required]
        public double Amount { get; set; }
        public DateTime PostedDate { get; set; }
        public PayrollPostedSourceEnum PostedSource { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EffectiveDate { get; set; }

        public PayrollProcessStatusEnum ProcessStatus { get; set; }
        public DateTime? ProcessedDate { get; set; }

        public ElementTypeEnum ElementType { get; set; }
        public virtual DateTime? ElementEffectiveStartDate { get; set; }
        public virtual DateTime? ElementEffectiveEndDate { get; set; }
        public ElementCategoryEnum ElementCategory { get; set; }
        public ElementClassificationEnum ElementClassification { get; set; }

        public NodeEnum? ReferenceNode { get; set; }
        public long? ReferenceId { get; set; }

        [Display(Name = "Payroll Organization")]
        public long? OrganizationId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        [Display(Name = "Iqamah No")]
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string UserNameWithEmail { get; set; }
        public string Email { get; set; }
        public string IqamahNo { get; set; }
        public string DisplayName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string JobName { get; set; }
        public string GradeName { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }
        public string CostCenter { get; set; }
        [Display(Name = "Contract End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEndDate { get; set; }
        [Display(Name = "Contract Renewal")]
        public string ContractRenewable { get; set; }
        [Display(Name = "Sponsor")]
        public string Sponsor { get; set; }

        [Display(Name = "Date Of Join")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfJoin { get; set; }

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
        public bool IsNewEntry { get; set; }

        public double? Quantity { get; set; }
        public TimeSpan? OverTime { get; set; }
        public TimeSpan? DeductionTime { get; set; }
        public double? Rate { get; set; }
        public PayrollUomEnum? Uom { get; set; }
        [Display(Name = "Opening Balance")]
        public double? OpeningBalance { get; set; }
        [Display(Name = "Closing Balance")]
        public double? ClosingBalance { get; set; }


        public double EarningQuantity { get; set; }
        public double DeductionQuantity { get; set; }
        public double? OpeningQuantity { get; set; }
        public double? ClosingQuantity { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime EndDate { get; set; }

        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }

        [Display(Name = "Attachment")]
        public long? AttachmentId { get; set; }
        public FileViewModel SelectedFile { get; set; }

        public bool? IsTransactionClosed { get; set; }
    }
}
