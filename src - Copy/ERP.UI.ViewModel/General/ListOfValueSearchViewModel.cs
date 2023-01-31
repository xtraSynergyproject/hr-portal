using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ListOfValueSearchViewModel : ViewModelBase
    {
        public string LOVTypeCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string Description { get; set; }
        public string ParentCode { get; set; }
        public long? SequenceNo { get; set; }
    }
}
