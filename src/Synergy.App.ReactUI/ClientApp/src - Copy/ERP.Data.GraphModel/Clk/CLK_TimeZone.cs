using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class CLK_TimeZone : NodeBase
    {
        public string Name { get; set; }
        public string TimeZoneName { get; set; }
        public TimeSpan UtcOffSet { get; set; }
    }
}
