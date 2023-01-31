using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class QuarterlyReportFrontPageViewModel : QuarterlyReportBaseViewModel
    {
        public QuarterlyReportFrontPageViewModel()
        {
        }
        public QuarterlyReportFrontPageViewModel(QuarterlyReportBaseViewModel baseModel)
        {
            this.MapValue(baseModel);
        }
        public string MainHeading { get; set; }
        public string SubHeading { get; set; }
 

    }

}
