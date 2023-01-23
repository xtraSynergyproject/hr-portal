using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
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
