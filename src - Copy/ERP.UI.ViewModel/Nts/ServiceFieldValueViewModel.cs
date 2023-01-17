using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;

namespace ERP.UI.ViewModel
{
    public class ServiceFieldValueViewModel : ViewModelBase
    {
       // public long ServiceFieldValueId { get; set; }
      //  public long? ServiceFieldValueVersionId { get; set; }
        public long ServiceId { get; set; }
        public long? ServiceVersionId { get; set; }
        public long TemplateId { get; set; }
        public long TemplateFieldId { get; set; }
        public string TemplateFieldName { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; } 
        public string ServiceSubject { get; set; }
        public string ServiceStatus { get; set; }
        public string ServiceStatusCode { get; set; }

    }
}
