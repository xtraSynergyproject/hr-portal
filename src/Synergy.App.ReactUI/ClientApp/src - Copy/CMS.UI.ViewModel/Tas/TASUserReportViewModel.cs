using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
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
