using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class EmailCalendarViewModel : EmailViewModel
    {
 
        public string Location { set; get; }     
        public long? SlotId { set; get; }


    }
}
