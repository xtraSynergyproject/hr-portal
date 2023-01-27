using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
  public class RemoteSignInSignOutViewModel
    {
        public PunchingTypeEnum PunchingType { get; set; }
        public string PunchingTypeText
        {
            get { return PunchingType.Description(); }
        }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]

        public DateTime? PunchingDate { get; set; }
        public long? LocationId { get; set; }
        public string LocationName{ get; set; }
    public string ServiceStatusName { get; set; }
        [Display(Name = "Service No")]
        public string ServiceNo { get; set; }
        public string ServiceOwner { get; set; }
        public long ServiceId { get; set; }
        public NtsActionEnum TemplateAction { get; set; }
    }
}
