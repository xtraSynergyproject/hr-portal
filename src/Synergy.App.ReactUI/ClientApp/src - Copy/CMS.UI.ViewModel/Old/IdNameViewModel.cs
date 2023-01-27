using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class IdNameViewModel : DataModelBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public long Count { get; set; }
        public int EnumId { get; set; }
        public DateTime? DateTest { get; set; }
        public DataTypeEnum? DataType  { get; set; }

        public string title { get; set; }
        public string key { get; set; }
        public bool lazy { get; set; }
        public bool selected { get; set; }
        public string DepartmentOwnerUserId { get; set; }
        public bool HasChildren { get; set; }
    }
}
