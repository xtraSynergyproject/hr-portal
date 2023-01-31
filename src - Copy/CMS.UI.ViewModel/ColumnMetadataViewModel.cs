using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class ColumnMetadataViewModel : ColumnMetadata
    {
        [UIHint("ForeignKeyTable")]
        public string ForeignKeyTableId { get; set; }
        public string ForeignKeyTableName { get; set; }
        [UIHint("ForeignKeyColumn")]
        public string ForeignKeyColumnId { get; set; }
        public string ForeignKeyColumnName { get; set; }
        [UIHint("ForeignKeyDisplayColumn")]
        public string ForeignKeyDisplayColumnId { get; set; }
        public string ForeignKeyDisplayColumnName { get; set; }
        [UIHint("DataColumnType")]
        public string DataTypestr { get; set; }
        public object Value { get; set; }
        public bool IsForeignKeyTableColumn { get; set; }
        public string TableName { get; set; }
        public TemplateTypeEnum TemplateType { get; set; }
        public string TableSchemaName { get; set; }
        public string TableAliasName { get; set; }
        public string TableMetadataName { get; set; }
        public bool IsVisible
        {
            get
            {
                return
                (
                    ViewableBy != null && ActiveUserType.HasValue
                    && ViewableBy.Any(x => x == "All" || x == ActiveUserType.ToString())
                )
                &&
                (
                    ViewableContext != null && NtsStatusCode.IsNotNullAndNotEmpty()
                    && ViewableContext.Any(x => x.EndsWith("_ALL") || x == NtsStatusCode)
                );
            }
        }
        public bool IsEditable
        {
            get
            {
                return 
                (
                    EditableBy != null && ActiveUserType.HasValue 
                    && EditableBy.Any(x => x == "All" || x == ActiveUserType.ToString())
                )
                && 
                (
                    EditableContext != null && NtsStatusCode.IsNotNullAndNotEmpty() 
                    && EditableContext.Any(x => x.EndsWith("_ALL") || x == NtsStatusCode)
                );
            }
        }
        public NtsActiveUserTypeEnum? ActiveUserType { get; set; }
        public string NtsStatusCode { get; set; }
        public bool IsChecked { get; set; }

        public bool IgnorePermission { get; set; }
        public string TemplateId { get; set; }
    }

    public class UdfPermissionViewModel : UdfPermission
    {
        public string Name { get; set; }
        public string EditableByDisplay { get; set; }
        public string ViewableByDisplay { get; set; }
        public string EditableContextDisplay { get; set; }
        public string ViewableContextDisplay { get; set; }
        public string TemplateName { get; set; }
        public string ColumnName { get; set; }
        public string HeaderName { get; set; }

        public bool IsVisible
        {
            get
            {
                return
                (
                    ViewableBy != null && ActiveUserType.HasValue
                    && ViewableBy.Any(x => x == "All" || x == ActiveUserType.ToString())
                )
                &&
                (
                    ViewableContext != null && NtsStatusCode.IsNotNullAndNotEmpty()
                    && ViewableContext.Any(x => x.EndsWith("_ALL") || x == NtsStatusCode)
                );
            }
        }
        public bool IsEditable
        {
            get
            {
                return
                (
                    EditableBy != null && ActiveUserType.HasValue
                    && EditableBy.Any(x => x == "All" || x == ActiveUserType.ToString())
                )
                &&
                (
                    EditableContext != null && NtsStatusCode.IsNotNullAndNotEmpty()
                    && EditableContext.Any(x => x.EndsWith("_ALL") || x == NtsStatusCode)
                );
            }
        }
        public NtsActiveUserTypeEnum? ActiveUserType { get; set; }
        public string NtsStatusCode { get; set; }

    }    
}
