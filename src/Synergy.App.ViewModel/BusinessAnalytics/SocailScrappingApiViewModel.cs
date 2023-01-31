using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class SocailScrappingApiViewModel : NoteTemplateViewModel
    {
        public string Url { get; set; }
        public string Parameters { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ApiAuthorization { get; set; }
        public string FilterColumn { get; set; }
        public string ResponseToken { get; set; }
        public string DateFormat { get; set; }
        public string IdColumn { get; set; }
        public int BatchDays { get; set; }
        public string ScheduleTime { get; set; }

    }
    public class SocailScrappingApiParameterViewModel 
    {
        public string ParameterName { get; set; }
        public string DefaultValue { get; set; }
        public int SequenceNo { get; set; }       

    }
}
