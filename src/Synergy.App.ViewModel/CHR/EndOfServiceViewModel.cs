﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
   public class EndOfServiceViewModel: ServiceTemplateViewModel
    {

        public DateTime Lastworkingdate { get; set; }
        public DateTime Resignationterminationdate { get; set; }
        public string ReasonId { get; set; }

        public string  Comment { get; set; }
    }
}
