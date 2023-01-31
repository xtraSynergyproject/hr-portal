using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{

    public class MeterReadingViewModel
    {
        public string ConsumerNo { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? CurrentReadingDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? LastReadingDate { get; set; }
        public long? CurrentReadingKL { get; set; }
        public long? LastReadingKL { get; set; }
        public string MeterStatusId { get; set; }
        public long? ConsumptionKL { get; set; }  
       
    }
}
