using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Hangfire;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Synergy.App.Business
{
    public class SalesBusiness : BusinessBase<ServiceViewModel, NtsService>, ISalesBusiness
    {
        private readonly IRepositoryQueryBase<DirectSalesViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<ProgramDashboardViewModel> _queryPDRepo;
        private readonly IRepositoryQueryBase<ProjectGanttTaskViewModel> _queryGantt;
        private readonly IRepositoryQueryBase<TeamWorkloadViewModel> _queryTWRepo;
        private readonly IRepositoryQueryBase<DashboardCalendarViewModel> _queryDCRepo;
        private readonly IRepositoryQueryBase<PerformanceDashboardViewModel> _queryProjDashRepo;
        private readonly IRepositoryQueryBase<ProjectDashboardChartViewModel> _queryProjDashChartRepo;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryTaskRepo;
        private readonly IRepositoryQueryBase<MailViewModel> _queryMailTaskRepo;
        private readonly IRepositoryQueryBase<PerformanceDocumentViewModel> _queryPerDoc;
        private readonly IRepositoryQueryBase<PerformanceDocumentStageViewModel> _queryPerDocStage;
        private readonly IRepositoryQueryBase<GoalViewModel> _queryGoal;
        private readonly IRepositoryQueryBase<NoteTemplateViewModel> _queryNoteTemplate;
        private readonly ICompanyBusiness _companyBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IUserContext _userContext;
        private readonly INtsTaskPrecedenceBusiness _ntsTaskPrecedenceBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IStepTaskComponentBusiness _stepCompBusiness;
        private readonly IRepositoryQueryBase<PerformaceRatingViewModel> _queryPerformanceRatingRepo;
        private readonly IRepositoryQueryBase<PerformanceRatingItemViewModel> _queryPerformanceRatingitemRepo;
        private readonly ICompanySettingBusiness _companySettingBusiness;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISalesQueryBusiness _salesQueryBusiness;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IRepositoryQueryBase<CompetencyCategoryViewModel> _queryCompeencyCategory;
        public SalesBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IRepositoryQueryBase<DirectSalesViewModel> queryRepo,
            IRepositoryQueryBase<ProgramDashboardViewModel> queryPDRepo,
            IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<DashboardCalendarViewModel> queryDCRepo,
            IRepositoryQueryBase<ProjectGanttTaskViewModel> queryGantt,
             IRepositoryQueryBase<TeamWorkloadViewModel> queryTWRepo,
             IRepositoryQueryBase<PerformanceDashboardViewModel> queryProjDashRepo,
             IRepositoryQueryBase<ProjectDashboardChartViewModel> queryProjDashChartRepo,
             ISalesQueryBusiness salesQueryBusiness,
             IRepositoryQueryBase<TaskViewModel> queryTaskRepo, INtsTaskPrecedenceBusiness ntsTaskPrecedenceBusiness, ITableMetadataBusiness tableMetadataBusiness,
            IMapper autoMapper, ICompanyBusiness companyBusiness, INoteBusiness noteBusiness, IServiceBusiness serviceBusiness, IRepositoryQueryBase<MailViewModel> queryMailTaskRepo,
            IRepositoryQueryBase<PerformanceDocumentViewModel> queryPerDoc, IRepositoryQueryBase<GoalViewModel> queryGoal, IRepositoryQueryBase<PerformanceDocumentStageViewModel> queryPerDocStage
            , IHRCoreBusiness hrCoreBusiness, IUserContext userContext, IRepositoryQueryBase<NoteTemplateViewModel> queryNoteTemplate, IComponentResultBusiness componentResultBusiness, ILOVBusiness lovBusiness
            , ITemplateBusiness templateBusiness, IStepTaskComponentBusiness stepCompBusiness, IRepositoryQueryBase<PerformaceRatingViewModel> queryPerformaceRating,
            IRepositoryQueryBase<PerformanceRatingItemViewModel> queryPerformaceRatingitem, IRepositoryQueryBase<CompetencyViewModel> queryComp, ICompanySettingBusiness companySettingBusiness, IRepositoryQueryBase<CompetencyCategoryViewModel> queryComptetencyCategory, IServiceProvider serviceProvider, Microsoft.Extensions.Configuration.IConfiguration configuration,
            IPortalBusiness portalBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _queryRepo1 = queryRepo1;
            _queryPDRepo = queryPDRepo;
            _queryDCRepo = queryDCRepo;
            _queryGantt = queryGantt;
            _queryTWRepo = queryTWRepo;
            _queryProjDashRepo = queryProjDashRepo;
            _queryProjDashChartRepo = queryProjDashChartRepo;
            _queryTaskRepo = queryTaskRepo;
            _companyBusiness = companyBusiness;
            _serviceBusiness = serviceBusiness;
            _noteBusiness = noteBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _ntsTaskPrecedenceBusiness = ntsTaskPrecedenceBusiness;
            _queryMailTaskRepo = queryMailTaskRepo;
            _queryPerDoc = queryPerDoc;
            _queryPerDocStage = queryPerDocStage;
            _hrCoreBusiness = hrCoreBusiness;
            _componentResultBusiness = componentResultBusiness;
            _queryGoal = queryGoal;
            _userContext = userContext;
            _queryNoteTemplate = queryNoteTemplate;
            _lovBusiness = lovBusiness;
            _templateBusiness = templateBusiness;
            _stepCompBusiness = stepCompBusiness;
            _serviceProvider = serviceProvider;
            _queryPerformanceRatingRepo = queryPerformaceRating;
            _queryPerformanceRatingitemRepo = queryPerformaceRatingitem;
            _companySettingBusiness = companySettingBusiness;
            _queryCompeencyCategory = queryComptetencyCategory;
            _salesQueryBusiness = salesQueryBusiness;
            _configuration = configuration;
            _portalBusiness = portalBusiness;
        }
        public async override Task<CommandResult<ServiceViewModel>> Create(ServiceViewModel model, bool autoCommit = true)
        {

            return CommandResult<ServiceViewModel>.Instance();
        }
        public async override Task<CommandResult<ServiceViewModel>> Edit(ServiceViewModel model, bool autoCommit = true)
        {
            return CommandResult<ServiceViewModel>.Instance();
        }
        public async Task<CommandResult<NoteTemplateViewModel>> GenerateLicense(ServiceTemplateViewModel svm)
        {
            var lvm = new Synergy.App.ViewModel.LicenseViewModel();
            var rowData1 = svm.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            if (svm != null )
            {                           
                var customer = rowData1.ContainsKey("CustomerId") ? Convert.ToString(rowData1["CustomerId"]) : "";
                if (customer.IsNotNullAndNotEmpty())
                {                    
                    lvm.CustomerId = Convert.ToString(rowData1["CustomerId"]);
                }
                var product = rowData1.ContainsKey("ProductId") ? Convert.ToString(rowData1["ProductId"]) : "";                
                if (product.IsNotNullAndNotEmpty())
                {                   
                    lvm.ProductId = Convert.ToString(rowData1["ProductId"]);
                }
                var machineName = rowData1.ContainsKey("MachineName") ? Convert.ToString(rowData1["MachineName"]) : "";                
                if (machineName.IsNotNullAndNotEmpty())
                {
                    lvm.MachineName = Convert.ToString(rowData1["MachineName"]);
                }
                var uc = rowData1.ContainsKey("UserCount") ? Convert.ToString(rowData1["UserCount"]) : "";                
                if (uc.IsNotNullAndNotEmpty())
                {
                    lvm.UserCount = Convert.ToInt64(rowData1["UserCount"]);

                }
                var le = rowData1.ContainsKey("LicenseExpiryDate") ? Convert.ToString(rowData1["LicenseExpiryDate"]) : "";              
                if (le.IsNotNullAndNotEmpty())
                {
                    lvm.ExpiryDate = Convert.ToDateTime(rowData1["LicenseExpiryDate"]);

                }
                lvm.SalesOrderId = svm.ServiceId;
            }
            var noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var noteViewModel = await _salesQueryBusiness.GenerateLicense(lvm.CustomerId, lvm.ProductId,lvm.MachineName);

            var licensePrivateKey = "";
            var guid = "";
            var newNoteViewModel = new NoteTemplateViewModel();
            if (noteViewModel != null)
            {
                 newNoteViewModel = await noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                {
                    NoteId = noteViewModel.NoteId,
                    ActiveUserId = svm.ActiveUserId,
                    DataAction = DataActionEnum.Edit,
                });
                //newNoteViewModel.TemplateAction = NtsActionEnum.EditAsNewVersion;
                newNoteViewModel.DataAction = DataActionEnum.Edit;
                licensePrivateKey = noteViewModel.PrivateKey;
                guid = Helper.Decrypt(licensePrivateKey);
            }
            else
            {
                 newNoteViewModel =await noteBusiness.GetNoteDetails(new NoteTemplateViewModel
                {
                    TemplateCode = "SYN_SALE_APPLICATION_LICENSE",
                    DataAction = DataActionEnum.Create,
                    ActiveUserId = _userContext.UserId
                });
               // noteViewModel.TemplateAction = NtsActionEnum.Submit;
                newNoteViewModel.DataAction = DataActionEnum.Create;
                guid = Guid.NewGuid().ToString();
                licensePrivateKey = Helper.Encrypt(guid);
                //newNoteViewModel.Code = licensePrivateKey;
            }

            var daysLeft = (lvm.ExpiryDate.Value - DateTime.Today).Days;
            var date = DateTime.Today.AddMilliseconds(Convert.ToDouble(lvm.UserCount));
            var data = string.Concat(guid, "|", lvm.ExpiryDate.Value.ToYYY_MM_DD(), "|", lvm.UserCount, "|", lvm.CustomerCode, "|", lvm.MachineName);
            var key = Helper.Encrypt(data);

            newNoteViewModel.StartDate = DateTime.Now;
            newNoteViewModel.ExpiryDate = lvm.ExpiryDate;
            newNoteViewModel.ActiveUserId = newNoteViewModel.OwnerUserId = newNoteViewModel.RequestedByUserId = _userContext.UserId;

           // newNoteViewModel.ReferenceTypeCode = ReferenceTypeEnum.NTS_Service;
            //newNoteViewModel.ReferenceNode = NodeEnum.NTS_Service;
            newNoteViewModel.ReferenceId = lvm.SalesOrderId;

            //newNoteViewModel.ReferenceType = NoteReferenceTypeEnum.Self;
          //  newNoteViewModel.ReferenceTo = _userContext.UserId;
            newNoteViewModel.Subject = svm.Subject;
            //noteViewModel.Description = svm.Description;



            if (newNoteViewModel != null)
            {
                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("LicenseKey",Convert.ToString(key));
                ((IDictionary<String, Object>)exo).Add("PrivateKey",Convert.ToString(licensePrivateKey));
                ((IDictionary<String, Object>)exo).Add("CustomerId", Convert.ToString(rowData1["CustomerId"]));
                ((IDictionary<String, Object>)exo).Add("ProductId", Convert.ToString(rowData1["ProductId"]));
                ((IDictionary<String, Object>)exo).Add("MachineName", Convert.ToString(rowData1["MachineName"]));
                ((IDictionary<String, Object>)exo).Add("UserCount", Convert.ToString(rowData1["UserCount"]));
                newNoteViewModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                newNoteViewModel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                //var lk = noteViewModel.Controls.FirstOrDefault(x => x.FieldName == "licenseKey");
                //if (lk != null)
                //{
                //    lk.Code = lk.Value = key;
                //}
                //var customer = noteViewModel.Controls.FirstOrDefault(x => x.FieldName == "customer");
                //if (customer != null)
                //{
                //    customer.Code = lvm.CustomerCode;
                //    customer.Value = lvm.CustomerName;
                //}
                //var product = noteViewModel.Controls.FirstOrDefault(x => x.FieldName == "product");
                //if (product != null)
                //{
                //    product.Code = lvm.ProductCode;
                //    product.Value = lvm.ProductName;
                //}
                //var machineName = noteViewModel.Controls.FirstOrDefault(x => x.FieldName == "machineName");
                //if (machineName != null)
                //{
                //    machineName.Code = lvm.MachineName;
                //    machineName.Value = lvm.MachineName;
                //}
                //var uc = noteViewModel.Controls.FirstOrDefault(x => x.FieldName == "userCount");
                //if (uc != null)
                //{
                //    uc.Code = uc.Value = lvm.UserCount.ToSafeString();

                //}
                var result = await noteBusiness.ManageNote(newNoteViewModel);
                return result;
            }
            return null;


        }
      
        private CommandResult<LicenseViewModel> DecryptLicense(string licenseKey, string licensePrivateKey, string machineName)
        {
            var result = CommandResult<LicenseViewModel>.Instance();
            try
            {

                var key = Helper.Decrypt(licenseKey);
                var items = key.Split('|');
                var guid = items[0];
                var exp = items[1].ToSafeDateTime();
                var userCount = items[2];
                var customerCode = items[3];
                var mName = items[4];

                //if (guid == licensePrivateKey && mName.ToLower() == machineName.ToLower())
                if (guid == licensePrivateKey)
                {
                    result.IsSuccess = true;
                    result.Item = new LicenseViewModel
                    {
                        IsLicenseValid = true,
                        ExpiryDate = exp,
                        IsExpired = exp < DateTime.Today,
                        UserCount = Convert.ToInt32(userCount),
                        MachineName = mName,
                        CustomerCode = customerCode,
                        LicenseKey = licenseKey
                    };
                    return result;
                }
                else
                {

                    result.IsSuccess = false;
                }
                return result;
            }
            catch (Exception)
            {

                return result;
            }

        }

        private async Task<CommandResult<LicenseViewModel>> GetLicenseDetailsFromLicenseServer(string licenseKey, string licensePrivateKey, string machineName)
        {
            var result = CommandResult<LicenseViewModel>.Instance();
            try
            {
                var licenseApiBaseurl = await _companySettingBusiness.GetSingle(x => x.Code == "LSU");
                var licenseApi = Helper.Decrypt(licenseApiBaseurl.Value/*ApplicationConstant.AppSettings.LicenseApiBaseUrl(_configuration)*/);
                using (var client = new WebClient())
                {
                    var uri = new Uri(string.Concat(licenseApi, "cms/query/getlicensedetils?licenseKey=", licenseKey, "&licensePrivateKey=", licensePrivateKey, "&machineName=", machineName));
                    client.Headers.Add("Content-Type:application/json");
                    client.Headers.Add("Accept:application/json");
                    var response = client.DownloadString(uri);
                    var json = JObject.Parse(response);
                    result.IsSuccess = json.Value<bool>("success");
                    result.Item = JsonConvert.DeserializeObject<LicenseViewModel>(json.Value<string>("result"), new Newtonsoft.Json.Converters.StringEnumConverter());
                    return result;

                }
            }
            catch (Exception ex)
            {
                result = CommandResult<LicenseViewModel>.Instance();
                return result;
            }

        }
        public async Task<LicenseViewModel> GetSelfLicense(string machineName)
        {
            return new LicenseViewModel { IsLicenseValid = true, IsExpired = false, UserCount = 300 };
            var portal =await _portalBusiness.GetSingleById(_userContext.PortalId);
            if (portal == null || portal.LicenseKey == null)
            {
                return new LicenseViewModel { Id = _userContext.PortalId, IsLicenseValid = false, UserCount = 0 };
            }
            try
            {
                var licensePrivateKey = Helper.Decrypt(portal.LicensePrivateKey /*ApplicationConstant.AppSettings.LicensePrivateKey(_configuration)*/);
                var license = DecryptLicense(portal.LicenseKey, licensePrivateKey, machineName);

                if (license != null && license.Item != null && license.IsSuccess)
                {
                    license.Item.Id = portal.Id;
                    return license.Item;
                }
                else
                {
                    license =await GetLicenseDetailsFromLicenseServer(portal.LicenseKey, licensePrivateKey, machineName);
                    if (license != null && license.Item != null && license.IsSuccess)
                    {
                        license.Item.Id = portal.Id;
                        return license.Item;
                    }
                    else
                    {
                        return new LicenseViewModel { Id = _userContext.PortalId, IsLicenseValid = false, UserCount = 0 };
                    }
                }
            }
            catch (Exception)
            {

                return new LicenseViewModel { Id = _userContext.CompanyId, IsLicenseValid = false, UserCount = 0 };
            }


        }
        public async Task<bool> EvaluateLicense(string licenseKey, string machineName)
        {
            try
            {
                var portal =await _portalBusiness.GetSingleById(_userContext.PortalId);
                if (portal == null)
                {
                    return false;
                }
                var license =await GetLicenseDetailsFromLicenseServer(licenseKey, portal.LicensePrivateKey /*ApplicationConstant.AppSettings.LicensePrivateKey(_configuration)*/, machineName);

                if (license != null && license.Item != null && license.IsSuccess && license.Item.IsLicenseValid)
                {
                    portal.LicenseKey = licenseKey;
                     portal.LastUpdatedDate = DateTime.Now;
                    await _portalBusiness.Edit(portal);
                    return true;
                }
                else
                {
                    license = DecryptLicense(licenseKey, Helper.Decrypt(portal.LicensePrivateKey/*ApplicationConstant.AppSettings.LicensePrivateKey(_configuration)*/), machineName);
                    if (license != null && license.Item != null && license.IsSuccess && license.Item.IsLicenseValid)
                    {
                        portal.LicenseKey = licenseKey;
                        portal.LastUpdatedDate = DateTime.Now;
                        await _portalBusiness.Edit(portal);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {

                return false;
            }


        }
        public async Task<LicenseViewModel> GetLicenseDetails(string licenseKey, string licensePrivateKey, string machineName)
        {


            var lvm = await _salesQueryBusiness.GetLicenseNote(licensePrivateKey) ;
            if (lvm == null)
            {
                return null;
            }
            else
            {
                var lk = DecryptLicense(licenseKey, Helper.Decrypt(licensePrivateKey), machineName);
                if (lk != null && lk.IsSuccess && lk.Item != null)
                {
                    lvm.UserCount = lk.Item.UserCount;
                    lvm.IsExpired = lk.Item.IsExpired;
                    lvm.ExpiryDate = lk.Item.ExpiryDate;
                    lvm.IsLicenseValid = lk.Item.IsLicenseValid;
                }
                return lvm;
            }
        }
    }
}
