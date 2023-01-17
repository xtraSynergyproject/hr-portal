using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsServiceLog : NtsService
    {
        [ForeignKey("NtsService")]
        public string NtsServiceId { get; set; }
        public NtsService NtsService { get; set; }
        public DateTime LogDate { get; set; }
        public string LogByUserId { get; set; }
    }

}
