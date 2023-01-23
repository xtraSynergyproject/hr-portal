using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("RecruitmentPayElement", Schema = "rec")]
    public class RecruitmentPayElement : DataModelBase
    {
        public ElementValueTypeEnum? ValueType { get; set; }
        public string ElementCode { get; set; }
        public string ElementName { get; set; }
        public string DisplayName { get; set; }
    }
}
