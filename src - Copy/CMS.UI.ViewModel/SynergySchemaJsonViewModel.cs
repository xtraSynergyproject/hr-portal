using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
  public class SynergySchemaJsonViewModel
    {
        public Dictionary<string, SqlDataTypes> dimensions { get; set; }
        public Dictionary<string, measurescnt> measures { get; set; }

        public string title { get; set; }
        public string sql { get; set; }

    }

    public class SqlDataTypes
    {
        public string sql { get; set; }
        public int type { get; set; }
    }

    public class measurescnt
    {
        public string type { get; set; }
        
    }


}
