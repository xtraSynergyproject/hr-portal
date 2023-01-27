using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class DeliveryItemViewModel:NoteTemplateViewModel
    {
        public string DeliveryNoteId { get; set; }
        public string IssuedItemsId { get; set; }
        public long DeliveredQuantity { get; set; }
        public string ItemName { get; set; }
        public string ItemUOM { get; set; }
        public int SNo { get; set; }
    }
}
