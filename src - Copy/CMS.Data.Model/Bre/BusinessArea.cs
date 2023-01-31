using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{   
    public class BusinessArea : DataModelBase
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
        public DataTypeEnum EnumTest { get; set; }


    }
}
