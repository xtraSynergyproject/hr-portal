using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.Reflection;

namespace ERP.UI.ViewModel
{
    public class LeadViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public long LeadPersonId { get; set; }
        [Display(Name = "Assign To")]
        public long AssignPersonId { get; set; }
        [Display(Name = "Source Of Lead")]
        public SalSourceOfLeadEnum SourceOfLead { get; set; }
        //[Required]
        //public string Location { get; set; }
        [Display(Name = "Lead Status")]
        [Required]
        public SalLeadStatusEnum LeadStatus { get; set; }
        public string Reason { get; set; }


        public long LeadNewCount { get; set; }
        public long LeadActiveCount { get; set; }
        public long LeadCloseDealCount { get; set; }
        public long LeadNotIntrestedCount { get; set; }
        public long PCCount { get; set; }
        public long BrokerCount { get; set; }
        [Display(Name = "Campaign Name")]
        public string CampaignName { get; set; }
        [Display(Name = "Broker")]
        public long? BrokerId { get; set; }
        public string Broker { get; set; }
        [Display(Name = "Property Consultant")]
        public string PropertyConsultantName { get; set; }
        [Display(Name = "Country")]
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public long CountryCodeId { get; set; }
        public string CountryCodeName { get; set; }
        public bool? IsBroker { get; set; }
        public string Remarks { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Creation Date")]
        public DateTime? LeadCreationDate { get; set; }
        public string BrokerAllowed { get; set; }
        [Display(Name = "Project")]
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        [Display(Name = "Unit")]
        public long UnitId { get; set; }
        public string UnitName { get; set; }
    }

    public class KanbanDasnboardData : ViewModelBase
    {
        public string name { get; set; }
       public SalLeadStatusEnum listID { get; set; }
    }
}

