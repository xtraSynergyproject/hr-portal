using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class WorksheetViewModel
    {
        public long UserId { get; set; }
        public long LegalEntityId { get; set; }
        public long? Id { get; set; }
        public string Json { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public long? WorkbookReferenceId { get; set; }
        public NtsTypeEnum? WorkbookReferenceType { get; set; }
    }
}
