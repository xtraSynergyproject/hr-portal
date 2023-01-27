using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class EmployeeJobStatusViewModel : ViewModelBase
    {

       

        public int JanActive { get; set; }
        public int FebActive { get; set; }
        public int MarActive { get; set; }
        public int AprActive { get; set; }
        public int MayActive { get; set; }
        public int JuneActive { get; set; }
        public int JulyActive { get; set; }
        public int AugActive { get; set; }
        public int SepActive { get; set; }
        public int OctActive { get; set; }
        public int NovActive { get; set; }
        public int DecActive { get; set; }
        public int JanInActive { get; set; }
        public int FebInActive { get; set; }
        public int MarInActive { get; set; }
        public int AprInActive { get; set; }
        public int MayInActive { get; set; }
        public int JuneInActive { get; set; }
        public int JulyInActive { get; set; }
        public int AugInActive { get; set; }
        public int SepInActive { get; set; }
        public int OctInActive { get; set; }
        public int NovInActive { get; set; }
        public int DecInActive { get; set; }
        public string Category { get; set; }

    }
}
