using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
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
