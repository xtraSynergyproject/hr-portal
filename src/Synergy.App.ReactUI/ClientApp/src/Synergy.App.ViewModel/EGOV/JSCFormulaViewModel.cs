using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCFormulaViewModel: ViewModelBase
    {        
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public string Formula { get; set; }
        public string FormulaText { get; set; }
        public string FormulaType { get; set; }
        public string FormulaTypeCode { get; set; }
        public string NtsNoteId { get; set; }
        public string FormulaDisplay 
        {  
            get
            {
                var str = FormulaText.Replace("|", "");
                //str = str.Replace("~", "");
                return str;
            }
        }
        public List<IdNameViewModel> CustomList { get; set; }
        public string[][] ItemArray { get; set; }
    }
}
