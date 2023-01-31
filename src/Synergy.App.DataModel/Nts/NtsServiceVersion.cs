using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsServiceVersion : NtsService
    {
        [ForeignKey("NtsService")]
        public string NtsServiceId { get; set; }
        public NtsService NtsService { get; set; }
        public DateTime VersionDate { get; set; }
        public string VersionByUserId { get; set; }
    }

}
