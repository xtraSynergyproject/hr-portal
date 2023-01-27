using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
  public class SynergySchemaViewModel :ViewModelBase
    {        
        public string NoteId { get; set; }
        public string SchemaName { get; set; }
        public string Query { get; set; }
        public string Metadata { get; set; }        
        public string RefreshKey { get; set; }        
        public bool ElsasticDB { get; set; }        
    }
}
