using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ApplicationLanguageProficiency", Schema = "rec")]
    public class ApplicationLanguageProficiency : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        [ForeignKey("ListOfValue")]
        public string Language { get; set; }
        [ForeignKey("ListOfValue")]
        public string ProficiencyLevel { get; set; }
        public bool IsLatest { get; set; }
    }
}
