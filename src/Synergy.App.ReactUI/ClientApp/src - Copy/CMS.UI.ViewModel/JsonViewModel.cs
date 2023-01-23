using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class JsonViewModel
    {
        public string label { get; set; }
        public dynamic widget { get; set; }
        public bool tableView { get; set; }
        public string parameterCode { get; set; }
        public DataViewModel data { get; set; }
        public string valueProperty { get; set; }
        public string idPath { get; set; }
        public string template { get; set; }
        public double selectThreshold { get; set; }
        public string ntsType { get; set; }
        public string[] editableContext { get; set; }
        public string[] viewableContext { get; set; }
        public string[] viewableBy { get; set; }
        public string[] editableBy { get; set; }
        public ValidateViewModel validate { get; set; }
        public string key { get; set; }
        public string type { get; set; }
        public dynamic indexeddb { get; set; }
        public string allTable { get; set; }
        public bool input { get; set; }
        public bool isDependantComponent { get; set; }
        public bool disableLimit { get; set; }
        public bool hideOnChildrenHidden { get; set; }
        public string columnMetadataId { get; set; }
        public string udfValue { get; set; }
        public string storage { get; set; }
        public string url { get; set; }
        public string webcam { get; set; }
        public dynamic fileTypes { get; set; }
        public bool enableMinDateInput { get; set; }
        public dynamic datePicker { get; set; }
        public bool enableMaxDateInput { get; set; }
    }

    public class DataViewModel
    {
        public dynamic values { get; set; }
        public string url { get; set; }
        public dynamic headers { get; set; }

    }
    public class ValidateViewModel
    {
        public bool required { get; set; }
        public bool onlyAvailableItems { get; set; }

    }
}
