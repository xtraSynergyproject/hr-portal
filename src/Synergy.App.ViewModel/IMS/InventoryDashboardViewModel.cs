using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class InventoryDashboardViewModel
    {
        public int TotalTobePackedItem { get; set; }
        public int TotalTobeShippedItem { get; set; }
        public int TotalTobeDeliveredItem { get; set; }
        public int TotalTobeInvoicedItem { get; set; }
        public double TotalQtyInHand { get; set; }
        public double TotalQtyTobeRecieved { get; set; }
        public double TotalLowStockItems { get; set; }
        public double TotalAllItemGroupItems { get; set; }
        public double TotalAllItem { get; set; }
        public double ItemsInHand { get; set; }
        public double ItemsToReceive { get; set; }        
        public List<string> ActiveItemLabels { get; set; }
        public List<int> ActionItemSeries { get; set; }
        public List<string> TopSellingLabels { get; set; }
        public List<int> TopLabelSeries { get; set; }
        public List<SalesOrder> SalesOrderSeries { get; set; }
        public double QtyOrdered { get; set; }
        public string TotalCost { get; set; }
        public string DirectSales { get; set; }
        public List<string> Categories { get; set; }
        public List<string> ItemValueLabel { get; set; }
        public List<int> ItemValueSeries { get; set; }
        public string Currency { get; set; }

    }

    public class DashboardSalesOrderViewModel
    {
        public string Channel { get; set; }
        public string Draft { get; set; }
        public string Confirmed { get; set; }
        public string Packed { get; set; }
        public string Shipped { get; set; }
        public string Invoiced { get; set; }

    }

    public class SalesOrder
    {
        public int RangeNumber { get; set; }
        public string Range { get; set; }
        public string ItemName { get; set; }
        public string Id { get; set; }
        public string ItemValue { get; set; }
        public string Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Month { get; set; }
        public string DayName { get; set; }
        public string Day { get; set; }
        public string Year { get; set; }
        public string QUARTER { get; set; }
        public string name { get; set; }
        public List<int> data { get; set; }
        public List<string> categories { get; set; }
    }
}
