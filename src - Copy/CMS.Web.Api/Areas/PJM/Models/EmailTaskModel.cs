using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Api.Areas.PJM.Models
{
    public class EmailTaskModel
    {
        public List<MessageEmailViewModel> Data { get; set; }
        public int Total { get; set; }

    }
}
