using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class DatedViewModelBase : ViewModelBase
    {

        private DateTime? _EffectiveStartDate;
        private DateTime? _EffectiveEndDate;

        public virtual bool IsFirstItem { get; set; }
        public virtual bool IsLastItem { get; set; }
        public virtual bool IsLatest { get; set; }

        public long? RootId { get; set; }

        //[Required]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [DataType(DataType.Date)]
        [DateRange]
        //[Display(Name = Constant.Annotation.Labels.EffectiveFromDate)]
        [Display(Name = "EffectiveFromDate", ResourceType = typeof(ERP.Translation.General))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public virtual DateTime? EffectiveStartDate
        {
            get
            {
                if (_EffectiveStartDate == null && this.Operation == DataOperation.Create)
                {
                    return DateTime.Now.ApplicationNow().Date;
                }

                else
                {
                    return _EffectiveStartDate;
                }
            }
            set
            {
                _EffectiveStartDate = value;
            }
        }


        //[Required]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [DataType(DataType.Date)]
        [DateRange]
        [DateCompare("EffectiveStartDate")]
        //[Display(Name = Constant.Annotation.Labels.EffectiveToDate)]
        [Display(Name = "EffectiveToDate", ResourceType = typeof(ERP.Translation.General))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public virtual DateTime? EffectiveEndDate
        {
            get
            {
                if (_EffectiveEndDate == null && (this.Operation == DataOperation.Create || this.Operation == DataOperation.Update))
                {
                    return Constant.ApplicationMaxDate;
                }
                else
                {
                    return _EffectiveEndDate;
                }


            }
            set
            {
                _EffectiveEndDate = value;
            }
        }
        public virtual bool IsDatedActive(DateTime? asofDate = null)
        {
            asofDate = asofDate ?? DateTime.Now.Date;
            if (EffectiveStartDate == null || EffectiveEndDate == null)
            {
                return false;
            }
            return EffectiveStartDate <= asofDate && asofDate <= EffectiveEndDate;
        }
    }
}
