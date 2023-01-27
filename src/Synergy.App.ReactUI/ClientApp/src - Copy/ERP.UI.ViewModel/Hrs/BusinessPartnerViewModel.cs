using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class BusinessPartnerViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Display(Name = "Position Name")]
        public int? PositionId { get; set; }
        [Display(Name = "Position Name")]
        public string PositionName { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = AnnotationDeclaration.Labels.SequenceNo)]
        public virtual int? SequenceNo { get; set; }


        public virtual string Organizations { get; set; }



    }
}
