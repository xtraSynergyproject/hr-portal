using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;
using ERP.Data.GraphModel;

namespace ERP.UI.ViewModel
{
    public class FormFieldViewModel
    {
        public string label { get; set; }
        public bool tableView { get; set; }
        public string key { get; set; }
        public string type { get; set; }
        public bool input { get; set; }
        public bool disableOnInvalid { get; set; }
        public string id { get; set; }
        public long? TemplateFieldId { get; set; }
        public decimal? SequenceNo { get; set; }

        public string RequiredTypeCode { get; set; }

    }
}
