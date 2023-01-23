﻿using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ManpowerSummaryComment", Schema = "rec")]
    public class ManpowerSummaryComment : DataModelBase
    {
        [ForeignKey("ManpowerRecruitmentSummary")]
        public string ManpowerRecruitmentSummaryId { get; set; }
        public ManpowerRecruitmentSummary ManpowerRecruitmentSummary { get; set; }
        public string Comment { get; set; }
        [ForeignKey("UserRole")]
        public string UserRoleId { get; set; }
        public UserRole UserRole { get; set; }

    }
}
