using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using CMS.UI.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace CMS.UI.Web.Areas.IMS.Controllers
{
    [Route("ims/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Route("GetDeliveryNoteDetails")]
        public async Task<IActionResult> GetDeliveryNoteDetails(string deliveryNoteId)
        {
            var inventoryManagementBusiness = _serviceProvider.GetService<IInventoryManagementBusiness>();
            try
            {
                var model = await inventoryManagementBusiness.GetDeliveryNoteReportData(deliveryNoteId);
                if (model!=null)
                {
                    var deliveryItems = await inventoryManagementBusiness.GetDeliveryItemsList(deliveryNoteId);
                    if (deliveryItems!=null)
                    {
                        model.DeliveryItems = deliveryItems;
                    }
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetPurchaseOrderDetails")]
        public async Task<IActionResult> GetPurchaseOrderDetails(string purchaseOrderId)
        {
            var inventoryManagementBusiness = _serviceProvider.GetService<IInventoryManagementBusiness>();
            try
            {
                var model = await inventoryManagementBusiness.GetPurchaseOrderReportData(purchaseOrderId);
                if (model!=null)
                {
                    var potc = await inventoryManagementBusiness.ReadPOTermsData(purchaseOrderId);
                    if (potc!=null && potc.Count>0)
                    {
                        model.POTermsAndConditions = potc;
                    }
                    else
                    {
                        var tc = new List<POTermsAndConditionsViewModel>();
                        tc.Add(new POTermsAndConditionsViewModel { TermsTitle = "Delivery", TermsDescription = "With-in 10 days" });
                        tc.Add(new POTermsAndConditionsViewModel { TermsTitle = "Payment", TermsDescription = "With-in 15 days after submission of Invoice" });
                        model.POTermsAndConditions = tc;
                    }
                    var poitems = await inventoryManagementBusiness.ReadPOItemsData(purchaseOrderId);
                    if (poitems!=null)
                    {
                        model.POItems = poitems;
                        var totalbasevalue = poitems.Sum(x => x.TotalAmount);
                        model.TotalBaseValue = totalbasevalue;
                        model.NetPayable = model.TotalBaseValue + model.TotalSGST + model.TotalCGST + model.TotalIGST;
                        string amtInWords = string.Concat(Humanizer.NumberToWordsExtension.ToWords(Convert.ToInt64(model.NetPayable)), " ", " Only");
                        model.NetPayableInWords = amtInWords.ToUpper();
                    }
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetPurchaseInvoiceDetails")]
        public async Task<IActionResult> GetPurchaseInvoiceDetails(string purchaseInvoiceId)
        {
            var inventoryManagementBusiness = _serviceProvider.GetService<IInventoryManagementBusiness>();
            try
            {
                var model = await inventoryManagementBusiness.GetPurchaseInvoiceReportData(purchaseInvoiceId);
                if (model != null)
                {
                    var invoiceItems = await inventoryManagementBusiness.GetPurchaseInvoiceItemsList(purchaseInvoiceId);
                    if (invoiceItems!=null)
                    {
                        model.PurchaseInvoiceItems = invoiceItems;
                        var totalAmount = invoiceItems.Sum(x => x.TotalAmount);
                        model.InvoiceAmount = totalAmount;
                    }
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetReceivedNoteDetails")]
        public async Task<IActionResult> GetReceivedNoteDetails(string receivedNoteId)
        {
            var inventoryManagementBusiness = _serviceProvider.GetService<IInventoryManagementBusiness>();
            try
            {
                var model = await inventoryManagementBusiness.GetReceivedNoteReportData(receivedNoteId);
                if (model!=null)
                {
                    var receivedItems = await inventoryManagementBusiness.GetGoodReceiptItemsList(receivedNoteId);
                    model.GoodsReceiptItems = receivedItems;
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
