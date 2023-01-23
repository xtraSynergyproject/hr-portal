﻿using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("ApplicationComputerProficiency", Schema = "rec")]
    public class ApplicationComputerProficiency : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }       
        public string Program { get; set; }
        [ForeignKey("ListOfValue")]
        public string ProficiencyLevel { get; set; }
        public bool IsLatest { get; set; }
    }
}
