using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class RuleToActionsGridViewModel :  DatedViewModelBase
    {
        public RuleToActionsGridViewModel()
        {
            Operation = DataOperation.Create;
        }  


        [Display(Name = "Action Name")]
        [Editable(false)]
        public string Name { get; set; }


        //[Display(Name = "Module")]
        //public string ModuleName { get; set; }

        //[Display(Name = "Sub Module")]
        //public string SubModuleName { get; set; }

        //[Display(Name = "Screen")]
        //public string ScreenName { get; set; }

        //[Display(Name = "Tab")]
        //public string TabName { get; set; }

        //[Display(Name = "Block")]
        //public string BlockName { get; set; }

        //public int BlockId { get; set; }

        [Display(Name = Constant.Annotation.Labels.SequenceNo)]
        public virtual int? SequenceNo { get; set; }

        [Display(Name = "Include Or Exclude?")]
        public virtual IncludeExclude ActionStatus { get; set; }


        [Display(Name = "Module/Sub Module")]
        [Editable(false)]
        public virtual string Module { get; set; }

        [Display(Name = "Screen/Tab/Block")]
        [Editable(false)]
        public virtual string Screen { get; set; }

    }
}


