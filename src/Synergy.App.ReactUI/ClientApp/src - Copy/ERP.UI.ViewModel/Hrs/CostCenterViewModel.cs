using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class CostCenterViewModel : ViewModelBase
    {
        //public override long Id { get; set; }
       // public long? CostCenterId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Cost Center Code")]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Cost Center Name")]
        public string Name { get; set; }

        [Display(Name = "Cost Center Holder Position")]
        public long? PositionId { get; set; }


        [Display(Name = "Cost Center Holder")]
        public Nullable<long> PersonId { get; set; }

        [Display(Name = "Cost Center Holder Position")]
        public string CostCenterHolderPosition { get; set; }

        [Display(Name = "Cost Center Holder")]
        public string CostCenterHolder { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public int? SequenceNo { get; set; }

        [Display(Name = "Legal Entity")]
        public long LegalEntityId { get; set; }

        [Display(Name = "Legal Entity")]
        public string LegalEntityName { get; set; }


    }
}
