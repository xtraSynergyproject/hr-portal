using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using static ERP.Utility.Constant;

namespace ERP.UI.ViewModel
{
    public class JobOfferViewModel : ViewModelBase
    {
        public string JobTitle { get; set; }
        public string JobOfferNo { get; set; }
        public string JobOfferStatus { get; set; }
        public string CandidateName { get; set; }
        public string Position { get; set; }        
        public double Salary { get; set; }
        public double BasicSalary { get; set; }
        public double TranspotationAllowance { get; set; }
        public double HousingAllowance { get; set; }
        public double YearlyEntitlement { get; set; }
        public long? TicketOriginCountry { get; set; }
        public string TicketOriginCountryName { get; set; }
        public string Dependent { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime JoinDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime Date { get; set; }
       // public long? JobOfferId { get; set; }
        public long? StepTaskId { get; set; }

    }

}


