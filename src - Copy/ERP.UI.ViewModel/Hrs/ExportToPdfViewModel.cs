using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ExportToPdfViewModel
    {

        public int Width { get; set; }
        public int Height { get; set; }
        public string Content { get; set; }

    }
}
