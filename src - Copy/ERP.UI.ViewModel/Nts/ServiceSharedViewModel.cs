using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;

namespace ERP.UI.ViewModel
{
    public class ServiceSharedViewModel : ViewModelBase
    {
        public long ServiceSharedId { get; set; }
        public long ServiceId { get; set; }
        [Display(Name = "SharedType", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public AssignToTypeEnum? SharedType { get; set; }
        public long? SharedTo { get; set; }
        [Display(Name = "SharedToUserName", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string SharedToUserName { get; set; }
        public string SharedTypeDisplay { get {
                return SharedType?.ToString(); } }
        public long? TeamId { get; set; }

    }
}
