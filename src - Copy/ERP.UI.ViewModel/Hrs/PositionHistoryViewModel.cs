using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.ComponentModel;

namespace ERP.UI.ViewModel
{
    public class PositionHistoryViewModel  
    {

        public string TableName { get; set; }
        public string EventName { get; set; }
        public GridSelectOption SelectOption { get; set; }
        public DateTime? EventDate { get; set; }
        public string EventBy { get; set; }
        public int? Id { get; set; }
    }
}
