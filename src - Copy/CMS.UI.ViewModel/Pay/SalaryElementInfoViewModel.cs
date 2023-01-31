using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class SalaryElementInfoViewModel:NoteTemplateViewModel
    {
        public string SalaryInfoId { get; set; }
        [Required]
        [Display(Name = "Element")]
        public string ElementId { get; set; }
        [Display(Name = "Element")]
        public string ElementName { get; set; }
        public string ElementCode { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public DateTime? EffectiveStartDate { get; set; }
        [Required]
        public DateTime? EffectiveEndDate { get; set; }
        public string PersonId { get; set; }
        public DateTime? ElementEffectiveStartDate { get; set; }
        public DateTime? ElementEffectiveEndDate { get; set; }
    }
}
