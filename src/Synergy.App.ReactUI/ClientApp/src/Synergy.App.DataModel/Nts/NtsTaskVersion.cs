using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsTaskVersion : NtsTask
    {
        [ForeignKey("NtsTask")]
        public string NtsTaskId { get; set; }
        public NtsTask NtsTask { get; set; }
        public DateTime VersionDate { get; set; }
        public string VersionByUserId { get; set; }

    }

}
