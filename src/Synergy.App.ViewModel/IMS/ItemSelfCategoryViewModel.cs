using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class ItemShelfViewModel : NoteTemplateViewModel
    {
        public string ShelfNo { get; set; }
        public string ShelfLocation { get; set; }

        public string ShelfId { get; set; }
        public string CategoryId { get; set; }
        public string ShelfName { get; set; }
        public string ItemCategoryName { get; set; }

    }
}
