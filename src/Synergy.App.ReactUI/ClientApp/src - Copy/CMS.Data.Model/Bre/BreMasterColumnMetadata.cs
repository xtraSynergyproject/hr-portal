using CMS.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace CMS.Data.Model
{
    public class BreMasterColumnMetadata : DataModelBase
    {
        public string Name { get; set; }
        public string ColumnName { get; set; }
        public string Alias { get; set; }
        public string ColumnMetadataId { get; set; }
        public DataTypeEnum DataType { get; set; }
        public BreInputDataTypeEnum BreInputDataType { get; set; }
        public string ParentId { get; set; }
        public string BreMasterTableMetadataId { get; set; }

    }
}
    
