using CMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class ReimbursementRequestViewModel
    {
        public string Id { get; set; }
        public string ReimbursementRequestId { get; set; }

        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public string ItemDate { get; set; }
        public string ItemDescription { get; set; }
        public string Amount { get; set; }
    }
}
