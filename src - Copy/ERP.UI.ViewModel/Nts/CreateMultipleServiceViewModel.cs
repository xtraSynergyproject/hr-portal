using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class CreateMultipleServiceViewModel
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string TemplateIds { get; set; }
        public long? PersonId { get; set; }
        public long? DependentId { get; set; }
    }
}
