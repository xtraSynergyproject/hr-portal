using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;

namespace ERP.UI.ViewModel
{
    public class TaskSharedViewModel : ViewModelBase
    {
        public long TaskSharedId { get; set; }
        public long TaskId { get; set; }
        [Display(Name = "SharedType", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public AssignToTypeEnum? SharedType { get; set; }
        public long? SharedTo { get; set; }
        [Display(Name = "SharedToUserName", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string SharedToUserName { get; set; }
        public long? TeamId { get; set; }
        public string SharedTypeDisplay
        {
            get
            {
                return SharedType?.ToString();
            }
        }

        public string ShareUserFirstLetter
        {
            get { return (SharedToUserName != null && SharedToUserName != "") ? SharedToUserName.First().ToString() : ""; }
        }

        public string SharedEmail { get; set; }
    }
}
