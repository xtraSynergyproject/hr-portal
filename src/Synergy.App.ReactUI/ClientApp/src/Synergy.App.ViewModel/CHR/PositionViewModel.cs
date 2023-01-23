using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
   public class PositionViewModel
    {
        public string Id { get; set; }
        
        public string PositionName { get; set; }
        public string ParentPositionId { get; set; }
        public string JobId { get; set; }
        public string JobName { get; set; }
        public string DepartmentId { get; set; }
        public int PositionNo { get; set; }

       
    }



}
