using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.UI.ViewModel
{
    public class BreMasterMetadataViewModel : BreMasterTableMetadata
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ParentName { get; set; }
        public int CId { get; set; }
        public int? PId { get; set; }
        public bool Expanded { get; set; }
        public bool HasSubFolders { get; set; }
        public string IconCss
        {
            get
            {
                switch (DataType)
                {
                    case DataTypeEnum.String:
                        return "fa fas fa-text";
                    case DataTypeEnum.Bool:
                        return "fa fas fa-check-square";
                    case DataTypeEnum.DateTime:
                        return "fa fas fa-calendar-alt";
                    case DataTypeEnum.Long:
                        return "fa fas fa-sort-numeric-up-alt";
                    case DataTypeEnum.Double:
                        return "fa fas fa-badge-percent";
                    case DataTypeEnum.Object:
                        return "fa fas fa-database";
                    default:
                        return "fa fas fa-database";
                }
            }
        }
        public DataActionEnum DataAction { get; set; }
        public DataColumnTypeEnum? FVDataType { get; set; }

    }
}
