
using System.Collections.Generic;


namespace ERP.UI.ViewModel
{
    public class ModuleViewModel : ViewModelBase
    {

        public string Name { get; set; }
        public string Code { get; set; }
        //public long ModuleId { get; set; } 
        // public long? SequenceNo { get; set; }
        public ICollection<SubModuleViewModel> SubModule { get; set; }
      
        public bool Checked { get; set; }
        
        public long SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public long ScreenId { get; set; }
        public string ScreenName { get; set; }
        public long? ActionId { get; set; }
        public string ActionName { get; set; }
        public long? FieldId { get; set; }
        public string FieldName { get; set; }
        public bool IsEditable { get; set; }
        public bool IsVisible { get; set; }

    }
}


