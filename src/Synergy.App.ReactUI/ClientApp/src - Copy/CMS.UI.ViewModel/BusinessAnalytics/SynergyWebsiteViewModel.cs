using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
  public class SynergyWebsiteViewModel : ViewModelBase
    {
        public string NoteId { get; set; }        
        public string Name { get; set; }        
        public string HtmlContent { get; set; }        
        public string Style { get; set; }        
        public string Html { get; set; }        
        public string Css { get; set; }        
        public bool IsTemplate { get; set; }        
    }
}
