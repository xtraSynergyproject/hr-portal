using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class AttendanceToPayrollViewModel : ViewModelBase
    {
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? AttendanceDate { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? SearchDate { get; set; }
        public bool CheckFlag { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        public string Leave { get; set; }


        public AttendanceTypeEnum? SystemAttendance { get; set; }
        public TimeSpan? SystemOTHours { get; set; }
        public TimeSpan? SystemDeductionHours { get; set; }


        public AttendanceTypeEnum? OverrideAttendance { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? OverrideOTHours { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? OverrideDeductionHours { get; set; }


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

        [Display(Name = "Department Name")]
        public string OrganizationId { get; set; }
        [Display(Name = "Payroll Department Name")]
        public string PayrollOrganizationId { get; set; }
        [UIHint("TypeEditor1")]
        public string SectionId { get; set; }
        [UIHint("TypeEditor1")]
        [Display(Name = "Section")]
        public string SectionName { get; set; }
        public string UserId { get; set; }
        public string AssignmentId { get; set; }

        public string UserIds { get; set; }


        [Display(Name = "Contract End Date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ContractEndDate { get; set; }
        [Display(Name = "Contract Renewal")]
        public string ContractRenewable { get; set; }
        [Display(Name = "Sponsor")]
        public string Sponsor { get; set; }


        public string PersonId { get; set; }

        public PayrollPostedStatusEnum? PayrollPostedStatus { get; set; }
        public DateTime PayrollPostedDate { get; set; }

        [Display(Name = "Total Present")]
        public string TotalPresent { get; set; }
        [Display(Name = "Total Absent")]
        public string TotalAbsent { get; set; }
        [Display(Name = "Total Over Time")]
        public string TotalOT { get; set; }
        [Display(Name = "Total Deduction")]
        public string TotalDED { get; set; }

        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
        public string Day5 { get; set; }
        public string Day6 { get; set; }
        public string Day7 { get; set; }
        public string Day8 { get; set; }
        public string Day9 { get; set; }
        public string Day10 { get; set; }
        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }
        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }

        public bool P1 { get; set; }
        public bool P2 { get; set; }
        public bool P3 { get; set; }
        public bool P4 { get; set; }
        public bool P5 { get; set; }
        public bool P6 { get; set; }
        public bool P7 { get; set; }
        public bool P8 { get; set; }
        public bool P9 { get; set; }
        public bool P10 { get; set; }
        public bool P11 { get; set; }
        public bool P12 { get; set; }
        public bool P13 { get; set; }
        public bool P14 { get; set; }
        public bool P15 { get; set; }
        public bool P16 { get; set; }
        public bool P17 { get; set; }
        public bool P18 { get; set; }
        public bool P19 { get; set; }
        public bool P20 { get; set; }
        public bool P21 { get; set; }
        public bool P22 { get; set; }
        public bool P23 { get; set; }
        public bool P24 { get; set; }
        public bool P25 { get; set; }
        public bool P26 { get; set; }
        public bool P27 { get; set; }
        public bool P28 { get; set; }
        public bool P29 { get; set; }
        public bool P30 { get; set; }
        public bool P31 { get; set; }

        public TimeSpan? OT1 { get; set; }
        public TimeSpan? OT2 { get; set; }
        public TimeSpan? OT3 { get; set; }
        public TimeSpan? OT4 { get; set; }
        public TimeSpan? OT5 { get; set; }
        public TimeSpan? OT6 { get; set; }
        public TimeSpan? OT7 { get; set; }
        public TimeSpan? OT8 { get; set; }
        public TimeSpan? OT9 { get; set; }
        public TimeSpan? OT10 { get; set; }
        public TimeSpan? OT11 { get; set; }
        public TimeSpan? OT12 { get; set; }
        public TimeSpan? OT13 { get; set; }
        public TimeSpan? OT14 { get; set; }
        public TimeSpan? OT15 { get; set; }
        public TimeSpan? OT16 { get; set; }
        public TimeSpan? OT17 { get; set; }
        public TimeSpan? OT18 { get; set; }
        public TimeSpan? OT19 { get; set; }
        public TimeSpan? OT20 { get; set; }
        public TimeSpan? OT21 { get; set; }
        public TimeSpan? OT22 { get; set; }
        public TimeSpan? OT23 { get; set; }
        public TimeSpan? OT24 { get; set; }
        public TimeSpan? OT25 { get; set; }
        public TimeSpan? OT26 { get; set; }
        public TimeSpan? OT27 { get; set; }
        public TimeSpan? OT28 { get; set; }
        public TimeSpan? OT29 { get; set; }
        public TimeSpan? OT30 { get; set; }
        public TimeSpan? OT31 { get; set; }

        public TimeSpan? D1 { get; set; }
        public TimeSpan? D2 { get; set; }
        public TimeSpan? D3 { get; set; }
        public TimeSpan? D4 { get; set; }
        public TimeSpan? D5 { get; set; }
        public TimeSpan? D6 { get; set; }
        public TimeSpan? D7 { get; set; }
        public TimeSpan? D8 { get; set; }
        public TimeSpan? D9 { get; set; }
        public TimeSpan? D10 { get; set; }
        public TimeSpan? D11 { get; set; }
        public TimeSpan? D12 { get; set; }
        public TimeSpan? D13 { get; set; }
        public TimeSpan? D14 { get; set; }
        public TimeSpan? D15 { get; set; }
        public TimeSpan? D16 { get; set; }
        public TimeSpan? D17 { get; set; }
        public TimeSpan? D18 { get; set; }
        public TimeSpan? D19 { get; set; }
        public TimeSpan? D20 { get; set; }
        public TimeSpan? D21 { get; set; }
        public TimeSpan? D22 { get; set; }
        public TimeSpan? D23 { get; set; }
        public TimeSpan? D24 { get; set; }
        public TimeSpan? D25 { get; set; }
        public TimeSpan? D26 { get; set; }
        public TimeSpan? D27 { get; set; }
        public TimeSpan? D28 { get; set; }
        public TimeSpan? D29 { get; set; }
        public TimeSpan? D30 { get; set; }
        public TimeSpan? D31 { get; set; }

        public DateTime? A1 { get; set; }
        public DateTime? A2 { get; set; }
        public DateTime? A3 { get; set; }
        public DateTime? A4 { get; set; }
        public DateTime? A5 { get; set; }
        public DateTime? A6 { get; set; }
        public DateTime? A7 { get; set; }
        public DateTime? A8 { get; set; }
        public DateTime? A9 { get; set; }
        public DateTime? A10 { get; set; }
        public DateTime? A11 { get; set; }
        public DateTime? A12 { get; set; }
        public DateTime? A13 { get; set; }
        public DateTime? A14 { get; set; }
        public DateTime? A15 { get; set; }
        public DateTime? A16 { get; set; }
        public DateTime? A17 { get; set; }
        public DateTime? A18 { get; set; }
        public DateTime? A19 { get; set; }
        public DateTime? A20 { get; set; }
        public DateTime? A21 { get; set; }
        public DateTime? A22 { get; set; }
        public DateTime? A23 { get; set; }
        public DateTime? A24 { get; set; }
        public DateTime? A25 { get; set; }
        public DateTime? A26 { get; set; }
        public DateTime? A27 { get; set; }
        public DateTime? A28 { get; set; }
        public DateTime? A29 { get; set; }
        public DateTime? A30 { get; set; }
        public DateTime? A31 { get; set; }

        public string Id1 { get; set; }
        public string Id2 { get; set; }
        public string Id3 { get; set; }
        public string Id4 { get; set; }
        public string Id5 { get; set; }
        public string Id6 { get; set; }
        public string Id7 { get; set; }
        public string Id8 { get; set; }
        public string Id9 { get; set; }
        public string Id10 { get; set; }
        public string Id11 { get; set; }
        public string Id12 { get; set; }
        public string Id13 { get; set; }
        public string Id14 { get; set; }
        public string Id15 { get; set; }
        public string Id16 { get; set; }
        public string Id17 { get; set; }
        public string Id18 { get; set; }
        public string Id19 { get; set; }
        public string Id20 { get; set; }
        public string Id21 { get; set; }
        public string Id22 { get; set; }
        public string Id23 { get; set; }
        public string Id24 { get; set; }
        public string Id25 { get; set; }
        public string Id26 { get; set; }
        public string Id27 { get; set; }
        public string Id28 { get; set; }
        public string Id29 { get; set; }
        public string Id30 { get; set; }
        public string Id31 { get; set; }

        public string Total { get; set; }
        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }
        public bool Select { get; set; }
    }
}
