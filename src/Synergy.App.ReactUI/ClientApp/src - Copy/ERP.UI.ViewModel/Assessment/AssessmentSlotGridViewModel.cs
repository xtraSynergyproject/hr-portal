using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class AssessmentSlotGridViewModel
    {
        public long Id { get; set; }

        public long? ProctorId
        {
            get;
            set;
        }
        
        public string ProctorName
        {
            get;
            set;
        }

    }
}
