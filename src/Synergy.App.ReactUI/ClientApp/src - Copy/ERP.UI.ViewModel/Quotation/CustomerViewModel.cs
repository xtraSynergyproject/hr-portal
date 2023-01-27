using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{


    public class CustomerViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string NameAr { get; set; }

        public CustomerTypeEnum? CustomerType { get; set; }
        public SupplierTypeEnum? SupplierType { get; set; }
        public string UserCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public string Discount { get; set; }
        public string Location { get; set; }
        public long? LocationId { get; set; }
        public DateTime? Date { get; set; }
        public string Website { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }
        public string TemplateMasterCode { get; set; }
        public long? CurrencyId { get; set; }
        public string Currency { get; set; }
        public long? UserId { get; set; }
        public string VatNumber { get; set; }
        public string POBoxNo { get; set; }
    }
    public class ContactViewModel : CustomerViewModel
    {
        public string Email { get; set; }
        public string Designation { get; set; }
        public string ContactName { get; set; }
        public string ContactNameAr { get; set; }
        public long? CustomerId { get; set; }
        public string Mobile { get; set; }

    }

}
