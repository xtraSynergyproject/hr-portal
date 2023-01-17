using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class NtsLogViewModel
    {
        public string Id { get; set; }
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
        public DateTime LogEndDateTime { get; set; }
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }
        public string Subject { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        public long VersionNo { get; set; }
        public string TemplateCode { get; set; }
        public List<string> ColumnName { get; set; }
    }
}
