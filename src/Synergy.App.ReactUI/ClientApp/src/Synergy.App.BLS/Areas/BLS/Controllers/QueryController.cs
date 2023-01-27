using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using CMS.UI.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.BLS.Areas.BLS.Controllers
{
    [Route("BLS/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileBusiness _fileBusiness;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider
            , IFileBusiness fileBusiness)
        {
            _serviceProvider = serviceProvider;
            _fileBusiness = fileBusiness;
        }

        [HttpGet]
        [Route("GetAppointmentReceipt/{serviceId}/{userId}")]
        public async Task<IActionResult> GetAppointmentReceipt(string serviceId, string userId)
        {
            try
            {
                var _blsBusiness = _serviceProvider.GetService<IBLSBusiness>();
                var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var _applicationDocumentBusiness = _serviceProvider.GetService<IApplicationDocumentBusiness>();
                //var userdata = await _userBusiness.GetSingleById(userId);
                var data = await _blsBusiness.GetAppointmentDetailsByServiceId(serviceId);
                var model = data.FirstOrDefault();
                if (model.IsNotNull())
                {
                    model.AppointmentDateText = string.Format(ApplicationConstant.DateAndTime.DefaultDateFormat, model.AppointmentDate);
                    model.AppointmentDateSlotText = string.Format(ApplicationConstant.DateAndTime.DefaultYY_MM_DD, model.AppointmentDate) + " " + model.AppointmentSlot;
                    if (model.CurrentContactNumber.IsNotNullAndNotEmpty())
                    {
                        model.CurrentContactNumberText = model.CurrentContactNumber.Substring(0, 1) + "******" + model.CurrentContactNumber.Substring(model.CurrentContactNumber.Length - 3, 3);
                    }
                    //if (userdata.IsNotNull())
                    //{
                    //    model.PhotoId = userdata.PhotoId;
                    //}
                    if (model.PhotoId.IsNotNullAndNotEmpty())
                    {
                        var bytes = await _fileBusiness.GetFileByte(model.PhotoId);
                        if (bytes.Length > 0)
                        {
                            model.Photo = Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }
                    else
                    {
                        var appdoc = await _applicationDocumentBusiness.GetSingle(x => x.Code == "USER_PROFILE");
                        var bytes = await _fileBusiness.GetFileByte(appdoc.DocumentId);
                        if (bytes.Length > 0)
                        {
                            model.Photo = Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }
                    if (model.QRCodeId.IsNotNullAndNotEmpty())
                    {
                        var bytes = await _fileBusiness.GetFileByte(model.QRCodeId);
                        if (bytes.Length > 0)
                        {
                            model.QRCode = Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }
                    else
                    {
                        var appdoc = await _applicationDocumentBusiness.GetSingle(x => x.Code == "EMPTY_QR_CODE");
                        var bytes = await _fileBusiness.GetFileByte(appdoc.DocumentId);
                        if (bytes.Length > 0)
                        {
                            model.QRCode = Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }
                    if (model.BarCodeId.IsNotNullAndNotEmpty())
                    {
                        var bytes2 = await _fileBusiness.GetFileByte(model.BarCodeId);
                        if (bytes2.Length > 0)
                        {
                            model.BarCode = Convert.ToBase64String(bytes2, 0, bytes2.Length);
                        }
                    }
                    else
                    {
                        var appdoc = await _applicationDocumentBusiness.GetSingle(x => x.Code == "EMPTY_QR_CODE");
                        var bytes2 = await _fileBusiness.GetFileByte(appdoc.DocumentId);
                        if (bytes2.Length > 0)
                        {
                            model.BarCode = Convert.ToBase64String(bytes2, 0, bytes2.Length);
                        }
                    }
                    if (model.LocationImageId.IsNotNullAndNotEmpty())
                    {
                        var bytes = await _fileBusiness.GetFileByte(model.LocationImageId);
                        if (bytes.Length > 0)
                        {
                            model.LocationImage = Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }
                    else
                    {
                        var appdoc = await _applicationDocumentBusiness.GetSingle(x => x.Code == "EMPTY_MAP");
                        var bytes = await _fileBusiness.GetFileByte(appdoc.DocumentId);
                        if (bytes.Length > 0)
                        {
                            model.LocationImage = Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }
                    //model.AppointmentAddress="Shop No# 13, Ground Floor, Zeenah Building of Budget Rent a Car, Opposite to Deira City Center P3 Parking, Deira, Dubai";
                }
                else
                {
                    model = new BLSVisaAppointmentViewModel();
                }
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
