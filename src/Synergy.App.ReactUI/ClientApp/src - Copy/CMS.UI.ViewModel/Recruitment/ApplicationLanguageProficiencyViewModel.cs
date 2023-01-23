using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class ApplicationLanguageProficiencyViewModel : DataModelBase
    {
        public string LanguageName { get; set; }
        public string ProficiencyLevelName { get; set; }
        public string ApplicationId { get; set; }       
        public string Language { get; set; }        
        public string ProficiencyLevel { get; set; }
        public bool IsLatest { get; set; }
    }
}
