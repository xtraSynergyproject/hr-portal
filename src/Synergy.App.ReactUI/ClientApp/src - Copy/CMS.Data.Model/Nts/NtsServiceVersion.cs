using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
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
