using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NoteResourceLanguage : DataModelBase
    {
        public string TemplateId { get; set; }
        public string NoteTableId { get; set; }
        public string ColumnId { get; set; }
        public string English { get; set; }
        public string Arabic { get; set; }
        public string Spanish { get; set; }
        public string French { get; set; }
        public string Hindi { get; set; }
        public string EnglishHelperText { get; set; }
        public string ArabicHelperText { get; set; }
        public string HindiHelperText { get; set; }
        public string EnglishTooltip { get; set; }
        public string ArabicTooltip { get; set; }
        public string HindiTooltip { get; set; }
    }
    [Table("NoteResourceLanguageLog", Schema = "log")]
    public class NoteResourceLanguageLog : NoteResourceLanguage
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
