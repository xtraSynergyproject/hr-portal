using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class FormFieldViewModel
    {
        public string label { get; set; }
        public bool tableView { get; set; }
        public string key { get; set; }
        public string type { get; set; }
        public bool input { get; set; }
        public bool unique { get; set; }
        public bool disableOnInvalid { get; set; }
        public string id { get; set; }
        public string columnMetadataId { get; set; }
        public decimal? SequenceNo { get; set; }
        public string[] EditableBy { get; set; }
        public string[] ViewableBy { get; set; }
        public string[] EditableContext { get; set; }
        public string[] ViewableContext { get; set; }
        public string allTable { get; set; }
        public string mapId { get; set; }
        public string mapValue { get; set; }
        public string loadTable { get; set; }
        public object value { get; set; }
        public bool multiple { get; set; }
    }
}
