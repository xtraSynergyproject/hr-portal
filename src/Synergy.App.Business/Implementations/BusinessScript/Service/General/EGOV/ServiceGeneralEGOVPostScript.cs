
using Newtonsoft.Json.Linq;
using Synergy.App.Business.Interface.BusinessScript.Service.General.EGOV;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Service.General.EGOV
{
    public class ServiceGeneralEGOVPostScript : IServiceGeneralEGOVPostScript
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>    
       
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateBinBookingDetails(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<IEGovernanceBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                //var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                //var ComplaintTilte = Convert.ToString(rowData["ComplaintTitle"]);
                //if (ComplaintTilte.IsNotNullAndNotEmpty())
                //{
                //    viewModel.ServiceSubject = ComplaintTilte;
                //}
                
                await _egovBusiness.UpdateBinBookingDetails(udf);
            }
            
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateRentalPropertyStatus(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<IEGovernanceBusiness>();
                        
            var model = new TaskTemplateViewModel()
            {
               TemplateCode = viewModel.TemplateCode,
               ServiceStatusCode = viewModel.ServiceStatusCode
            };

            await _egovBusiness.UpdateRentalStatus(model, udf);                     

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateJSCRevAssetDocToDMS(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _smartCityBusiness = sp.GetService<ISmartCityBusiness>();
            var _fileBusiness = sp.GetService<IFileBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();

            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var assetname = Convert.ToString(rowData["AssetName"]);
            var assettypeid = Convert.ToString(rowData["AssetTypeId"]);
            var wardid = Convert.ToString(rowData["WardId"]);            
            var assetphotodata = rowData["AssetPhotoId"];
            var filePhotoData = new FileViewModel();
            if (assetphotodata.IsNotNull())
            {               
                var photodata = JToken.Parse(assetphotodata.ToString());
                if (photodata.IsNotNull())
                {
                    var photodata1 = photodata.FirstOrDefault();
                    if (photodata1.IsNotNull())
                    {
                        var idtoken = photodata1.SelectToken("id");
                        if (idtoken.IsNotNull())
                        {
                            filePhotoData.Id = idtoken.Value<string>();
                        }
                        

                    }
                }
            }
            
            var assettypename = "Others";
            var wardname = "Others";
            var assetphotoname = "Others";

            var folderNo = "N-19.09.2022-103";
            var folderList = new List<string>();

            if (assetname.IsNullOrEmptyOrWhiteSpace())
            {
                assetname = "Others";
            }
            if (assettypeid.IsNotNullAndNotEmpty())
            {
                var assettypedata = await _smartCityBusiness.GetAssetTypeForJammuById(assettypeid);
                if (assettypedata!=null)
                {
                    assettypename = assettypedata.Name;
                }
            }
            if (wardid.IsNotNullAndNotEmpty())
            {
                var warddata = await _smartCityBusiness.GetWardForJammuById(wardid);
                if (warddata != null)
                {
                    wardname = warddata.Name;
                }
            }

            folderList.Add(wardname);
            folderList.Add(assettypename);

            if (filePhotoData.Id.IsNotNull())
            {
                var file = await _fileBusiness.GetSingleById(filePhotoData.Id);
                if (file !=null)
                {
                    assetphotoname = string.Concat(assetname, "_", file.FileName);
                    var res = await _noteBusiness.CreateGeneralDocument(folderNo, folderList, filePhotoData.Id, assetphotoname, uc.UserId);
                }
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateJSCRevAssetAllotmentDocToDMS(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _smartCityBusiness = sp.GetService<ISmartCityBusiness>();
            var _fileBusiness = sp.GetService<IFileBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();

            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var assetid = Convert.ToString(rowData["AssetId"]);
            
            var idproofdatajson = rowData["IdProofImageId"];
            var fileIdProofData = new FileViewModel();
            if (idproofdatajson.IsNotNull())
            {
                var photodata = JToken.Parse(idproofdatajson.ToString());
                if (photodata.IsNotNull())
                {
                    var photodata1 = photodata.FirstOrDefault();
                    if (photodata1.IsNotNull())
                    {
                        var idtoken = photodata1.SelectToken("id");
                        if (idtoken.IsNotNull())
                        {
                            fileIdProofData.Id = idtoken.Value<string>();
                        }


                    }
                }
            }
            var assetname = "Others";
            var assettypename = "Others";
            var wardname = "Others";
            var idproofname = "Others";

            var folderNo = "N-19.09.2022-104";
            var folderList = new List<string>();

            if (assetid.IsNotNullAndNotEmpty())
            {
                var assetdata = await _smartCityBusiness.GetJSCAssetDetailsById(assetid);
                if (assetdata!=null)
                {
                    if (assetdata.AssetName.IsNotNullAndNotEmpty())
                    {
                        assetname = assetdata.AssetName;
                    }
                    if (assetdata.AssetTypeName.IsNotNullAndNotEmpty())
                    {
                        assettypename = assetdata.AssetTypeName;
                    }
                    if (assetdata.WardName.IsNotNullAndNotEmpty())
                    {
                        wardname = assetdata.WardName;
                    }
                }
            }
            folderList.Add(wardname);
            folderList.Add(assettypename);
            folderList.Add(assetname);

            if (fileIdProofData.Id.IsNotNull())
            {
                var file = await _fileBusiness.GetSingleById(fileIdProofData.Id);
                if (file != null)
                {
                    idproofname = string.Concat(assetname, "_", file.FileName);
                    var res = await _noteBusiness.CreateGeneralDocument(folderNo, folderList, fileIdProofData.Id, idproofname, uc.UserId);
                }
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }
}
