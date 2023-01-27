using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DashboardTaskGridSearchViewModel
    {
        public long UserId { get; set; }
        public long FilterbyGroup { get; set; }

        public NtsPriorityEnum? Priority { get; set; }
        public ICollection<NtsPriorityEnum?> Priorities { get; set; }

        public string StatusCode { get; set; }
        public ICollection<string> Status { get; set; }

        public ModuleEnum? Module { get; set; }
        public ICollection<ModuleEnum?> Modules { get; set; }

        public ICollection<long> Templates { get; set; }
        public ICollection<long> Users { get; set; }
        public ICollection<long> TaskRequesters { get; set; }
        public long? TemplateId { get; set; }
        public long? ServiceId { get; set; }
        public long? ServiceStepServiceId { get; set; }
        public ICollection<string> Priorityfilter { get; set; }
        public ICollection<string> TaskRequestersfilter { get; set; }
        public ICollection<string> Usersfilter { get; set; }
        public ICollection<string> Templatesfilter { get; set; }
        public DmsDocumentViewTypeEnum DocumentViewType { get; set; }
        public long? ParentId { get; set; }
        public NtsUserTypeEnum? TemplateUserType { get; set; }
    }
}
