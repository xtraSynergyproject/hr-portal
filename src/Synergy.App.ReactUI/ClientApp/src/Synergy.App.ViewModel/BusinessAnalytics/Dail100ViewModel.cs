using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class Dial100ViewModel
    {
        public string caller_name { get; set; }
        public string caller_number { get; set; }
        public string event_number { get; set; }
        public string event_time { get; set; }
        public string event_remark { get; set; }
        public string event_type { get; set; }
        public string event_subType { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string district_Code { get; set; }
        public string police_Station { get; set; }
        public string Frv_Code { get; set; }
        public string dispatch_Time { get; set; }
        public string acknowledge_Time { get; set; }
        public string enroute_Time { get; set; }
        public string arrival_Time { get; set; }
        public string closing_Time { get; set; }
        public string disposition_Code { get; set; }
        public bool? isAlerted { get; set; }
    }
    public class Dial100ArrayViewModel
    {
        public string[] caller_name { get; set; }
        public string[] caller_number { get; set; }
        public string[] event_number { get; set; }
        public string[] event_time { get; set; }
        public string[] event_remark { get; set; }
        public string[] event_type { get; set; }
        public string[] event_subType { get; set; }
        public string[] latitude { get; set; }
        public string[] longitude { get; set; }
        public string[] district_Code { get; set; }
        public string[] police_Station { get; set; }
        public string[] Frv_Code { get; set; }
        public string[] dispatch_Time { get; set; }
        public string[] acknowledge_Time { get; set; }
        public string[] enroute_Time { get; set; }
        public string[] arrival_Time { get; set; }
        public string[] closing_Time { get; set; }
        public string[] disposition_Code { get; set; }
    }
    public class TestDial100ViewModel
    {
        public string caller_name { get; set; }
        public string caller_number { get; set; }
        public string event_number { get; set; }
        public DateTime event_time { get; set; }
        public string event_remark { get; set; }
        public string event_type { get; set; }
        public string event_subType { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string district_Code { get; set; }
        public string police_Station { get; set; }
        public string Frv_Code { get; set; }
        public string dispatch_Time { get; set; }
        public string acknowledge_Time { get; set; }
        public string enroute_Time { get; set; }
        public string arrival_Time { get; set; }
        public string closing_Time { get; set; }
        public string disposition_Code { get; set; }
    }
    public class Dial100ForcastViewModel
    {
        public string json { get; set; }
        public DateTime created_at { get; set; }        
    }
}
