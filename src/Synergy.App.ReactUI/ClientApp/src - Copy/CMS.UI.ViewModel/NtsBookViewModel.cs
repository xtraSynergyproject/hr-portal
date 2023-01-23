using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class NtsBookViewModel
    {
        public NtsTypeEnum NtsType { get; set; }
        public string Id { get; set; }
        public List<NtsBookItemViewModel> Items { get; set; }

    }
    public class NtsBookItemViewModel
    {
        public NtsTypeEnum NtsType { get; set; }
        public NtsTypeEnum? ParentNtsType { get; set; }
        public string Id { get; set; }
        public string TemplateName { get; set; }
        public string Subject { get; set; }
        public string parentId { get; set; }
        public int Level { get; set; }
        public string ItemNo { get; set; }
        public long SequenceOrder { get; set; }
        public string AssigneeOrOwner { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public DateTime? DueDate { get; set; }
        public string Section { get { return $"{Subject}"; } }
        public ItemTypeEnum ItemType { get; set; }
        public bool AutoLoad { get; set; }
        public string TemplateId { get; set; }
        public string TemplateCode { get; set; }
         public string ReferenceNo { get; set; }
        public bool HasChild { get; set; }
        public long MaxChildSequence { get; set; }

    }
}
