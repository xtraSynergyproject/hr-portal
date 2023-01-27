using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class CapacityRiskViewModel
    {
        public string JobId { get; set; }
        public string JobName { get; set; }
        public long ExternalAvailability { get; set; }
        public long InternalAvailability { get; set; }
        public double Size { get; set; }
        public string DepartmentId { get; set; }
        public string PositionId { get; set; }
        public string JsonData { get; set; }
    }
}
