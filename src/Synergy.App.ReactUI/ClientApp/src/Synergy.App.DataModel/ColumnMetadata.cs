using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class ColumnMetadata : DataModelBase
    {
        public string Name { get; set; }
        public bool IsDefaultDisplayColumn { get; set; }
        public string LabelName { get; set; }
        public string Alias { get; set; }
        public bool IsNullable { get; set; }
        public DataColumnTypeEnum DataType { get; set; }
        public UdfUITypeEnum UdfUIType { get; set; }
        public bool IsForeignKey { get; set; }

        public bool IsVirtualColumn { get; set; }
        public bool IsVirtualForeignKey { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsSystemColumn { get; set; }
        public bool IsUniqueColumn { get; set; }
        public bool IsLogColumn { get; set; }
        public bool IsMultiValueColumn { get; set; }
        public bool IsUdfColumn { get; set; }
        public bool IsHiddenColumn { get; set; }
        public bool HideForeignKeyTableColumns { get; set; }
        public bool IsReferenceColumn { get; set; }
        public string ReferenceTableName { get; set; }
        public string ReferenceTableSchemaName { get; set; }

        [ForeignKey("ForeignKeyTable")]
        public string ForeignKeyTableId { get; set; }
        public TableMetadata ForeignKeyTable { get; set; }
        public string ForeignKeyTableName { get; set; }
        public string ForeignKeyTableAliasName { get; set; }
        public string ForeignKeyTableSchemaName { get; set; }

        //public bool IsForeignKeyDisplayColumn { get; set; }

        public string ForeignKeyDisplayColumnReferenceId { get; set; }

        [ForeignKey("ForeignKeyColumn")]
        public string ForeignKeyColumnId { get; set; }
        public string ForeignKeyColumnName { get; set; }
        public ColumnMetadata ForeignKeyColumn { get; set; }


        [ForeignKey("ForeignKeyDisplayColumn")]
        public string ForeignKeyDisplayColumnId { get; set; }
        public ColumnMetadata ForeignKeyDisplayColumn { get; set; }


        public string ForeignKeyDisplayColumnName { get; set; }
        public string ForeignKeyDisplayColumnLabelName { get; set; }
        public string ForeignKeyDisplayColumnAlias { get; set; }
        public DataColumnTypeEnum ForeignKeyDisplayColumnDataType { get; set; }


        public string ForeignKeyConstraintName { get; set; }

        [ForeignKey("TableMetadata")]
        public string TableMetadataId { get; set; }
        public TableMetadata TableMetadata { get; set; }

        public string[] EditableBy { get; set; }
        public string[] ViewableBy { get; set; }
        public string[] EditableContext { get; set; }
        public string[] ViewableContext { get; set; }
        public bool ShowInForeignKeyReference { get; set; }
        public bool EnableLocalization { get; set; }
        public bool EnableLanguageValidation { get; set; }
        public bool ShowInBusinessLogic { get; set; }
        public bool DisableForeignKey { get; set; }
        public bool DontCreateTableColumn { get; set; }

    }
    [Table("ColumnMetadataLog", Schema = "log")]
    public class ColumnMetadataLog : ColumnMetadata
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
        public DateTime LogEndDateTime { get; set; }
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }
    }
}
