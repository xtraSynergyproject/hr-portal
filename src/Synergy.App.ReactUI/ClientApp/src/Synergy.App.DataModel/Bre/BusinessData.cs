﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Synergy.App.DataModel
{   
    public class BusinessData : DataModelBase
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
    }
}
