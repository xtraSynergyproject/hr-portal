using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;

namespace ERP.UI.ViewModel
{
    public class TaskFieldValueViewModel : ViewModelBase
    {
        /// <summary>
        /// Used in Versioning only
        /// </summary>
        public long TaskFieldValueId { get; set; }
        public long? TaskFieldValueVersionId { get; set; }
        public long TaskId { get; set; }
        public long? TaskVersionId { get; set; }
        public long TemplateId { get; set; }
        public long TemplateFieldId { get; set; }
        public string Code { get; set; }
        public string Value { get; set; } 
        public string Description { get; set; }
        public long RowId { get; set; }
        public string FieldName { get; set; }

    }
}
