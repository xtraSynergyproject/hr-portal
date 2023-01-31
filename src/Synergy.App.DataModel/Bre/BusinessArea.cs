using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{   
    public class BusinessArea : DataModelBase
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
        public DataTypeEnum EnumTest { get; set; }


    }
}
