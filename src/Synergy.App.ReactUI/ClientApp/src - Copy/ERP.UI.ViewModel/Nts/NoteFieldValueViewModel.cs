using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;

namespace ERP.UI.ViewModel
{
    public class NoteFieldValueViewModel : ViewModelBase
    {
        public long NoteFieldValueId { get; set; }
        public long? NoteFieldValueVersionId { get; set; }
        public long NoteId { get; set; }
        public long? NoteVersionId { get; set; }
        public long TemplateId { get; set; }
        public long TemplateFieldId { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public long? RowId { get; set; }
        public string FieldPartialViewName { get; set; }
        public string TemplateFieldName { get; set; }
    }
}
