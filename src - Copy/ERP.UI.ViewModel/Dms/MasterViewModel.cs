using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class MasterViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string DataSourceActionName { get; set; }
        public string DataSourceAreaName { get; set; }
        public string DataSourceControllerName { get; set; }
        public string DataTextField { get; set; }
        public string DataValueField { get; set; }
        public string DataSourceParameters { get; set; }
        public string NavigationUrl { get; set; }
        public string Description { get; set; }
        public NodeEnum RefeneceNode { get; set; }
        public ReferenceTypeEnum RefeneceTypeNode { get; set; }
        public string MasterSource { get; set; }
    }
}
