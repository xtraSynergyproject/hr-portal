using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class IdNameViewModel : DataModelBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public long Count { get; set; }
        public int EnumId { get; set; }
        public DateTime? DateTest { get; set; }
        public DataTypeEnum? DataType  { get; set; }

        public string ClassName { get; set; }
        public string title { get; set; }
        public string key { get; set; }
        public bool lazy { get; set; }
        public bool selected { get; set; }
        public string DepartmentOwnerUserId { get; set; }
        public bool HasChildren { get; set; }
    }
}
