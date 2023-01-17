using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace CMS.Data.Model
{   
    public class BusinessData : DataModelBase
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
    }
}
