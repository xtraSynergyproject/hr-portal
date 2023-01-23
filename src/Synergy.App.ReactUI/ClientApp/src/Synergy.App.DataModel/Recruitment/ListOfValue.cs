using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ListOfValue", Schema = "rec")]
    public class ListOfValue : DataModelBase
    {
        public string ListOfValueType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? EnableDescription { get; set; }
        public string Code { get; set; }
        [ForeignKey("ListOfValue")]
        public string ParentId { get; set; }
        public ListOfValue Parent { get; set; }
        public string ImageId { get; set; }
        public string IconCss { get; set; }

        public ReferenceTypeEnum? ReferenceTypeCode { get; set; }
        public string ReferenceTypeId { get; set; }
    }
}
