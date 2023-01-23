﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class CandidateComputerProficiencyViewModel : DataModelBase
    {
        public string ProficiencyLevelName { get; set; }       
        public string CandidateId { get; set; }
        public string CandidateProfileId { get; set; }
        public string Program { get; set; }
        public string ProficiencyLevelId { get; set; }
        public bool IsLatest { get; set; }
        public string NoteId { get; set; }
        public string Json { get; set; }
    }
}