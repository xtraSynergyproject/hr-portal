using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsTaskLog : NtsTask
    {
        [ForeignKey("NtsTask")]
        public string NtsTaskId { get; set; }
        public NtsTask NtsTask { get; set; }
        public DateTime LogDate { get; set; }
        public string LogByUserId { get; set; }
    }

}
