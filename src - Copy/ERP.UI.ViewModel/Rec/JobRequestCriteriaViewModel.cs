using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using static ERP.Utility.Constant;

namespace ERP.UI.ViewModel
{
    public class JobRequestCriteriaViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public double Weightage { get; set; }


    }

}


