using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class MisconductViewModel : DatedViewModelBase
    {
        public long PersonId { get; set; }
        public long UserId { get; set; }
        public string FineAmountType { get; set; }
        public string ServiceNo { get; set; }
        public string DisciplinaryActionTaken { get; set; }
        public string MisconductType { get; set; }
        public string Name { get; set; }
        public long ServiceId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? MisconductDate { get; set; }
        public string FineAmount { get; set; }
        public string NoOfDays { get; set; }
        public string InvestigationResult { get; set; }
        public string Description { get; set; }

        public string PersonName { get; set; }
        [Display(Name = "Iqamah No")]
        public string SponsorshipNo { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string JobName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfJoin { get; set; }
        public string PersonNo { get; set; }

    }
}
