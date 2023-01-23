﻿using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class CandidateComputerProficiencyViewModel : DataModelBase
    {
        public string ProficiencyLevelName { get; set; }       
        public string CandidateProfileId { get; set; }
        public string Program { get; set; }
        public string ProficiencyLevel { get; set; }
        public bool IsLatest { get; set; }
    }
}