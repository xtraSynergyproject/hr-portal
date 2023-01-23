using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class TASUserReportViewModel :TASUserReport
    {
        public string FullNameWithJobTitle
        {
            get
            {
                return string.Concat(UserFullName, ", ", UserJobTitle, ", ", MinistryName);
            }
        }

        public long? SubordinateReport
        {
            get
            {
                return IsSubordinateReport == "Yes" ? 1 : 0;
            }
        }


    }
}
