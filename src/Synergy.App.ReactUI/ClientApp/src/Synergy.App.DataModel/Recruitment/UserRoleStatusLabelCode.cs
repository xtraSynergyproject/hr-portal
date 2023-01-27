using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("UserRoleStatusLabelCode", Schema = "public")]
    public class UserRoleStatusLabelCode : DataModelBase
    {
        public string StatusCode { get; set; }

        [ForeignKey("UserRoleStageChild")]
        public string StatusLabelId { get; set; }
        public UserRoleStageChild StatusLabel { get; set; }
        public String Sequenece { get; set; }
    }
}
