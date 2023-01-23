using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class ApplicationJobCriteriaViewModel : DataModelBase
    {
        public string ApplicationId { get; set; }
        public string Criteria { get; set; }        
        public string CriteriaType { get; set; }
        public string CriteriaTypeCode { get; set; }
        public string Type { get; set; }
        public int? Weightage { get; set; }
        public string Value { get; set; }
        public string CriteriaValue { get; set; }
        public string ListOfValueType { get; set; }
        public string ListOfValueTypeId { get; set; }
        public string Description { get; set; }
        public bool? EnableDescription { get; set; }
    }
}
