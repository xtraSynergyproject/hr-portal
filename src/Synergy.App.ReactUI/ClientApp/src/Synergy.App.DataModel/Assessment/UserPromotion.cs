using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class UserPromotion : DataModelBase
    {
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string Promotion2017 { get; set; }
        public string Promotion2018 { get; set; }
        public string Promotion2019 { get; set; }

    }
}
