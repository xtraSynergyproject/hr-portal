using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.App.DataModel
{


    public class NtsTemplate : DataModelBase
    {
        public long TemplateId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string NtsType { get; set; }
        public string DocumentStatus { get; set; }
    }
}
