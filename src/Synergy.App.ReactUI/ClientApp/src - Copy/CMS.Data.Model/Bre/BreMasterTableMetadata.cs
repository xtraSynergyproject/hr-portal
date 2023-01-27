using CMS.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace CMS.Data.Model
{

    public class BreMasterTableMetadata : DataModelBase
    {
        public string BusinessRuleId { get; set; }
        public string TableMetadataId { get; set; }
        public string ParentId { get; set; }
        public BreInputDataTypeEnum BreInputDataType { get; set; }
        public DataTypeEnum DataType { get; set; }
        public string OperationValue { get; set; }

    }
}
