using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Cms.Models
{
    public class WorkboardDublicateModel
    {
        public string WorkBoardName { get; set; }
        public bool IsUserData { get; set; }
        public bool IsCharts { get; set; }
        public bool IsComments { get; set; }
        public bool IsTasks { get; set; }

        public string WorkBoardId { get; set; }

        public string UserId { get; set; }

        public string PortalName { get; set; }

        public string PortalId { get; set; }

    }
}
