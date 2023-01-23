using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class MyDASJobRequestViewModel : BaseRequestViewModel
    {
        public  int Id { get; set; }

        public TransactionMode Mode { get; set; }

        public  int? OldMyDASJobId { get; set; }

        public  int? MyDASJobId { get; set; }


        [Required]
        [Display(Name = "Grade Name")]
        public int? GradeId { get; set; }

        [Display(Name = "Grade Name")]
        public string GradeName { get; set; }


        [Required]
        [Display(Name = "Job Family Name")]
        public int? JobFamilyId { get; set; }

        [Display(Name = "Job Family Name")]
        public string JobFamilyName { get; set; }


        [Required]
        [StringLength(200)]
        [Display(Name = "MyDAS Job Name")]
        public string Name { get; set; }

        [StringLength(200)]
        [Display(Name = "MyDAS Job Name (In Arabic)")]
        public string NameAr { get; set; }

        public  int? FusionReferenceId { get; set; }

        [StringLength(2000)]
        [Display(Name = "MyDAS Job Description")]
        public  string Description { get; set; }

        [Display(Name = "Sequence No")]
        public  int? SequenceNo { get; set; }

        [Display(Name = "Sequence No")]
        public string SequenceNoName { get; set; }

        [Display(Name = "Requested By")]
        public  string RequestedBy { get; set; }

        [Display(Name = "Requested Date")]
        public  DateTime? RequestedDate { get; set; }
        public string Source { get; set; }


    }
}
