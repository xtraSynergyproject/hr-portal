using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class WidgetItemViewModel : NoteTemplateViewModel
    {
        public string keyword { get; set; }
        public string socialMediaType { get; set; }
        public string[] socialMediaTypeArray { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string chartMetadata { get; set; }
        public string chartTypeId { get; set; }
        public string chartTypeName { get; set; }
        public string ChartKey { get; set; }
        public string boilerplateCode { get; set; }
    }    

    
}
