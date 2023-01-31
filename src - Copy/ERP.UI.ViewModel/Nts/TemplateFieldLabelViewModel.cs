using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class TemplateFieldLabelViewModel : ViewModelBase
    {
        public long TemplateFieldId { get; set; }

        public string LabelDisplayName_English { get; set; }
        public string LabelDisplayName_Arabic { get; set; }

        public string AdditionalInfo_English { get; set; }
        public string AdditionalInfo_Arabic { get; set; }

        public string HelpInfo_English { get; set; }
        public string HelpInfo_Arabic { get; set; }


        public string PoupTitle_English { get; set; }
        public string PoupTitle_Arabic { get; set; }

        public string Tooltip_English { get; set; }
        public string Tooltip_Arabic { get; set; }
    }
}
