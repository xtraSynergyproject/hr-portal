using CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class NtsViewModel
    {
        public string Id { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        public string NtsNo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string TemplateCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public string OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public string AssigneeUserId { get; set; }
        public string AssigneeUserName { get; set; }
        public string PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string PriorityCode { get; set; }
        public List<ColumnMetadataViewModel> ColumnList { get; set; }
        public NtsTypeEnum? ParentNtsType { get; set; }
        public string TemplateName { get; set; }
        public string parentId { get; set; }
        public int Level { get; set; }
        public string ItemNo { get; set; }
        public long SequenceOrder { get; set; }
        public string AssigneeOrOwner { get; set; }
        public string Section { get { return $"{Subject}"; } }
        public ItemTypeEnum ItemType { get; set; }
        public bool AutoLoad { get; set; }
        public string TemplateId { get; set; }
        public string ReferenceNo { get; set; }
        public bool HasChild { get; set; }
        public long MaxChildSequence { get; set; }
        public long? UdfCount { get; set; }

        public long Sequence
        {
            get
            {
                if (ItemNo.IsNullOrEmpty())
                {
                    return 0;
                }
                long seq = 0;
                if (long.TryParse(ItemNo.Replace(".", ""), out seq))
                {
                    return seq;
                }
                return 0;
            }
        }
        public bool HideHeader { get; set; }
        public bool HideSubject { get; set; }
        public bool HideDescription { get; set; }
        public long TemplateSequence { get; set; }
    }
}
