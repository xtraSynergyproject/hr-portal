using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class CustomerViewModel:NoteTemplateViewModel
    {
        public string CustomerName { get; set; }
        public string CityName { get; set; }
        public string GstNo { get; set; }
        public string GSTRegistrationType { get; set; }        
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }
        public string BankId { get; set; }
        public string PIN { get; set; }
        public string Address { get; set; }
        public string PANNo { get; set; }
        public string CreditDays { get; set; }       
        public string TANNo { get; set; }
        public string Website { get; set; }
        public string AccountHolderName { get; set; }
        public string FileNo { get; set; }
        public string AccountNo { get; set; }
        public string CustomerCategoryName { get; set; }
        public string CustomerVerticalName { get; set; }
    }
}
