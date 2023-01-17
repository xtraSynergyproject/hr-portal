using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Synergy.App.DataModel
{   
    public class DataIntegration : DataModelBase
    {
        public string CollectionName { get; set; }
        public string CollectionDisplayName { get; set; }
        public string CompanyCode { get; set; }
        public string Schema { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
