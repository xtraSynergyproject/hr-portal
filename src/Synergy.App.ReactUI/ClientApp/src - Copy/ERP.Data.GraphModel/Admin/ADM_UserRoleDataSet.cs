using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.Model
{
    public partial class ADM_UserRoleDataSet : AdminBase
    {
        public ulong UserRoleId { get; set; }
        public virtual ADM_UserRole UserRole { get; set; }
        public ulong DataSetId { get; set; }
        public virtual ADM_DataSet DataSet { get; set; }
    }
}
