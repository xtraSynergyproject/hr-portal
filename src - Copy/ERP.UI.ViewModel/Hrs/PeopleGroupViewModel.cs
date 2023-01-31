using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PeopleGroupViewModel : BaseViewModel
    {
        public int Id { get; set; }

     
        [Required]
        [Display(Name = "Nationality Group")]
        public string NationalityGroupCode { get; set; }


        [Display(Name = "Nationality Group")]
        public string NationalityGroupName { get; set; }

        [Display(Name = "Housing Status")]
        public string HousingStatusCode { get; set; }


        [Display(Name = "Housing Status")]
        public string HousingStatusName { get; set; }

        [Display(Name = "Car Status")]
        public string CarStatusCode { get; set; }


        [Display(Name = "Car Status")]
        public string CarStatusName { get; set; }

        [Display(Name = "Staff Category")]
        public string StaffCategoryCode { get; set; }


        [Display(Name = "Staff Category")]
        public string StaffCategoryName { get; set; }

        [Display(Name = "Entity")]
        public string EntityCode { get; set; }


        [Display(Name = "Entity")]
        public string EntityName { get; set; }

        [Display(Name = "Working Specialty")]
        public string WorkingSpecialtyCode { get; set; }


        [Display(Name = "Working Specialty")]
        public string WorkingSpecialtyName { get; set; }

        [Display(Name = "Retirement Category")]
        public string RetirementCategoryCode { get; set; }


        [Display(Name = "Retirement Category")]
        public string RetirementCategoryName { get; set; }

        public int? TransactionId { get; set; }

    }
}
