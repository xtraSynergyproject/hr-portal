using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class StockAdjustmentViewModel:ServiceTemplateViewModel
    {
       public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public DateTime? AdjustmentDate { get; set; }
        public string Comment { get; set; }
        public StockAdjustmentTypeEnum StockAdjustmentType { get; set; }
    }
    public class StockAdjustmentItemViewModel : NoteTemplateViewModel
    {
        public string StockAdjustmentId { get; set; }
        public string AdjustmentQuantity { get; set; }
        public string ItemCategory { get; set; }
        public string ItemSubCategory { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string AdjustmentReason { get; set; }
        public string ServiceStatusCode { get; set; }
        
    }
}
