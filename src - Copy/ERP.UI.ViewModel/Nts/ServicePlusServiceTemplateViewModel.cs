using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class ServicePlusServiceTemplateViewModel : ViewModelBase
    {
        public string Subject { get; set; }
        public string Description { get; set; }

        public long ServiceTemplateMasterId { get; set; }
        [Display(Name = "ServiceTemplateMasterName", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string ServiceTemplateMasterName { get; set; }

        public long? ServicePlusTemplateId { get; set; }


        public long ServiceTemplateId { get; set; }
        [Display(Name = "TriggeredByServicePlusTemplateId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long? TriggeredByServicePlusTemplateId { get; set; }
        public string TriggeredByServicePlusTemplateName { get; set; }

        public string TriggeredByScript { get; set; }
        public RatingTypeEnum RatingType { get; set; }



        [Display(Name = "ServicePlusServiceType", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public NtsServicePlusServiceTypeEnum? ServicePlusServiceType { get; set; }
        [Display(Name = "HideIfDraft", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public bool HideIfDraft { get; set; }
        public decimal? SequenceNo { get; set; }



        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "DueDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "SLA", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public TimeSpan? SLA { get; set; }

        public SLACalculationMode? SLACalculationMode { get; set; }


        [Display(Name = "ServiceStartDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ServiceStartDate { get; set; }

        [Display(Name = "ServiceDueDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ServiceDueDate { get; set; }

        [Display(Name = "ServiceSLA", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public TimeSpan? ServiceSLA { get; set; }

        public int? SLADay { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? SLAHour { get; set; }

        public SLACalculationMode? ServiceSLACalculationMode { get; set; }

        [Display(Name = "ServiceOwnerId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long? ServiceOwnerId { get; set; }

        [Display(Name = "ServiceOwnerType", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public AssignToTypeEnum? ServiceOwnerType { get; set; }

        [Display(Name = "ServiceOwnerQueryType", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public AssignedQueryTypeEnum? ServiceOwnerQueryType { get; set; }

        public string ServiceOwnerQuery { get; set; }

        [Display(Name = "ServiceTeamId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long? ServiceTeamId { get; set; }

        public long? ServiceOwnerUserId { get; set; }



        public long? OwnerId { get; set; }
        public AssignToTypeEnum? OwnerType { get; set; }
        public AssignedQueryTypeEnum? OwnerQueryType { get; set; }
        public string OwnerQuery { get; set; }
        public long? TeamId { get; set; }
        public long? OwnerUserId { get; set; }
        public string ServiceTemplateMasterCode { get; set; }





    }
}
