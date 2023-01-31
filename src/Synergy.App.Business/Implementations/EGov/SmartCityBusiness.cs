using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using System.Data;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using ProtoBuf.Meta;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Office2010.Excel;
using static Nest.JoinField;
using Org.BouncyCastle.Asn1.X500;
using DocumentFormat.OpenXml.Bibliography;
using UglyToad.PdfPig.Graphics.Operations.PathPainting;



namespace Synergy.App.Business
{
    public class SmartCityBusiness : BusinessBase<NoteViewModel, NtsNote>, ISmartCityBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _querydata;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<ServiceTemplateViewModel> _query;
        private readonly INoteBusiness _noteBusiness;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryRepo1;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly ISmartCityQueryBusiness _smartCityQueryBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        IUserContext _userContext;
        private readonly INotificationBusiness _notificationBusiness;
        public SmartCityBusiness(IRepositoryQueryBase<IdNameViewModel> querydata
            , IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper
            , IRepositoryQueryBase<NoteViewModel> queryRepo
            , IRepositoryQueryBase<TaskViewModel> queryRepo1, ICmsBusiness cmsBusiness
            , IRepositoryQueryBase<ServiceTemplateViewModel> query
            , INoteBusiness noteBusiness
            , ISmartCityQueryBusiness smartCityQueryBusiness
            , ILOVBusiness lOVBusiness
            , IServiceBusiness serviceBusiness
            , IUserBusiness userBusiness
            , IUserContext userContext
            , ITaskBusiness taskBusiness, INotificationBusiness notificationBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _query = query;
            _noteBusiness = noteBusiness;
            _queryRepo1 = queryRepo1;
            _lOVBusiness = lOVBusiness;
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _querydata = querydata;
            _smartCityQueryBusiness = smartCityQueryBusiness;
            _userContext = userContext;
            _taskBusiness = taskBusiness;
            _cmsBusiness = cmsBusiness;
            _notificationBusiness = notificationBusiness;
        }

        public async Task<JSCAssetConsumerViewModel> ReadJSCAssetConsumerData(string consumerId)
        {
            var result = await _smartCityQueryBusiness.ReadJSCAssetConsumerData(consumerId);
            return result;
        }
        public async Task<List<TreeViewViewModel>> GetJSCMapViewTreeList()
        {
            var result = await _smartCityQueryBusiness.GetJSCMapViewTreeList();
            return result;
        }
        public async Task<List<JSCGrievanceWorkflow>> GetGrievanceWorkflowList()
        {
            var result = await _smartCityQueryBusiness.GetGrievanceWorkflowList();
            return result;
        }

        public async Task<List<TreeViewViewModel>> GetAssetMapViewTreeList()
        {
            var result = await _smartCityQueryBusiness.GetAssetMapViewTreeList();
            return result;
        }
        public async Task<IList<JSCAssetConsumerViewModel>> GetAssetCountByWard(string wardId = null, string collectorId = null, string revType = null)
        {
            var result = await _smartCityQueryBusiness.GetAssetCountByWard(wardId, collectorId, revType);
            return result;
        }

        public async Task<IList<JSCAssetConsumerViewModel>> GetAssetAllotmentCountByWard(string wardId = null, string collectorId = null, string revType = null)
        {
            var result = await _smartCityQueryBusiness.GetAssetAllotmentCountByWard(wardId, collectorId, revType);
            return result;
        }

        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByWard(string wardId = null, string collectorId = null)
        {
            var result = await _smartCityQueryBusiness.GetAssetPaymentCountByWard(wardId, collectorId);
            return result;

        }

        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByAssetType(string wardId = null, string collectorId = null)
        {
            var result = await _smartCityQueryBusiness.GetAssetPaymentCountByAssetType(wardId, collectorId);
            return result;
        }

        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByCollector(string wardId = null, string collectorId = null)
        {
            var result = await _smartCityQueryBusiness.GetAssetPaymentCountByCollector(wardId, collectorId);
            return result;
        }

        public async Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByPaymentStatus(string wardId = null, string collectorId = null)
        {
            var result = await _smartCityQueryBusiness.GetAssetPaymentCountByPaymentStatus(wardId, collectorId);
            return result;
        }

        public async Task<IdNameViewModel> GetAssetTypeForJammuById(string assetTypeId)
        {
            var result = await _smartCityQueryBusiness.GetAssetTypeForJammuById(assetTypeId);
            return result;
        }
        public async Task<IdNameViewModel> GetWardForJammuById(string wardId)
        {
            var result = await _smartCityQueryBusiness.GetWardForJammuById(wardId);
            return result;
        }
        public async Task<JSCAssetViewModel> GetJSCAssetDetailsById(string assetId)
        {
            var result = await _smartCityQueryBusiness.GetJSCAssetDetailsById(assetId);
            return result;

        }
        public async Task<JSCBillPaymentReportViewModel> GetJSCBillPaymentDetails(string serviceId)
        {
            var result = await _smartCityQueryBusiness.GetJSCBillPaymentDetails(serviceId);
            return result;
        }

        public async Task<PropertyTaxPaymentViewModel> GetPropertyTaxReportData(string ddno, string year)
        {
            var result = await _smartCityQueryBusiness.GetPropertyTaxReportData(ddno, year);
            return result;
        }

        public async Task<List<PropertyTaxPaymentViewModel>> GetPropertyTaxReportFloorData(string ddno, string year)
        {
            var result = await _smartCityQueryBusiness.GetPropertyTaxReportFloorData(ddno, year);
            return result;
        }
        public async Task<IdNameViewModel> GetBuildingCategory(string buildingType)
        {
            var result = await _smartCityQueryBusiness.GetBuildingCategory(buildingType);
            return result;
        }
        public async Task<List<JSCPropertyFloorViewModel>> GetAssessmentFloorData(string assessmentId)
        {
            var result = await _smartCityQueryBusiness.GetAssessmentFloorData(assessmentId);
            return result;
        }
        public async Task<JSCPropertySelfAssessmentViewModel> GetSelfAssessmentData(string assessmentId)
        {
            var result = await _smartCityQueryBusiness.GetSelfAssessmentData(assessmentId);
            return result;
        }
        public async Task GenerateAssetBillPayment()
        {
            try
            {
                var billDate = DateTime.Today;
                var result = await _smartCityQueryBusiness.GetAssetAllotmentDetails(billDate);

                if (result.IsNotNull())
                {
                    foreach (var res in result)
                    {
                        var paymentList = await _smartCityQueryBusiness.CheckIfPaymentDataAlreadyProcessed(res.AssetId, res.ConsumerId, billDate);
                        if (paymentList <= 0)
                        {
                            var serviceTempModel = new ServiceTemplateViewModel();

                            serviceTempModel.ActiveUserId = _userContext.UserId;
                            serviceTempModel.TemplateCode = "JSC_ASSET_PAYMENT";
                            var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);

                            servicemodel.OwnerUserId = _userContext.UserId;
                            servicemodel.DataAction = DataActionEnum.Create;
                            servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                            dynamic exo = new System.Dynamic.ExpandoObject();

                            ((IDictionary<String, Object>)exo).Add("AssetId", res.AssetId);
                            ((IDictionary<String, Object>)exo).Add("ConsumerId", res.ConsumerId);
                            ((IDictionary<String, Object>)exo).Add("BillDate", billDate);
                            ((IDictionary<String, Object>)exo).Add("Amount", res.FeeAmount);
                            //((IDictionary<String, Object>)exo).Add("PaymentModeId", res.Payme);
                            // ((IDictionary<String, Object>)exo).Add("PaymentReferenceNo", servicemodel.ServiceNo);
                            var dueDate = billDate.AddDays(res.PaymentDueDays.ToSafeInt());
                            ((IDictionary<String, Object>)exo).Add("DueDate", dueDate);

                            servicemodel.Json = JsonConvert.SerializeObject(exo);
                            await _serviceBusiness.ManageService(servicemodel);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public async Task<string> GetJSCConsumerUserId(string consumerId)
        {
            var result = await _smartCityQueryBusiness.GetJSCConsumerUserId(consumerId);
            return result;
        }

        public async Task<List<JSCColonyViewModel>> GetJSCColonyMapViewList()
        {
            var result = await _smartCityQueryBusiness.GetJSCColonyMapViewList();
            return result;
        }

        public async Task<JSCAssetConsumerViewModel> GetConsumerByConsumerNo(string consumerNo)
        {
            var queryData = await _smartCityQueryBusiness.GetConsumerByConsumerNo(consumerNo);
            return queryData;
        }

        public async Task<JMCAssetViewModel> GetAssetByServiceNo(string serviceNo)
        {
            var queryData = await _smartCityQueryBusiness.GetAssetByServiceNo(serviceNo);
            return queryData;
        }

        public async Task<JMCAssetViewModel> GetAssetById(string id)
        {
            var queryData = await _smartCityQueryBusiness.GetAssetById(id);
            return queryData;
        }

        public async Task<List<JSCAssetConsumerViewModel>> GetAssetConsumerData(string assetId)
        {
            var queryData = await _smartCityQueryBusiness.GetAssetConsumerData(assetId);
            return queryData;
        }

        public async Task<List<JSCAssetConsumerViewModel>> GetAssetPaymentData(string assetId)
        {
            var queryData = await _smartCityQueryBusiness.GetAssetPaymentData(assetId);
            return queryData;
        }

        public async Task<List<JSCAssetConsumerViewModel>> GetConsumerPaymentData(string consumerId)
        {
            var queryData = await _smartCityQueryBusiness.GetConsumerPaymentData(consumerId);
            return queryData;
        }

        public async Task<List<JMCAssetViewModel>> GetConsumerAssetData(string consumerId)
        {
            var queryData = await _smartCityQueryBusiness.GetConsumerAssetData(consumerId);
            return queryData;
        }

        public async Task<JSCAssetConsumerViewModel> GetConsumerById(string id)
        {
            var queryData = await _smartCityQueryBusiness.GetConsumerById(id);
            return queryData;
        }

        public async Task<List<JSCParcelViewModel>> GetJSCParcelMapViewList(string colName, string colText)
        {
            var result = await _smartCityQueryBusiness.GetJSCParcelMapViewList(colName, colText);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetParcelColumnList()
        {
            var result = await _smartCityQueryBusiness.GetParcelColumnList();
            return result;
        }

        public async Task<List<IdNameViewModel>> GetColonyList()
        {
            var result = await _smartCityQueryBusiness.GetColonyList();
            return result;
        }

        public async Task<List<IdNameViewModel>> GetParcelTypeList()
        {
            var result = await _smartCityQueryBusiness.GetParcelTypeList();
            return result;
        }

        public async Task<List<IdNameViewModel>> GetWardList()
        {
            var result = await _smartCityQueryBusiness.GetWardList();
            return result;
        }
        public async Task<List<IdNameViewModel>> GetJSCZoneList()
        {
            var result = await _smartCityQueryBusiness.GetJSCZoneList();
            return result;
        }
        public async Task<List<IdNameViewModel>> GetJSCZoneListByDepartment(string departmentId)
        {
            var result = await _smartCityQueryBusiness.GetJSCZoneListByDepartment(departmentId);
            return result;
        }
        public async Task<List<JSCParcelViewModel>> GetParcelSearchByWardandType(string ward, string parcelType)
        {
            var result = await _smartCityQueryBusiness.GetParcelSearchByWardandType(ward, parcelType);
            return result;
        }
        public async Task<bool> ValidateStartDateandEndDate(ServiceTemplateViewModel viewModel)
        {

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var startDate = Convert.ToString(rowData.GetValueOrDefault("StartDate"));
            var endDate = Convert.ToString(rowData.GetValueOrDefault("EndDate"));
            var newId = viewModel.UdfNoteTableId;
            var query =
                $@" select ""Id""
                    from cms.""N_SNC_JSC_BOOKING_PERMISSION_JSCHoardingBooking"" 
                        where  ""IsDeleted""='false' and  ""CompanyId""='{_userContext.CompanyId}' and ((""StartDate"">='{startDate}' and ""StartDate""<='{endDate}')
                                                    or
                                                         (""EndDate"">='{startDate}' and ""EndDate""<='{endDate}')) and ""Id""!='{newId}' ";

            var exisiting = await _queryRepo1.ExecuteQueryList(query, null);
            if (exisiting.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<List<IdNameViewModel>> GetParcelIdNameList()
        {
            var result = await _smartCityQueryBusiness.GetParcelIdNameList();
            return result;
        }
        public async Task<List<IdNameViewModel>> GetBinCollectorNameList()
        {
            var result = await _smartCityQueryBusiness.GetBinCollectorNameList();
            return result;
        }
        public async Task<JSCCollectorViewModel> GetBinCollectorMobile(string userId)
        {
            var result = await _smartCityQueryBusiness.GetBinCollectorMobile(userId);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetJSCOwnerList()
        {
            var result = await _smartCityQueryBusiness.GetJSCOwnerList();
            return result;
        }

        public async Task<List<JSCParcelViewModel>> GetJSCWardMapViewList(string wardNo)
        {
            var query = $@"SELECT 
                            'Feature' as ""type"",
                            ST_AsGeoJSON(ST_Transform(""geom"", 4326), 6) :: text as ""geometry"",
                            ""prop_id"", ""res_stat"", ""road_desc"", ""road_type"", ""own_dtls"", ""own_name"", ""tel_no"",
                            ""aadhar"",
                            'COLONY' as ParentId
                            FROM public.""parcel"" where ""ward_no""='{wardNo}' ";

            var querydata = await _queryRepo.ExecuteQueryList<JSCParcelViewModel>(query, null);
            return querydata;
        }

        public async Task GenerateRevenueCollectionBillForJammu()
        {
            var date = DateTime.Today;
            var FirstDay = new DateTime(date.Year, date.Month, 1);
            var paymentdate = new DateTime(date.Year, date.Month, 10);
            var LastDay = new DateTime(date.Year, date.Month, FirstDay.DaysInMonth());
            //if (date.Date == FirstDay.Date)           
            if (true)
            {
                var queryData = await _smartCityQueryBusiness.GenerateRevenueCollectionBillForJammu();
                var monthly = queryData.Where(x => x.FeeType == "MONTHLY");
                var Yearly = queryData.Where(x => x.FeeType == "YEARLY");
                foreach (var item in queryData)
                {
                    if (item.FeeType == "YEARLY" /*&& date.Month == 1*/)
                    {
                        var Last = new DateTime(date.Year, 12, 1);
                        LastDay = new DateTime(date.Year, 12, Last.DaysInMonth());
                        FirstDay = new DateTime(date.Year, 1, 1);
                    }
                    else
                    {
                        LastDay = new DateTime(date.Year, date.Month, FirstDay.DaysInMonth());
                        FirstDay = new DateTime(date.Year, date.Month, 1);
                    }
                    if ((item.FeeType == "MONTHLY") || (item.FeeType == "YEARLY" /*&& date.Month == 1*/))
                    {
                        var taskTemplate = new TaskTemplateViewModel();
                        taskTemplate.ActiveUserId = _userContext.UserId;
                        taskTemplate.TemplateCode = "JSC_GENERAL_PAYMENT_TASK";
                        var task = await _taskBusiness.GetTaskDetails(taskTemplate);

                        if (item.RevenueType == "GARBAGE_COLLECTION")
                        {
                            task.TaskSubject = "Garbage Collection Bill for Property:" + item.pcl_id + " from " + FirstDay.ToDefaultDateFormat() + " to " + LastDay.ToDefaultDateFormat();
                        }
                        //else if (item.RevenueType == "PROPERTY_TAX")
                        //{
                        //    task.TaskSubject = "Property Tax Bill for Property:" + item.pcl_id + " from " + FirstDay.ToDefaultDateFormat() + " to " + LastDay.ToDefaultDateFormat();
                        //}
                        else
                        {
                            task.TaskSubject = "Rent Bill for Property:" + item.pcl_id + " from " + FirstDay.ToDefaultDateFormat() + " to " + LastDay.ToDefaultDateFormat();
                        }

                        task.OwnerUserId = _userContext.UserId;
                        task.StartDate = DateTime.Now;
                        task.DueDate = DateTime.Now.AddDays(10);
                        task.DataAction = DataActionEnum.Create;
                        task.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        var lov = await _lOVBusiness.GetSingle(x => x.Code == "TASK_ASSIGN_TO_USER");
                        if (lov.IsNotNull())
                        {
                            task.AssignedToTypeId = lov.Id;
                            task.AssignedToTypeCode = lov.Code;
                            task.AssignedToTypeName = lov.Name;
                        }
                        task.AssignedToUserId = item.UserId;
                        var plov = await _lOVBusiness.GetSingle(x => x.Code == "TASK_PRIORITY_MEDIUM");
                        if (plov.IsNotNull())
                        {
                            task.TaskPriorityId = plov.Id;
                        }
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        ((IDictionary<String, Object>)exo).Add("ParcelId", item.gid);
                        ((IDictionary<String, Object>)exo).Add("BillDate", FirstDay);
                        ((IDictionary<String, Object>)exo).Add("BillFromDate", FirstDay);
                        ((IDictionary<String, Object>)exo).Add("BillToDate", LastDay);
                        ((IDictionary<String, Object>)exo).Add("PaymentDueDate", paymentdate);
                        ((IDictionary<String, Object>)exo).Add("RevenueTypeId", item.RevenueTypeId);
                        ((IDictionary<String, Object>)exo).Add("Amount", item.Amount);

                        task.Json = JsonConvert.SerializeObject(exo);
                        var res = await _taskBusiness.ManageTask(task);
                    }
                }
            }

        }
        public async Task<JSCPropertySelfAssessmentViewModel> GetCalculatedPropertyTaxAmount(JSCPropertySelfAssessmentViewModel assessment, string assmentId)
        {
            //var model = new JSCLandRateViewModel { Error = "" };
            //var check = await _smartCityQueryBusiness.CheckPropertyTaxExist(propId, year);
            //if (check != null)
            //{
            //    model.Error = "Property tax for this " + propId + " property is already created for this year";
            //    return model;
            //}
            //else
            //{
            //var parcel = await _smartCityQueryBusiness.GetParcelByPropertyId(propId); 
            //var assessment = await _smartCityQueryBusiness.GetSelfAssessmentData(assmentId);

            double sum = 0.0;
            double btup_area = 0.0;

            var propertyrate = await _smartCityQueryBusiness.GetPropertyTaxRate();
            var formula = await _smartCityQueryBusiness.GetFormula(assessment.PropertyType);
            // var floorDetail = await _smartCityQueryBusiness.GetAssessmentFloorData(assmentId);
            //Unit Area Value(Rate) x Built-up area(sq.ft) x F1(Location Factor) x F2(Age Factor) x F4(Occupancy Factor) x Usage factor for commercial(Type)  property(F3)
            //var landRate = await _smartCityQueryBusiness.GetLandRate(year, parcel.ward_no);
            //var unitareaD = await _smartCityQueryBusiness.GetUnitArea(year);
            var locationfactor = await _smartCityQueryBusiness.GetLandValueRate(assessment.WardId, assessment.PropertyType, formula.FormulaTypeCode);

            var occfactorD = await _smartCityQueryBusiness.GetLandRate(assessment.OccupancyId, "OCCUPANCY");
            var usagefactorD = await _smartCityQueryBusiness.GetLandRate(assessment.BuildingCategory, "USAGE");
            var agefactorData = await _smartCityQueryBusiness.GetAgeFactor("");
            var rooffactorData = await _smartCityQueryBusiness.GetRoofFactor("");

            if (formula.FormulaTypeCode == "JSC_RESIDENTIAL")
            {
                assessment.PropertyRate = propertyrate.ResidentialRate;
            }
            else
            {
                assessment.PropertyRate = propertyrate.CommercialRate;
            }


            if (locationfactor!=null && locationfactor.Rate.IsNotNull())
            {
                assessment.LocationRate = locationfactor.Rate;
            }
            if (occfactorD != null && occfactorD.Rate.IsNotNull())
            {
                assessment.OccupancyRate = occfactorD.Rate;
            }

            if (usagefactorD != null && usagefactorD.Rate.IsNotNull())
            {
                assessment.BuildingRate = usagefactorD.Rate;
            }
            if (assessment.PlinthArea>0)
            {
                btup_area = assessment.PlinthArea * 3;
            }




            foreach (var floor in assessment.FloorDetail)
            {

                
                var agefactorD = agefactorData.FirstOrDefault(x => floor.BuildingAge >= x.RangeFrom && floor.BuildingAge <= x.RangeTo);
                var roofD = rooffactorData.FirstOrDefault(x => x.Id == floor.RoofTypeId);
                //if(floor.FloorType== "GROUND_FLOOR")
                //{
                //    btup_area = floor.Area*3;
                //}
                //var TotalAmount = unitarea * btup_area * locfactor * agefactor * occfactor * (usagefactor == 0.0 ? 1 : usagefactor);
                if (formula.FormulaText.IsNotNullAndNotEmpty())
                {
                    var formulatext = ReplaceFormula(formula.FormulaText);
                    formulatext = formulatext.Replace("LOCATION_FACTOR", locationfactor.Rate.ToString());
                    //formulatext = formulatext.Replace("AGE_FACTOR", agefactorD.ToString());
                    formulatext = formulatext.Replace("OCCUPANCY_FACTOR", occfactorD.Rate.ToString());
                    if (usagefactorD!=null)
                    {
                        formulatext = formulatext.Replace("USAGE_FACTOR", usagefactorD.Rate.ToString());
                    }
                    //formulatext = formulatext.Replace("UNIT_AREA_VALUE", unitarea.ToString());
                    formulatext = formulatext.Replace("BUILD_UP_AREA", floor.Area.ToString());
                    if (roofD != null)
                    {
                        formulatext = formulatext.Replace("ROOF_TYPE", roofD.Rate.ToString());
                    }

                    formulatext = formulatext.Replace("DEPRICIATION", agefactorD.Rate.ToString() + "/100");


                    DataTable dt = new DataTable();
                    var v = dt.Compute(formulatext, "");

                    if(formula.FormulaTypeCode=="JSC_RESIDENTIAL")
                    {
                        formulatext = "("+propertyrate.ResidentialRate.ToString() + "/100)*" + v.ToString();
                    }
                    else
                    {
                        formulatext = "(" + propertyrate.CommercialRate.ToString() + "/100)*" + v.ToString();

                    }

                    var fv = dt.Compute(formulatext, "");


                    sum += Convert.ToDouble(fv);    
                    floor.AgeRate = agefactorD.Rate;
                    floor.Amount = Convert.ToDouble(fv);
                }
            }

            //if any vacant plot
            if (btup_area > 0.0)
            {
                double vacant = assessment.TotalArea - btup_area;
                if (vacant > 0.0)
                {
                    assessment.VacantArea = vacant;
                    var proplov = await _lOVBusiness.GetSingle(x => x.Code == "JSC_VACANT");
                    var vformula = await _smartCityQueryBusiness.GetFormula(proplov.Id);
                    var vlocationfactor = await _smartCityQueryBusiness.GetLandValueRate(assessment.WardId, proplov.Id, formula.FormulaTypeCode);
                    if (vformula.FormulaText.IsNotNullAndNotEmpty())
                    {
                        var formulatext = ReplaceFormula(vformula.FormulaText);
                        formulatext = formulatext.Replace("LOCATION_FACTOR", vlocationfactor.Rate.ToString());                //formulatext = formulatext.Replace("AGE_FACTOR", agefactorD.ToString());
                        formulatext = formulatext.Replace("OCCUPANCY_FACTOR", occfactorD.Rate.ToString());
                        formulatext = formulatext.Replace("VACANT_AREA", vacant.ToString());

                        DataTable dt = new DataTable();
                        var v = dt.Compute(formulatext, "");

                        if (formula.FormulaTypeCode == "JSC_RESIDENTIAL")
                        {
                            formulatext = "(" + propertyrate.ResidentialRate.ToString() + "/100)*" + v.ToString();
                        }
                        else
                        {
                            formulatext = "(" + propertyrate.CommercialRate.ToString() + "/100)*" + v.ToString();

                        }

                        var fv = dt.Compute(formulatext, "");
                        assessment.VacantAmount = Convert.ToDouble(fv);
                        sum += Convert.ToDouble(fv);
                    }
                }
            }
            assessment.TotalAmount = sum;
            return assessment;
            //}

        }
        private string ReplaceFormula(string formulatext)
        {
            if (formulatext.Contains("~"))
            {
                var text = formulatext.Split("|");
                foreach (var item in text)
                {
                    if (item.Contains("~"))
                    {
                        var label = item.Split("~");
                        formulatext = formulatext.Replace(label[1], "");
                    }
                }

            }
            formulatext = formulatext.Replace("|", "");
            formulatext = formulatext.Replace("~", "");
            formulatext = formulatext.Replace("%", "/100*");

            return formulatext;
        }

        //public async Task<JSCLandRateViewModel> GetCalculatedPropertyTaxAmount(string propId, string year)
        //{
        //    var model = new JSCLandRateViewModel { Error = "" };
        //    var check = await _smartCityQueryBusiness.CheckPropertyTaxExist(propId, year);
        //    if (check != null)
        //    {
        //        model.Error = "Property tax for this " + propId + " property is already created for this year";
        //        return model;
        //    }
        //    else
        //    {
        //        var parcel = await _smartCityQueryBusiness.GetParcelByPropertyId(propId);
        //        //Unit Area Value(Rate) x Built-up area(sq.ft) x F1(Location Factor) x F2(Age Factor) x F4(Occupancy Factor) x Usage factor for commercial(Type)  property(F3)
        //        var landRate = await _smartCityQueryBusiness.GetLandRate(year, parcel.ward_no);
        //        //var unitareaD = await _smartCityQueryBusiness.GetUnitArea(year);
        //        //var locationfactor = await _smartCityQueryBusiness.GetLocationFactor(year);
        //        //var agefactorD = await _smartCityQueryBusiness.GetAgeFactor(year);
        //        //var occfactorD = await _smartCityQueryBusiness.GetOccupancyFactor(year);
        //        //var usagefactorD = await _smartCityQueryBusiness.GetUsageFactor(year);




        //        var formula = await _smartCityQueryBusiness.GetFormula();

        //        double sum = 0.0;
        //        double unitarea = 0.0;
        //        //double locfactor = 0.0;
        //        //double agefactor = 1.0;
        //        //double occfactor = 0.0;
        //        //double usagefactor = 0.0;
        //        double btup_area = 0.0;



        //        if (parcel.btup_ar_bm.IsNotNullAndNotEmptyAndNotValue("null") && parcel.type_bm.IsNotNullAndNotEmptyAndNotValue("null"))
        //        {
        //            var rate = landRate.FirstOrDefault(x => x.Code == parcel.type_bm);
        //            btup_area = double.Parse(parcel.btup_ar_bm);
        //            if (rate != null)
        //            {
        //                //sum = double.Parse(parcel.btup_ar_bm) * rate.Rate;        
        //                unitarea = rate.Rate;
        //            }
        //            else
        //            {
        //                model.Error = parcel.type_bm + " not available in land rate configuration";
        //                return model;
        //            }

        //        }
        //        if (parcel.btup_ar_gf.IsNotNullAndNotEmptyAndNotValue("null") && parcel.type_gf.IsNotNullAndNotEmptyAndNotValue("null"))
        //        {
        //            var rate = landRate.FirstOrDefault(x => x.Code == parcel.type_gf);
        //            btup_area += double.Parse(parcel.btup_ar_gf);
        //            if (rate != null)
        //            {
        //                //sum += double.Parse(parcel.btup_ar_gf) * rate.Rate;
        //                unitarea += rate.Rate;
        //            }
        //            else
        //            {
        //                model.Error = parcel.type_gf + " not available in land rate configuration";
        //                return model;
        //            }
        //        }
        //        if (parcel.btup_ar_ff.IsNotNullAndNotEmptyAndNotValue("null") && parcel.type_ff.IsNotNullAndNotEmptyAndNotValue("null"))
        //        {
        //            var rate = landRate.FirstOrDefault(x => x.Code == parcel.type_ff);
        //            btup_area += double.Parse(parcel.btup_ar_ff);
        //            if (rate != null)
        //            {
        //                // sum += double.Parse(parcel.btup_ar_ff) * rate.Rate;       
        //                unitarea += rate.Rate;
        //            }
        //            else
        //            {
        //                model.Error = parcel.type_ff + " not available in land rate configuration";
        //                return model;
        //            }

        //        }
        //        if (parcel.btup_ar_sf.IsNotNullAndNotEmptyAndNotValue("null") && parcel.type_sf.IsNotNullAndNotEmptyAndNotValue("null"))
        //        {
        //            var rate = landRate.FirstOrDefault(x => x.Code == parcel.type_sf);
        //            btup_area += double.Parse(parcel.btup_ar_sf);
        //            if (rate != null)
        //            {
        //                //sum += double.Parse(parcel.btup_ar_sf) * rate.Rate;   
        //                unitarea += rate.Rate;
        //            }
        //            else
        //            {
        //                model.Error = parcel.type_sf + " not available in land rate configuration";
        //                return model;
        //            }

        //        }
        //        if (parcel.btup_ar_tf.IsNotNullAndNotEmptyAndNotValue("null") && parcel.type_tf.IsNotNullAndNotEmptyAndNotValue("null"))
        //        {
        //            var rate = landRate.FirstOrDefault(x => x.Code == parcel.type_tf);
        //            btup_area += double.Parse(parcel.btup_ar_tf);
        //            if (rate != null)
        //            {
        //                // sum += double.Parse(parcel.btup_ar_tf) * rate.Rate;   
        //                unitarea += rate.Rate;
        //            }
        //            else
        //            {
        //                model.Error = parcel.type_tf + " not available in land rate configuration";
        //                return model;
        //            }

        //        }
        //        if (parcel.btup_ar_4f.IsNotNullAndNotEmptyAndNotValue("null") && parcel.type_4f.IsNotNullAndNotEmptyAndNotValue("null"))
        //        {
        //            var rate = landRate.FirstOrDefault(x => x.Code == parcel.type_4f);
        //            btup_area += double.Parse(parcel.btup_ar_4f);
        //            if (rate != null)
        //            {
        //                // sum += double.Parse(parcel.btup_ar_4f) * rate.Rate;   
        //                unitarea += rate.Rate;
        //            }
        //            else
        //            {
        //                model.Error = parcel.type_4f + " not available in land rate configuration";
        //                return model;
        //            }

        //        }
        //        if (parcel.btup_ar_5f.IsNotNullAndNotEmptyAndNotValue("null") && parcel.type_5f.IsNotNullAndNotEmptyAndNotValue("null"))
        //        {
        //            var rate = landRate.FirstOrDefault(x => x.Code == parcel.type_5f);
        //            btup_area += double.Parse(parcel.btup_ar_5f);
        //            if (rate != null)
        //            {
        //                //sum += double.Parse(parcel.btup_ar_5f) * rate.Rate;
        //                unitarea += rate.Rate;
        //            }
        //            else
        //            {
        //                model.Error = parcel.type_5f + " not available in land rate configuration";
        //                return model;
        //            }

        //        }
        //        if (parcel.btup_ar_6f.IsNotNullAndNotEmptyAndNotValue("null") && parcel.type_6f.IsNotNullAndNotEmptyAndNotValue("null"))
        //        {
        //            var rate = landRate.FirstOrDefault(x => x.Code == parcel.type_6f);
        //            btup_area += double.Parse(parcel.btup_ar_6f);
        //            if (rate != null)
        //            {
        //                //sum += double.Parse(parcel.btup_ar_6f) * rate.Rate;    
        //                unitarea += rate.Rate;
        //            }
        //            else
        //            {
        //                model.Error = parcel.type_6f + " not available in land rate configuration";
        //                return model;
        //            }
        //        }


        //        //var unitareaD = await GetUnitArea(year,sum,"",model);
        //        //var locationfactor = await GetLocationFactor(year,sum,"",model);
        //        //var agefactorD = await GetAgeFactor(year,sum, parcel.bld_age, model);
        //        //var occfactorD = await GetOccupancyFactor(year,sum,model,parcel);
        //        //var usagefactorD = await GetUsageFactor(year,sum,model,parcel);

        //        var age = 0.0;
        //        if (parcel.bld_age.IsNotNullAndNotEmptyAndNotValue("null"))
        //        {
        //            age = Convert.ToDouble(parcel.bld_age) - 5;
        //        }

        //        //var TotalAmount = unitarea * btup_area * locfactor * agefactor * occfactor * (usagefactor == 0.0 ? 1 : usagefactor);
        //        if (formula.FormulaText.IsNotNullAndNotEmpty())
        //        {
        //            var formulatext = formula.FormulaText;
        //            if (formulatext.Contains("~"))
        //            {
        //                var text = formulatext.Split("|");
        //                foreach (var item in text)
        //                {
        //                    if (item.Contains("~"))
        //                    {
        //                        var label = item.Split("~");
        //                        formulatext = formulatext.Replace(label[1], "");
        //                    }
        //                }

        //            }
        //            formulatext = formulatext.Replace("|", "");
        //            formulatext = formulatext.Replace("~", "");
        //            formulatext = formulatext.Replace("%", "/100");
        //            //formulatext = formulatext.Replace("LOCATION_FACTOR", locationfactor.ToString());
        //            //formulatext = formulatext.Replace("AGE_FACTOR", agefactorD.ToString());
        //            //formulatext = formulatext.Replace("OCCUPANCY_FACTOR", occfactorD.ToString());
        //            formulatext = formulatext.Replace("UNIT_AREA_VALUE", unitarea.ToString());
        //            formulatext = formulatext.Replace("BUILD_UP_AREA", btup_area.ToString());
        //            if (age >= 5)
        //            {
        //                var dep = age.ToString() + "/100";
        //                formulatext = formulatext.Replace("DEPRICIATION", dep);
        //            }
        //            else
        //            {
        //                formulatext = formulatext.Replace("DEPRICIATION", "0");
        //            }
        //            formulatext = formulatext.Replace("REBATE", "10/100");
        //            //formulatext = formulatext.Replace("USAGE_FACTOR_FOR_COMMERCIAL_PROPERTY", usagefactorD.ToString());

        //            DataTable dt = new DataTable();
        //            var v = dt.Compute(formulatext, "");

        //            model.Rate = Convert.ToDouble(v);
        //        }
        //        //model.Rate = TotalAmount;
        //        return model;
        //    }

        //}
        private async Task<double> GetLocationFactor(string year, double sum, string code, JSCLandRateViewModel model)
        {
            var factor = await _smartCityQueryBusiness.GetFormulaTypeByCode(year, "LOCATION_FACTOR");
            if (factor.Count > 0)
            {
                var factortype = factor.FirstOrDefault();
                if (factortype.PropertyColumnType == PropertyColumnTypeEnum.Code)
                {
                    var locationf = factor.FirstOrDefault(x => x.Code == code);
                    if (locationf != null)
                    {
                        return locationf.Value;
                    }
                    else
                    {
                        model.Error = " factor configuration not available";
                        return 0.0;
                    }
                }
                else
                {
                    var locationf = factor.FirstOrDefault(x => sum >= x.RangeFrom && sum <= x.RangeTo);
                    if (locationf != null)
                    {
                        return locationf.Value;
                    }
                    else
                    {
                        model.Error = " factor configuration not available";
                        return 0.0;
                    }
                }
            }
            model.Error = " factor configuration not available";
            return 0.0;
        }
        private async Task<double> GetAgeFactor(string year, double sum, string code, JSCLandRateViewModel model)
        {
            var factor = await _smartCityQueryBusiness.GetFormulaTypeByCode(year, "AGE_FACTOR");
            if (factor.Count > 0)
            {
                var factortype = factor.FirstOrDefault();
                if (factortype.PropertyColumnType == PropertyColumnTypeEnum.Code)
                {
                    var agef = factor.FirstOrDefault(x => x.Code == code);
                    if (agef != null)
                    {
                        return agef.Value;
                    }
                    else
                    {
                        model.Error = " factor configuration not available";
                        return 0.0;
                    }
                }
                else
                {
                    var agef = factor.FirstOrDefault(x => sum >= x.RangeFrom && sum <= x.RangeTo);
                    if (agef != null)
                    {
                        return agef.Value;
                    }
                    else
                    {
                        model.Error = " factor configuration not available";
                        return 0.0;
                    }
                }
            }
            model.Error = " factor configuration not available";
            return 0.0;
        }
        private async Task<double> GetOccupancyFactor(string year, double sum, JSCLandRateViewModel model, JSCParcelViewModel parcel)
        {
            var factor = await _smartCityQueryBusiness.GetFormulaTypeByCode(year, "OCCUPANCY_FACTOR");
            double occfactor = 0.0;
            if (factor.Count > 0)
            {
                var factortype = factor.FirstOrDefault();
                if (factortype.PropertyColumnType == PropertyColumnTypeEnum.Code)
                {
                    if (parcel.selft_bm.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = factor.FirstOrDefault(x => x.Code == parcel.selft_bm);
                        if (rate != null)
                        {
                            occfactor = rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.selft_bm + " not available in occupancy factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.selft_gf.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = factor.FirstOrDefault(x => x.Code == parcel.selft_gf);
                        if (rate != null)
                        {
                            occfactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.selft_gf + " not available in occupancy factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.occ_st_ff.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = factor.FirstOrDefault(x => x.Code == parcel.occ_st_ff);
                        if (rate != null)
                        {
                            occfactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.occ_st_ff + " not available in occupancy factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.occ_st_sf.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = factor.FirstOrDefault(x => x.Code == parcel.occ_st_sf);
                        if (rate != null)
                        {
                            occfactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.occ_st_sf + " not available in occupancy factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.occ_st_tf.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = factor.FirstOrDefault(x => x.Code == parcel.occ_st_tf);
                        if (rate != null)
                        {
                            occfactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.occ_st_tf + " not available in occupancy factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.occ_st_4f.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = factor.FirstOrDefault(x => x.Code == parcel.occ_st_4f);
                        if (rate != null)
                        {
                            occfactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.occ_st_4f + " not available in occupancy factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.occ_st_5f.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = factor.FirstOrDefault(x => x.Code == parcel.occ_st_5f);
                        if (rate != null)
                        {
                            occfactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.occ_st_5f + " not available in occupancy factor configuration";
                            return 0.0;
                        }

                    }
                    if (parcel.occ_st_6f.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = factor.FirstOrDefault(x => x.Code == parcel.occ_st_6f);
                        if (rate != null)
                        {
                            occfactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.occ_st_6f + " not available in occupancy factor configuration";
                            return 0.0;
                        }
                    }
                    return occfactor;
                }
                else
                {
                    var fact = factor.FirstOrDefault(x => sum >= x.RangeFrom && sum <= x.RangeTo);
                    if (fact != null)
                    {
                        return fact.Value;
                    }
                    else
                    {
                        model.Error = " factor configuration not available";
                        return 0.0;
                    }
                }
            }
            model.Error = " factor configuration not available";
            return 0.0;
        }
        private async Task<double> GetUnitArea(string year, double sum, string code, JSCLandRateViewModel model)
        {
            var factor = await _smartCityQueryBusiness.GetFormulaTypeByCode(year, "UNIT_AREA_VALUE");
            if (factor.Count > 0)
            {
                var factortype = factor.FirstOrDefault();
                if (factortype.PropertyColumnType == PropertyColumnTypeEnum.Code)
                {
                    var fact = factor.FirstOrDefault(x => x.Code == code);
                    if (fact != null)
                    {
                        return fact.Value;
                    }
                    else
                    {
                        model.Error = " factor configuration not available";
                        return 0.0;
                    }
                }
                else
                {
                    var fact = factor.FirstOrDefault(x => sum >= x.RangeFrom && sum <= x.RangeTo);
                    if (fact != null)
                    {
                        return fact.Value;
                    }
                    else
                    {
                        model.Error = " factor configuration not available";
                        return 0.0;
                    }
                }
            }
            model.Error = " factor configuration not available";
            return 0.0;
        }
        private async Task<double> GetUsageFactor(string year, double sum, JSCLandRateViewModel model, JSCParcelViewModel parcel)
        {
            var usagefactorD = await _smartCityQueryBusiness.GetFormulaTypeByCode(year, "USAGE_FACTOR_FOR_COMMERCIAL_PROPERTY");
            double usagefactor = 0.0;
            if (usagefactorD.Count > 0)
            {
                var factortype = usagefactorD.FirstOrDefault();
                if (factortype.PropertyColumnType == PropertyColumnTypeEnum.Code)
                {
                    if (parcel.ty_comm_bm.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = usagefactorD.FirstOrDefault(x => x.Code == parcel.ty_comm_bm);
                        if (rate != null)
                        {
                            usagefactor = rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.ty_comm_bm + " not available in usage factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.ty_comm_gf.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = usagefactorD.FirstOrDefault(x => x.Code == parcel.ty_comm_gf);
                        if (rate != null)
                        {
                            usagefactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.ty_comm_gf + " not available in usage factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.ty_comm_ff.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = usagefactorD.FirstOrDefault(x => x.Code == parcel.ty_comm_ff);
                        if (rate != null)
                        {
                            usagefactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.ty_comm_ff + " not available in usage factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.ty_comm_sf.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = usagefactorD.FirstOrDefault(x => x.Code == parcel.ty_comm_sf);
                        if (rate != null)
                        {
                            usagefactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.ty_comm_sf + " not available in usage factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.ty_comm_tf.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = usagefactorD.FirstOrDefault(x => x.Code == parcel.ty_comm_tf);
                        if (rate != null)
                        {
                            usagefactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.ty_comm_tf + " not available in usage factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.ty_comm_4f.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = usagefactorD.FirstOrDefault(x => x.Code == parcel.ty_comm_4f);
                        if (rate != null)
                        {
                            usagefactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.ty_comm_4f + " not available in usage factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.ty_comm_5f.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = usagefactorD.FirstOrDefault(x => x.Code == parcel.ty_comm_5f);
                        if (rate != null)
                        {
                            usagefactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.ty_comm_5f + " not available in usage factor configuration";
                            return 0.0;
                        }
                    }
                    if (parcel.ty_comm_6f.IsNotNullAndNotEmptyAndNotValue("null"))
                    {
                        var rate = usagefactorD.FirstOrDefault(x => x.Code == parcel.ty_comm_6f);
                        if (rate != null)
                        {
                            usagefactor += rate.Value;
                        }
                        else
                        {
                            model.Error = parcel.ty_comm_6f + " not available in usage factor configuration";
                            return 0.0;
                        }
                    }
                    return usagefactor;
                }
                else
                {
                    var fact = usagefactorD.FirstOrDefault(x => sum >= x.RangeFrom && sum <= x.RangeTo);
                    if (fact != null)
                    {
                        return fact.Value;
                    }
                    else
                    {
                        model.Error = " factor configuration not available";
                        return 0.0;
                    }
                }
            }
            model.Error = " factor configuration not available";
            return 0.0;
        }
        public async Task<List<JSCParcelViewModel>> GetJSCParcelListByUser(string userId)
        {
            var queryData = await _smartCityQueryBusiness.GetJSCParcelListByUser(userId);
            return queryData;
        }

        public async Task<JSCParcelViewModel> GetParcelDataByPclId(string id)
        {
            var result = await _smartCityQueryBusiness.GetParcelDataByPclId(id);
            return result;
        }
        public async Task UpdatePaymentDetails(dynamic udf, TaskTemplateViewModel viewModel)
        {
            var status = await _lOVBusiness.GetSingle(x => x.Code == "JSC_NOT_PAID");
            var mode = await _lOVBusiness.GetSingle(x => x.Code == "JSC_ONLINE");

            var list = await _smartCityQueryBusiness.GetJSCPaymentsList();

            var exist = list.Where(x => x.TaskId == viewModel.TaskId && x.ServiceId == viewModel.ParentServiceId).FirstOrDefault();

            if (!exist.IsNotNull())
            {
                try
                {
                    var paymentModel = new JSCPaymentViewModel()
                    {
                        TaskId = viewModel.TaskId,
                        ServiceId = viewModel.ParentServiceId,
                        PaymentSubject = viewModel.TaskSubject,
                        Amount = udf.Amount,
                        PaymentStatus = status.Id,
                        PaymentMode = mode.Id,
                        NoteOwnerUserId = viewModel.AssignedToUserId,
                        DueDate = viewModel.DueDate,
                        SourceReferenceId = udf.ParcelId != null ? udf.ParcelId : null,
                        RevenueTypeId = udf.RevenueTypeId != null ? udf.RevenueTypeId : null
                    };

                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = DataActionEnum.Create;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "JSC_PAYMENTS";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                    notemodel.Json = JsonConvert.SerializeObject(paymentModel);

                    var result = await _noteBusiness.ManageNote(notemodel);
                }
                catch (Exception e)
                {
                    throw;
                }

            }

        }

        public async Task<IList<JSCPaymentViewModel>> GetJSCPaymentsList(string portalIds = null, string userId = null)
        {
            var result = await _smartCityQueryBusiness.GetJSCPaymentsList(portalIds, userId);
            return result;
        }

        public async Task<IList<IdNameViewModel>> GetJSCSubLocalityList(string wardNo, string loc)
        {
            var result = await _smartCityQueryBusiness.GetJSCSubLocalityList(wardNo, loc);
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetJSCSubLocalityIdNameList()
        {
            var result = await _smartCityQueryBusiness.GetJSCSubLocalityIdNameList();
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetJSCLocalityList(string wardNo)
        {
            var result = await _smartCityQueryBusiness.GetJSCLocalityList(wardNo);
            return result;
        }
        public async Task<List<JSCParcelViewModel>> GetGrievanceReportGISBasedData(DateTime fromDate, DateTime toDate)
        {
            var result = await _smartCityQueryBusiness.GetGrievanceReportGISBasedData(fromDate, toDate);
            return result;
        }
        public async Task<List<JSCParcelViewModel>> GetGrievanceReportWardHeatMapData(DateTime fromDate, DateTime toDate, string departmentId)
        {
            var result = await _smartCityQueryBusiness.GetGrievanceReportWardHeatMapData(fromDate, toDate, departmentId);
            return result;
        }
        public async Task<List<JSCParcelViewModel>> GetJSCParcelDataForGarbageCollection(string wardId, string locality, string ddn, string autoId, DateTime? date)
        {
            var result = await _smartCityQueryBusiness.GetJSCParcelDataForGarbageCollection(wardId, locality, ddn, autoId, date);
            return result;
        }
        public async Task<bool> ManageGarbageCollection(string parcelIds, string userId, string latitude, string longitude, string garbageType)
        {
            var parcels = parcelIds.Split(",");
            foreach (var item in parcels)
            {
                var temp = await base.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_GARBAGE_COLLECTION");
                if (temp.IsNotNull())
                {
                    var formTemplate = await base.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == temp.Id);
                    formTemplate.TemplateCode = "JSC_GARBAGE_COLLECTION";
                    formTemplate.DataAction = DataActionEnum.Create;
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("ParcelId", item);
                    ((IDictionary<String, Object>)exo).Add("CollectionDateTime", DateTime.Now.ToDatabaseDateFormat());
                    ((IDictionary<String, Object>)exo).Add("CollectedByUserId", userId);
                    ((IDictionary<String, Object>)exo).Add("Latitude", latitude);
                    ((IDictionary<String, Object>)exo).Add("Longitude", longitude);
                    ((IDictionary<String, Object>)exo).Add("GarbageType", garbageType);
                    formTemplate.Json = JsonConvert.SerializeObject(exo);
                    await _cmsBusiness.ManageForm(formTemplate);

                }

            }
            //JSC_GARBAGE_COLLECTION
            //===========
            //ParcelId
            //CollectionDateTime
            //CollectedByUserId
            //throw new NotImplementedException();
            return true;
        }

        public async Task<bool> ManageSingleGarbageCollection(string parcelId, string userId, string latitude, string longitude)
        {
            var res = await _smartCityQueryBusiness.GetParcelByPropertyId(parcelId, userId);

            if (res.IsGarbageCollected)
            {
                return false;
            }
            else
            {
                var temp = await base.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_GARBAGE_COLLECTION");
                if (temp.IsNotNull())
                {
                    var formTemplate = await base.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == temp.Id);
                    formTemplate.TemplateCode = "JSC_GARBAGE_COLLECTION";
                    formTemplate.DataAction = DataActionEnum.Create;
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    formTemplate.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };

                    ((IDictionary<String, Object>)exo).Add("ParcelId", parcelId);
                    ((IDictionary<String, Object>)exo).Add("CollectionDateTime", DateTime.Now.ToDatabaseDateFormat());
                    ((IDictionary<String, Object>)exo).Add("CollectedByUserId", userId);
                    ((IDictionary<String, Object>)exo).Add("latitude", latitude);
                    ((IDictionary<String, Object>)exo).Add("Longitude", longitude);
                    formTemplate.Json = JsonConvert.SerializeObject(exo);
                    var result = await _cmsBusiness.ManageForm(formTemplate);

                    return result.IsSuccess;
                }
            }

            return true;

        }
        public async Task<double> GetJSCBinFeeAmount(DateTime bookingFromDate, DateTime bookingToDate, string binTypeId, string binSizeId, long binNumber)
        {
            var result = await _smartCityQueryBusiness.GetJSCBinFeeAmount(bookingFromDate, bookingToDate, binTypeId, binSizeId, binNumber);
            return result;
        }
        public async Task<CommandResult<OnlinePaymentViewModel>> UpdateOnlinePaymentDetailsJSC(OnlinePaymentViewModel model)
        {
            var result = await _smartCityQueryBusiness.GetOnlinePaymentDetailsJSC(model);

            //var result = await _EGovernanceQueryBusiness.UpdateOnlinePaymentDetails(model);

            if (result.IsNotNull() && result.PaymentStatusId.IsNotNullAndNotEmpty())
            {
                return CommandResult<OnlinePaymentViewModel>.Instance(model, false, "Your payment has been initiated already");
            }

            var userdetail = await _userBusiness.GetSingleById(model.UserId);
            if (userdetail == null)
            {
                return CommandResult<OnlinePaymentViewModel>.Instance(model, false, "User details is invalid. Please check with administrator");
            }
            var companySettings = await _repo.GetList<CompanySettingViewModel, CompanySetting>();
            //create viewmodel for all params and return this
            if (result != null)
            {
                model.Id = result.Id;
            }
            else
            {
                model.Id = Guid.NewGuid().ToString();
            }

            var date = DateTime.Now.ToDatabaseDateFormat();

            model.EmailId = userdetail.Email;
            model.MobileNumber = userdetail.Mobile;
            if (model.MobileNumber.IsNullOrEmpty())
            {
                model.MobileNumber = "NA";
            }
            if (companySettings != null && companySettings.Any())
            {
                model.MerchantID = companySettings.FirstOrDefault(x => x.Code == "PGWY_MERCHANT_ID")?.Value;
                model.CurrencyType = companySettings.FirstOrDefault(x => x.Code == "PGWY_CURRENCY_TYPE")?.Value;
                model.TypeField1 = companySettings.FirstOrDefault(x => x.Code == "PGWY_TYPE_FIELD_1")?.Value;
                model.SecurityID = companySettings.FirstOrDefault(x => x.Code == "PGWY_SECURITY_ID")?.Value;
                model.ChecksumKey = companySettings.FirstOrDefault(x => x.Code == "PGWY_CHECKSUM_KEY")?.Value;
                model.PaymentGatewayUrl = companySettings.FirstOrDefault(x => x.Code == "PGWY_GATEWAY_URL")?.Value;
                model.PaymentGatewayReturnUrl = companySettings.FirstOrDefault(x => x.Code == "JSC_PGWY_GATEWAY_RETURN_URL")?.Value;
                // model.PaymentGatewayReturnUrl = "https://localhost:44389/egov/egovernment/paymentresponse";
            }
            model.TypeField2 = "NA";
            model.Filler1 = "NA";
            model.AdditionalInfo1 = _userContext.UserId;
            model.AdditionalInfo2 = _userContext.PortalId;
            model.AdditionalInfo3 = "NA";
            model.AdditionalInfo4 = "NA";
            model.AdditionalInfo5 = "NA";
            model.AdditionalInfo6 = "NA";
            model.AdditionalInfo7 = "NA";
            model.Message = String.Concat(model.MerchantID, "|", model.Id, "|", model.Filler1, "|", model.Amount.ToString("#.00"), "|NA|NA|NA|", model.CurrencyType, "|NA|", model.TypeField1, "|", model.SecurityID, "|NA|NA|F|", model.MobileNumber, "|", model.EmailId, "|", model.AdditionalInfo1, "|", model.AdditionalInfo2, "|NA|NA|NA|", model.PaymentGatewayReturnUrl);

            model.ChecksumValue = await GenerateCheckSum(model.ChecksumKey, model.Message);

            model.RequestUrl = String.Concat(model.PaymentGatewayUrl, "?msg=", model.Message, "|", model.ChecksumValue);
            if (result.IsNotNull())
            {
                await UpdateOnlinePaymentJSC(model);
            }
            else
            {
                await _smartCityQueryBusiness.InsertOnlinePaymentDetailsDataJSC(model, date);
            }

            // return commandresult - if paymentstatus is having value then return message with payment initiated and status
            return CommandResult<OnlinePaymentViewModel>.Instance(model);

        }

        public async Task UpdateOnlinePaymentJSC(OnlinePaymentViewModel responseViewModel)
        {
            await _smartCityQueryBusiness.UpdateOnlinePaymentJSC(responseViewModel);
        }
        private async Task<string> GenerateCheckSum(string key, string text)
        {
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] hashValue;
            byte[] keybyt = encoder.GetBytes(key);
            byte[] message = encoder.GetBytes(text);

            var hashString = new HMACSHA256(keybyt);
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex.ToUpper();
        }
        public async Task<List<JSCParcelViewModel>> GetJSCAssetParcelListByUser(string userId)
        {
            var result = await _smartCityQueryBusiness.GetJSCAssetParcelListByUser(userId);
            return result;
        }
        public async Task<OnlinePaymentViewModel> GetOnlinePaymentJSC(string id)
        {
            var result = await _smartCityQueryBusiness.GetOnlinePaymentJSC(id);
            return result;
        }


        public async Task<JSCParcelViewModel> GetPropertyById(string propertyId)
        {
            var res = await _smartCityQueryBusiness.GetPropertyById(propertyId);
            return res;
        }

        public async Task<List<JSCPaymentViewModel>> GetPaymentDetailsByPropertyId(string gid)
        {
            var res = await _smartCityQueryBusiness.GetPaymentDetailsByPropertyId(gid);
            return res;
        }
        public async Task<List<JSCPaymentViewModel>> GetPaymentDetailsByServiceId(string serviceId)
        {
            var res = await _smartCityQueryBusiness.GetPaymentDetailsByServiceId(serviceId);
            return res;
        }
        public async Task UpdatePropertyTax(string paymentStatus, string reference, string id)
        {
            await _smartCityQueryBusiness.UpdatePropertyTax(paymentStatus, reference, id);
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDetailsByPropertyId(string gid)
        {
            var res = await _smartCityQueryBusiness.GetGarbageCollectionDetailsByPropertyId(gid);
            return res;
        }

        public async Task<List<JSCPaymentViewModel>> GetPaymentDetailsForConsumer(string mobileNo, string aadhar)
        {
            var res = await _smartCityQueryBusiness.GetPaymentDetailsForConsumer(mobileNo, aadhar);
            return res;
        }

        public async Task<List<JSCParcelViewModel>> GetPropertyDetailsForConsumer(string mobileNo, string aadhar)
        {
            var res = await _smartCityQueryBusiness.GetPropertyDetailsForConsumer(mobileNo, aadhar);
            return res;
        }

        public async Task<JSCParcelViewModel> GetParcelByMobileOrAadhar(string value)
        {
            var res = await _smartCityQueryBusiness.GetParcelByMobileOrAadhar(value);
            return res;
        }

        public async Task<JSCParcelViewModel> GetParcelByPropertyId(string propId, string userId = null, string latitude = null, string longitude = null)
        {
            var res = await _smartCityQueryBusiness.GetParcelByPropertyId(propId, userId);

            var rowData = JsonConvert.DeserializeObject(res.geometry);
            var coords = rowData["coordinates"];
            var lat = coords[0][0][0][1];
            var lng = coords[0][0][0][0];
            var sCoord = new GeoCoordinate(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            var eCoord = new GeoCoordinate(Convert.ToDouble(lat), Convert.ToDouble(lng));
            var dist = sCoord.GetDistanceTo(eCoord);

            res.IsSameLocation = dist > 50 ? false : true;
            return res;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTotalRevenue(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null)
        {
            var res = await _smartCityQueryBusiness.GetTotalRevenue(year, months, wards, assetTypes, revenueTypes);
            return res;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetRevenueByWard(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null)
        {
            var res = await _smartCityQueryBusiness.GetRevenueByWard(year, months, wards, assetTypes, revenueTypes);
            return res;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetRevenueByAssetType(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null)
        {
            var res = await _smartCityQueryBusiness.GetRevenueByAssetType(year, months, wards, assetTypes, revenueTypes);
            return res;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetRevenueByRevenueType(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null)
        {
            var res = await _smartCityQueryBusiness.GetRevenueByRevenueType(year, months, wards, assetTypes, revenueTypes);
            return res;
        }

        public async Task<IList<ServiceTemplateViewModel>> GetMyAllRequestList(bool showAllOwnersService, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string searchtext = null, DateTime? From = null, DateTime? To = null, string statusIds = null, string templateIds = null)
        {
            var result = await _smartCityQueryBusiness.GetMyAllRequestList(showAllOwnersService, moduleCodes, templateCodes, categoryCodes, searchtext, From, To, statusIds, templateIds);
            return result;
        }

        public async Task<IList<IdNameViewModel>> GetAssetTypeIdNameList()
        {
            var result = await _smartCityQueryBusiness.GetAssetTypeIdNameList();
            return result;
        }

        public async Task<List<PropertyTaxPaymentViewModel>> GetPropertyTaxPaymentDetails(string propNo = null, string taskNo = null)
        {
            var result = await _smartCityQueryBusiness.GetPropertyTaxPaymentDetails(propNo, taskNo);
            return result;

        }

        public async Task<IdNameViewModel> GetFinancialYearDetailsById(string year)
        {
            var result = await _smartCityQueryBusiness.GetFinancialYearDetailsById(year);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetParcelSearchByWard(string ward)
        {
            var result = await _smartCityQueryBusiness.GetParcelSearchByWard(ward);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetTransferStationList()
        {
            var result = await _smartCityQueryBusiness.GetTransferStationList();
            return result;
        }

        public async Task<JSCCollectorViewModel> GetCollectorDetailsByUserId(string userId)
        {
            var result = await _smartCityQueryBusiness.GetCollectorDetailsByUserId(userId);
            return result;
        }

        public async Task<bool> ManageGarbageCollectorProperty(JSCCollectorPropertyViewModel model)
        {
            var exist = await _cmsBusiness.GetDataListByTemplate("GARBAGE_COLLECTOR_PROPERTY", "", $@" and ""F_JSC_REV_GarbageCollectorProperty"".""ParcelId"" = '{model.ParcelId}'");

            if (exist.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                var temp = await base.GetSingle<TemplateViewModel, Template>(x => x.Code == "GARBAGE_COLLECTOR_PROPERTY");
                if (temp.IsNotNull())
                {
                    var garbageType = await _lOVBusiness.GetSingle(x => x.Code == model.GarbageTypeId);
                    model.GarbageTypeId = garbageType.Id;

                    var formTemplate = await base.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == temp.Id);
                    formTemplate.TemplateCode = "GARBAGE_COLLECTOR_PROPERTY";
                    formTemplate.DataAction = DataActionEnum.Create;
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    formTemplate.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };

                    //((IDictionary<String, Object>)exo).Add("CollectorId", model.CollectorId);
                    //((IDictionary<String, Object>)exo).Add("ParcelId", model.ParcelId);
                    //((IDictionary<String, Object>)exo).Add("GarbageTypeId", model.GarbageTypeId);
                    formTemplate.Json = JsonConvert.SerializeObject(model);
                    var result = await _cmsBusiness.ManageForm(formTemplate);
                }
                return true;
            }

        }

        public async Task<JSCParcelViewModel> IsParcelIdValid(string parcelId)
        {
            var data = await _smartCityQueryBusiness.IsParcelIdValid(parcelId);
            return data;
        }
        public async Task<JSCGrievanceWorkflow> GetGrievanceWorkflowById(string Id)
        {
            var data = await _smartCityQueryBusiness.GetGrievanceWorkflowById(Id);
            return data;
        }
        public async Task<JSCFormulaViewModel> GetFormulaById(string Id)
        {
            var data = await _smartCityQueryBusiness.GetFormulaById(Id);
            return data;
        }
        public async Task<List<JSCFormulaViewModel>> GetFormulaList(string type)
        {
            var data = await _smartCityQueryBusiness.GetFormulaList(type);
            return data;
        }

        public async Task<JSCFormulaViewModel> GetLatestFormula()
        {
            var data = await _smartCityQueryBusiness.GetLatestFormula();
            return data;
        }
        public async Task<List<IdNameViewModel>> GetFormulaType()
        {
            var data = await _smartCityQueryBusiness.GetFormulaType();
            return data;
        }
        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDetailsByUserId(string userId, DateTime? date = null, string mobileNo = null, string userName = null, string ddnNo = null)
        {
            var data = await _smartCityQueryBusiness.GetGarbageCollectionDetailsByUserId(userId, date, mobileNo, userName, ddnNo);
            return data;
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDetailsByCitizen(string userId, DateTime? date = null, string ddnNo = null)
        {
            var data = await _smartCityQueryBusiness.GetGarbageCollectionDetailsByCitizen(userId, date, ddnNo);
            return data;
        }

        public async Task UpdateCollectorUserId(string id, string userId)
        {
            await _smartCityQueryBusiness.UpdateCollectorUserId(id, userId);
        }
        public async Task UpdateDriverUserId(string id, string userId)
        {
            await _smartCityQueryBusiness.UpdateDriverUserId(id, userId);
        }

        public async Task UpdateComplaintOperatorUserId(string id, string userId)
        {
            await _smartCityQueryBusiness.UpdateComplaintOperatorUserId(id, userId);
        }
        public async Task UpdateTransferStationUserId(string id, string userId)
        {
            await _smartCityQueryBusiness.UpdateTransferStationUserId(id, userId);
        }
        public async Task UpdateWTPUserId(string id, string userId)
        {
            await _smartCityQueryBusiness.UpdateWTPUserId(id, userId);
        }

        public async Task UpdateSubLoginUserId(string id, string userId)
        {
            await _smartCityQueryBusiness.UpdateSubLoginUserId(id, userId);
        }
        public async Task<List<JSCParcelViewModel>> GetJSCParcelDataForGarbageCollectionByWard(string wardId = null)
        {
            var result = await _smartCityQueryBusiness.GetJSCParcelDataForGarbageCollectionByWard(wardId);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetGrievanceTypeByDepartment(string department)
        {
            var result = await _smartCityQueryBusiness.GetGrievanceTypeByDepartment(department);
            return result;
        }

        //public async Task<JSCCollectorViewModel> GetCollectorDetailsByUserId(string userId)
        //{
        //    var result = await _smartCityQueryBusiness.GetCollectorDetailsByUserId(userId);
        //    return result;
        //}
        public async Task<List<JSCGarbageCollectionViewModel>> GetAllGarbageCollectionData(string autoNo, string wardNo, string collector, DateTime? collectionDate)
        {
            var res = await _smartCityQueryBusiness.GetAllGarbageCollectionData(autoNo, wardNo, collector, collectionDate);
            return res;
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetUserDoorToDoorGarbageCollectionData(string userId, string garbageType, DateTime? collectionDate = null)
        {
            var res = await _smartCityQueryBusiness.GetUserDoorToDoorGarbageCollectionData(userId, garbageType, collectionDate);
            return res;
        }

        public async Task<JSCGrievanceWorkflow> GetGrievanceWorkflow(string ward, string dept)
        {
            var model = await _smartCityQueryBusiness.GetGrievanceWorkflow(ward, dept);

            //var where = $@" and ""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"".""WardId""='{ward}' and ""F_JSC_GRIEVANCE_MGMT_GRIEVANCE_WORKFLOW"".""DepartmentId""='{dept}' ";
            //var data = await _cmsBusiness.GetDataListByTemplate("JSC_GRIEVANCE_WORKFLOW", "", where);

            //var model = new JSCGrievanceWorkflow();

            //foreach (DataRow d in data.Rows)
            //{
            //    var json = JsonConvert.SerializeObject(d);
            //    model = JsonConvert.DeserializeObject<JSCGrievanceWorkflow>(json);
            //    model.Id = Convert.ToString(d["Id"]);
            //    model.WorkflowLevelId = Convert.ToString(d["WorkflowLevelId"]);
            //    model.DepartmentId = Convert.ToString(d["DepartmentId"]);
            //    model.WardId = Convert.ToString(d["WardId"]);
            //    model.Level1AssignedToTypeId = Convert.ToString(d["Level1AssignedToTypeId"]);
            //    model.Level2AssignedToTypeId = Convert.ToString(d["Level2AssignedToTypeId"]);
            //    model.Level3AssignedToTypeId = Convert.ToString(d["Level3AssignedToTypeId"]);
            //    model.Level4AssignedToTypeId = Convert.ToString(d["Level4AssignedToTypeId"]);
            //    model.Level1AssignedToTeamId = Convert.ToString(d["Level1AssignedToTeamId"]);
            //    model.Level2AssignedToTeamId = Convert.ToString(d["Level2AssignedToTeamId"]);
            //    model.Level3AssignedToTeamId = Convert.ToString(d["Level3AssignedToTeamId"]);
            //    model.Level4AssignedToTeamId = Convert.ToString(d["Level4AssignedToTeamId"]);
            //    model.Level1AssignedToUserId = Convert.ToString(d["Level1AssignedToUserId"]);
            //    model.Level2AssignedToUserId = Convert.ToString(d["Level2AssignedToUserId"]);
            //    model.Level3AssignedToUserId = Convert.ToString(d["Level3AssignedToUserId"]);
            //    model.Level4AssignedToUserId = Convert.ToString(d["Level4AssignedToUserId"]);
            //}
            return model;
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> CreateLodgeComplaintService(string ddnNo, string userId)
        {
            var deptId = "";
            var grvTypeId = "";

            var raiseComplaint = await _smartCityQueryBusiness.GetGarbageCollectionCompaintCountandUpdate(ddnNo, DateTime.Now);
            if (raiseComplaint)
            {

                var parcel = await _smartCityQueryBusiness.GetParcelByDDNNO(ddnNo);
                var dept = await _cmsBusiness.GetDataListByTemplate("JSC_Department", "", $@" and ""F_JSC_GRIEVANCE_MGMT_Department"".""Code""='SA' ");
                var grvType = await _cmsBusiness.GetDataListByTemplate("JSC_GRIEVANCE_TYPE", "", $@" and ""F_JSC_GRIEVANCE_MGMT_GrievanceType"".""Code""='DOOR_TO_DOOR_GARBAGE_NOT_COLLECTED' ");

                foreach (DataRow d in dept.Rows)
                {
                    deptId = Convert.ToString(d["Id"]);
                }
                foreach (DataRow d in grvType.Rows)
                {
                    grvTypeId = Convert.ToString(d["Id"]);
                }

                if (parcel.IsNotNull())
                {
                    ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                    serviceModel.ActiveUserId = userId;
                    serviceModel.TemplateCode = "JSC_LODGECOMPLAINT";
                    var service = await _serviceBusiness.GetServiceDetails(serviceModel);

                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("Ward", parcel.ward_no);
                    ((IDictionary<String, Object>)exo).Add("Department", deptId);
                    ((IDictionary<String, Object>)exo).Add("GrievanceType", grvTypeId);
                    ((IDictionary<String, Object>)exo).Add("Name", "Complaint logged from mobile");
                    ((IDictionary<String, Object>)exo).Add("Details", "Complaint logged from mobile");
                    ((IDictionary<String, Object>)exo).Add("radio", "DDN");
                    ((IDictionary<String, Object>)exo).Add("DDN", ddnNo);
                    ((IDictionary<String, Object>)exo).Add("EventDate", DateTime.Now.Date);

                    service.OwnerUserId = userId;
                    service.StartDate = DateTime.Now;
                    service.ActiveUserId = userId;
                    service.DataAction = DataActionEnum.Create;
                    service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                    service.Json = JsonConvert.SerializeObject(exo);

                    var res = await _serviceBusiness.ManageService(service);

                    return res;
                }
            }
            else
            {
                return CommandResult<ServiceTemplateViewModel>.Instance(new ServiceTemplateViewModel(), true, "Complaint Report in Progress.");
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(new ServiceTemplateViewModel(), false, "Invalid DDN No");
        }

        public async Task<JSCParcelViewModel> GetParcelByDDNNO(string ddnNo)
        {
            var data = await _smartCityQueryBusiness.GetParcelByDDNNO(ddnNo);
            return data;
        }

        public async Task<List<IdNameViewModel>> GetViolationData()
        {
            var data = await _smartCityQueryBusiness.GetViolationData();
            return data;
        }
        public async Task<List<JSCParcelViewModel>> GetPropertiesByDDNNO(string ddnNo)
        {
            var data = await _smartCityQueryBusiness.GetPropertiesByDDNNO(ddnNo);
            return data;
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> RegisterNewAsset(JSCAssetViewModel model)
        {
            var property = await _smartCityQueryBusiness.GetParcelByDDNNO(model.DDNNO);

            if (property.tel_no.IsNullOrEmpty())
            {
                return CommandResult<ServiceTemplateViewModel>.Instance(model, false, "Mobile number not registered with this property, please contact administrator");
            }

            var userdata = await _userBusiness.GetSingle(x => x.Mobile == property.tel_no);
            string useremail = "";

            if (userdata.IsNotNull())
            {
                useremail = userdata.Email;
            }
            else
            {
                useremail = property.e_mail_id;
                var usermodel = new UserViewModel()
                {
                    Name = property.own_name,
                    Email = property.e_mail_id,
                    Mobile = property.tel_no,
                    Password = "!Welcome123",
                    ConfirmPassword = "!Welcome123",
                };

                var userres = await _userBusiness.Create(usermodel);
            }

            var otpresult = await GenerateOtp(useremail);

            if (otpresult)
            {
                ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                serviceModel.ActiveUserId = _userContext.UserId;
                serviceModel.TemplateCode = "JSC_REGISTER_ASSET";
                var service = await _serviceBusiness.GetServiceDetails(serviceModel);

                dynamic exo = new System.Dynamic.ExpandoObject();

                ((IDictionary<String, Object>)exo).Add("ParcelId", property.gid);
                ((IDictionary<String, Object>)exo).Add("RegisteredDate", DateTime.Now);
                ((IDictionary<String, Object>)exo).Add("UserId", _userContext.UserId);
                //((IDictionary<String, Object>)exo).Add("Otp", otp);

                service.OwnerUserId = _userContext.UserId;
                service.StartDate = DateTime.Now;
                service.ActiveUserId = _userContext.UserId;
                service.DataAction = DataActionEnum.Create;
                service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                service.Json = JsonConvert.SerializeObject(exo);

                var res = await _serviceBusiness.ManageService(service);

                return CommandResult<ServiceTemplateViewModel>.Instance(res.Item, res.IsSuccess, res.Messages);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(model, otpresult, "Try again after some time");
        }

        private async Task<bool> GenerateOtp(string email)
        {
            var user = await _userBusiness.TwoFactorAuthOTP(email);
            var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "TWO_FACTOR_OTP");
            if (notificationTemplateModel.IsNotNull())
            {
                var notificationViewModel = new NotificationViewModel
                {
                    To = user.Email,
                    ToUserId = user.Id,
                    SendAlways = true,
                    NotifyBySms = true,
                    NotifyByEmail = true,
                    Subject = notificationTemplateModel.Subject,
                    Body = notificationTemplateModel.Body.Replace("{{EMAIL_OTP}}", user.TwoFactorAuthOTP)
                };
                await _notificationBusiness.Create(notificationViewModel);
                return true;
            }
            return false;
        }

        public async Task<List<JSCParcelViewModel>> GetJSCRegisteredAssetsList(string userId)
        {
            var data = await _smartCityQueryBusiness.GetJSCRegisteredAssetsList(userId);
            return data;
        }

        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportData(GrievanceDatefilters datefilters, string startDate, string endDate)
        {
            var data = await _smartCityQueryBusiness.GetGrievanceReportData(datefilters, startDate, endDate);
            return data;
        }
        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportTurnaroundTimeData(string department, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityQueryBusiness.GetGrievanceReportTurnaroundTimeData(department, fromDate, toDate);
            return data;
        }
        public async Task<List<JSCComplaintViewModel>> GetJSCMyComplaint()
        {
            var data = await _smartCityQueryBusiness.GetJSCMyComplaint();
            return data;
        }

        public async Task<IList<JSCComplaintViewModel>> GetComplaintslist(string templateCodes, string userId)
        {
            var data = await _smartCityQueryBusiness.GetComplaintslist(templateCodes, userId);
            return data;
        }
        public async Task<List<BBPSRegisterViewModel>> GetBBPSRegisterList(string serviceType)
        {
            var data = await _smartCityQueryBusiness.GetBBPSRegisterList(serviceType);
            return data;
        }

        public async Task<IList<IdNameViewModel>> GetJSCDepartmentList()
        {
            var data = await _smartCityQueryBusiness.GetJSCDepartmentList();
            return data;
        }
        public async Task<IList<IdNameViewModel>> GetJSCRevenueTypeList()
        {
            var data = await _smartCityQueryBusiness.GetJSCRevenueTypeList();
            return data;
        }
        public async Task<IList<IdNameViewModel>> GetJSCGrievanceTypeList()
        {
            var data = await _smartCityQueryBusiness.GetJSCGrievanceTypeList();
            return data;
        }

        public async Task<JSCComplaintViewModel> GetJSCMyComplaintById(string serviceId)
        {
            var data = await _smartCityQueryBusiness.GetJSCMyComplaintById(serviceId);
            return data;
        }

        public async Task<List<JSCComplaintViewModel>> GetJSCComplaintByDDN(string ddn)
        {
            var data = await _smartCityQueryBusiness.GetJSCComplaintByDDN(ddn);
            return data;
        }
        public async Task<List<JSCComplaintViewModel>> GetJSCComplaintForResolver(bool isAdmin, bool isUpperLevel)
        {
            var data = await _smartCityQueryBusiness.GetJSCComplaintForResolver(isAdmin, isUpperLevel);
            return data;
        }
        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportComplaintListData(string wardId, string departmentId, string complaintTypeId, string statusCode, DateTime fromDate, DateTime toDate, string complaintNo)
        {
            var data = await _smartCityQueryBusiness.GetGrievanceReportComplaintListData(wardId, departmentId, complaintTypeId, statusCode, fromDate, toDate, complaintNo);
            return data;
        }
        public async Task<IList<JSCComplaintViewModel>> UpdateResolverInput(string id, string status, string documentId)
        {
            var data = await _smartCityQueryBusiness.UpdateResolverInput(id, status, documentId);
            var statusdetails = await _lOVBusiness.GetSingleById(status);
            if (statusdetails.IsNotNull())
            {
                if (statusdetails.Code == "GRV_DISPOSED")
                {
                    var service = await _serviceBusiness.GetSingle(x => x.UdfNoteTableId == id);
                    if (service.IsNotNull())
                    {
                        ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                        serviceModel.ServiceId = service.Id;
                        var servicedata = await _serviceBusiness.GetServiceDetails(serviceModel);
                        servicedata.DataAction = DataActionEnum.Edit;
                        servicedata.ServiceStatusCode = "SERVICE_STATUS_COMPLETE";
                        var res = await _serviceBusiness.ManageService(servicedata);

                    }
                }

            }
            return data;
        }

        public async Task<IList<JSCComplaintViewModel>> ComplaintMarkFlag(string id)
        {
            var data = await _smartCityQueryBusiness.ComplaintMarkFlag(id);
            return data;
        }

        public async Task<IList<JSCComplaintViewModel>> ReopenComplaint(string parentId, string documents)
        {
            var data = await _smartCityQueryBusiness.ReopenComplaint(parentId, documents);
            //var service = await _serviceBusiness.GetSingle(x => x.UdfNoteTableId == parentId);
            //if (service.IsNotNull())
            //{
            //    ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            //    serviceModel.ServiceId = service.Id;
            //    var servicedata = await _serviceBusiness.GetServiceDetails(serviceModel);
            //    servicedata.DataAction = DataActionEnum.Edit;
            //    servicedata.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
            //    servicedata.CompletedDate = null;
            //    var res = await _serviceBusiness.ManageService(servicedata);
            //}
            return data;
        }

        public async Task<IList<JSCComplaintViewModel>> GetReopenComplaintDetails(string parentId)
        {
            var data = await _smartCityQueryBusiness.GetReopenComplaintDetails(parentId);
            return data;
        }

        public async Task<List<NtsServiceCommentViewModel>> GetAllGrievanceComment(string serviceId, bool isLevelUser)
        {
            var list = new List<NtsServiceCommentViewModel>();
            var replylist = new List<NtsServiceCommentViewModel>();

            var result = await _smartCityQueryBusiness.GetAllGrievanceComment(serviceId);
            list.AddRange(result);

            foreach (var p in list)
            {

                var result1 = await _smartCityQueryBusiness.GetAllGrievanceComment1(p.id);
                replylist.AddRange(result1);


            }
            list.AddRange(replylist);
            if (!isLevelUser)
            {
                list = list.Where(x => x.JobTitle == "Level 1").ToList();
            }
            return list;
        }
        public async Task<List<JSCCommunityHallViewModel>> GetJSCCommunityHallIdNameList(string wardId)
        {
            var data = await _smartCityQueryBusiness.GetJSCCommunityHallIdNameList(wardId);
            return data;
        }
        public async Task<List<IdNameViewModel>> GetJSCFunctionTypeIdNameList()
        {
            var data = await _smartCityQueryBusiness.GetJSCFunctionTypeIdNameList();
            return data;
        }
        public async Task<JSCCommunityHallViewModel> GetJSCCommunityHallDetailsById(string communityHallId)
        {
            var data = await _smartCityQueryBusiness.GetJSCCommunityHallDetailsById(communityHallId);
            return data;
        }
        public async Task<JSCCommunityHallViewModel> GetJSCCommunityHallPhotos(string communityHallId)
        {
            var data = await _smartCityQueryBusiness.GetJSCCommunityHallPhotos(communityHallId);
            return data;
        }
        public async Task<List<JSCCommunityHallViewModel>> GetJSCCommunityHallServiceChildData(string parentId)
        {
            var data = await _smartCityQueryBusiness.GetJSCCommunityHallServiceChildData(parentId);
            return data;
        }
        public async Task<List<JSCCommunityHallViewModel>> SearchJSCCommunityHallList(string communityHallId, string wardId)
        {
            var data = await _smartCityQueryBusiness.SearchJSCCommunityHallList(communityHallId,wardId);
            return data;
        }
        public async Task<List<JSCCommunityHallViewModel>> GetCommunityHallList(string type, DateTime? st = null, DateTime? en = null, string[] dates = null)
        {
            var data = await _smartCityQueryBusiness.GetCommunityHallList(type, st, en, dates);
            return data;
        }

        public async Task<JSCParcelViewModel> CheckIfDDNExist(string ddn)
        {
            var data = await _smartCityQueryBusiness.CheckIfDDNExist(ddn);
            return data;
        }

        public async Task<IList<JSCComplaintViewModel>> GetFlagComplaintDetails(string parentId)
        {
            var data = await _smartCityQueryBusiness.GetFlagComplaintDetails(parentId);
            return data;
        }

        public async Task<bool> UpdateDepartmentByOperator(string id, string departmentId, string grievanceTypeId)
        {
            var data = await _smartCityQueryBusiness.UpdateDepartmentByOperator(id, departmentId, grievanceTypeId);
            return data;
        }

        public async Task<bool> MarkDisposedByOperator(string id)
        {
            var data = await _smartCityQueryBusiness.MarkDisposedByOperator(id);
            return data;
        }

        public async Task<JSCSanitationReportViewModel> GetMSWAutoDetails(string id)
        {
            var data = await _smartCityQueryBusiness.GetMSWAutoDetails(id);
            return data;
        }
        public async Task<JSCSanitationReportViewModel> GetBWGAutoDetails(string id)
        {
            var data = await _smartCityQueryBusiness.GetBWGAutoDetails(id);
            return data;
        }
        public async Task<List<JSCSanitationReportViewModel>> GetMSWReportDetails(string autoId, DateTime startDate, DateTime endDate, string transferStationId)
        {
            var data = await _smartCityQueryBusiness.GetMSWReportDetails(autoId, startDate, endDate, transferStationId);
            return data;
        }
        public async Task<List<JSCSanitationReportViewModel>> GetBWGReportDetails(string autoId, DateTime startDate, DateTime endDate, string transferStationId)
        {
            var data = await _smartCityQueryBusiness.GetBWGReportDetails(autoId, startDate, endDate, transferStationId);
            return data;
        }
        public async Task<List<IdNameViewModel>> GetJSCAutoList()
        {
            var data = await _smartCityQueryBusiness.GetJSCAutoList();
            return data;
        }

        public async Task<List<IdNameViewModel>> GetJSCAutoListByTransferStation(string transferStationId)
        {
            var data = await _smartCityQueryBusiness.GetJSCAutoListByTransferStation(transferStationId);
            return data;
        }

        public async Task<List<JSCAutoViewModel>> GetAutoListByUserId(string userId)
        {
            var data = await _smartCityQueryBusiness.GetAutoListByUserId(userId);
            return data;
        }

        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportDeptWardData()
        {
            var data = await _smartCityQueryBusiness.GetComplaintByWardAndDepartmentWithStatus();
            return data;
        }
        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportDeptTurnaroundTimeData(string typeCode, string departmentId, string wardId, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityQueryBusiness.GetGrievanceReportDeptTurnaroundTimeData(typeCode, departmentId, wardId, fromDate, toDate);
            return data;
        }
        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportAgingData(string typeCode, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityQueryBusiness.GetGrievanceReportAgingData(typeCode, fromDate, toDate);
            return data;
        }
        public async Task<List<JSCComplaintViewModel>> GetGrievanceReportDepartmentWiseData(string typeCode, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityQueryBusiness.GetGrievanceReportDepartmentWiseData(typeCode, fromDate, toDate);
            return data;
        }

        public async Task<List<JSCComplaintViewModel>> GetComplaintZoneStatusData(string zone, string status, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityQueryBusiness.GetComplaintZoneStatusData(zone, status, fromDate, toDate);
            return data;
        }
        public async Task<List<JSCComplaintViewModel>> GetComplaintWardDepartmentStatusData(string warddept, string status, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityQueryBusiness.GetComplaintWardDepartmentStatusData(warddept, status, fromDate, toDate);
            return data;
        }
        public async Task<List<JSCComplaintViewModel>> GetComplaintByWardAndDepartmentWithStatusDetails(string department, string status)
        {
            var data = await _smartCityQueryBusiness.GetComplaintByWardAndDepartmentWithStatusDetails(department, status);
            return data;
        }

        public async Task<List<JSCComplaintViewModel>> GetComplaintReportData(string name, string reportType, DateTime fromDate, DateTime toDate)
        {
            var data = await _smartCityQueryBusiness.GetComplaintReportData(name, reportType, fromDate, toDate);
            return data;
        }

        public async Task<List<JSCCollectorViewModel>> GetCollectorWithWardByCollectorId(string collectorId)
        {
            var data = await _smartCityQueryBusiness.GetCollectorWithWardByCollectorId(collectorId);
            return data;
        }
        public async Task<List<JSCParcelViewModel>> GetGISDataByAutoWise(string autoId, DateTime? date)
        {
            var data = await _smartCityQueryBusiness.GetGISDataByAutoWise(autoId, date);
            return data;
        }
        public async Task<List<IdNameViewModel>> GetCollectorListByWard(string wardId)
        {
            var data = await _smartCityQueryBusiness.GetCollectorListByWard(wardId);
            return data;
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectedAndNotCollectedList()
        {
            var data = await _smartCityQueryBusiness.GetGarbageCollectedAndNotCollectedList();
            return data;
        }
        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectedAndNotCollectedListByWard(string wardId)
        {
            var data = await _smartCityQueryBusiness.GetGarbageCollectedAndNotCollectedListByWard(wardId);
            return data;
        }
        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDateByPropertyType()
        {
            var data = await _smartCityQueryBusiness.GetGarbageCollectionDateByPropertyType();
            return data;
        }
        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDateByPropertyTypeAndWard(string wardId)
        {
            var data = await _smartCityQueryBusiness.GetGarbageCollectionDateByPropertyTypeAndWard(wardId);
            return data;
        }
        public async Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgs()
        {
            var data = await _smartCityQueryBusiness.GetGarbageWetAndDryWasteInKgs();
            return data;
        }
        public async Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgsByWard(string wardId)
        {
            var data = await _smartCityQueryBusiness.GetGarbageWetAndDryWasteInKgsByWard(wardId);
            return data;
        }
        public async Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgsByPropertyType()
        {
            var data = await _smartCityQueryBusiness.GetGarbageWetAndDryWasteInKgsByPropertyType();
            return data;
        }
        public async Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgsByPropertyTypeByWard(string wardId)
        {
            var data = await _smartCityQueryBusiness.GetGarbageWetAndDryWasteInKgsByPropertyTypeByWard(wardId);
            return data;
        }

        public async Task<List<JSCPropertyRegistrationViewModel>> GetPropertyRegistrationStatusWise()
        {
            var data = await _smartCityQueryBusiness.GetPropertyRegistrationStatusWise();
            return data;
        }
        public async Task<List<JSCParcelViewModel>> GetParcelForPropertyTaxCal()
        {
            var data = await _smartCityQueryBusiness.GetParcelForPropertyTaxCal();
            return data;
        }

        public async Task<CommandResult<JSCDailyBasedActivityViewModel>> ManageDailyBasedActivity(JSCDailyBasedActivityViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var points = await _cmsBusiness.GetDataListByTemplate("JSC_POINT_VEHICLE_MAPPING", "", $@" and ""F_JSC_REV_PointAndVehicleMapping"".""PointName"" = '{model.PointName}' ");

                if (points.Rows.Count > 0)
                {
                    var sCoord = new GeoCoordinate();
                    foreach (DataRow dr in points.Rows)
                    {
                        sCoord = new GeoCoordinate(Convert.ToDouble(dr["Latitude"]), Convert.ToDouble(dr["Longitude"]));
                    }

                    var eCoord = new GeoCoordinate(Convert.ToDouble(model.Latitude), Convert.ToDouble(model.Longitude));
                    var dist = sCoord.GetDistanceTo(eCoord);
                    if (dist < 20)
                    {
                        var temp = await base.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_GVP_DAILY_BASE_ACTIVITY");
                        if (temp.IsNotNull())
                        {
                            var formTemplate = await base.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == temp.Id);
                            formTemplate.TemplateCode = "JSC_GVP_DAILY_BASE_ACTIVITY";
                            formTemplate.DataAction = DataActionEnum.Create;
                            dynamic exo = new System.Dynamic.ExpandoObject();
                            formTemplate.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };

                            formTemplate.Json = JsonConvert.SerializeObject(model);
                            var result = await _cmsBusiness.ManageForm(formTemplate);
                            return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, result.IsSuccess, result.Messages);
                        }
                    }
                    else
                    {
                        return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, false, "Invalid Location");
                    }
                }
                else
                {
                    return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, false, "Given Point Name is not valid");
                }
            }
            else
            {
                var exist = await _cmsBusiness.GetDataListByTemplate("JSC_GVP_DAILY_BASE_ACTIVITY", "", $@" and ""F_JSC_REV_GVP_Daily_Based_Activity"".""Id"" = '{model.Id}'");

                if (exist.Rows.Count > 0)
                {
                    var temp = await base.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_GVP_DAILY_BASE_ACTIVITY");
                    if (temp.IsNotNull())
                    {
                        var formTemplate = await base.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == temp.Id);
                        formTemplate.TemplateCode = "JSC_GVP_DAILY_BASE_ACTIVITY";
                        formTemplate.DataAction = DataActionEnum.Edit;
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        formTemplate.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };

                        formTemplate.Json = JsonConvert.SerializeObject(model);
                        formTemplate.RecordId = model.Id;

                        var result = await _cmsBusiness.ManageForm(formTemplate);

                        return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, result.IsSuccess, result.Messages);
                    }
                }

                return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, false, "Record Not Found");
            }
            return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, false, "Record Not Found");
        }

        public async Task<List<JSCDailyBasedActivityViewModel>> GetJSCGVPData(DateTime? date)
        {
            var data = await _cmsBusiness.GetDataListByTemplate("JSC_GVP_DAILY_BASE_ACTIVITY", "");
            List<JSCDailyBasedActivityViewModel> list = new List<JSCDailyBasedActivityViewModel>();
            list = (from DataRow dr in data.Rows
                    select new JSCDailyBasedActivityViewModel()
                    {
                        Id = dr["Id"] != DBNull.Value ? dr["Id"].ToString() : "",
                        PointName = dr["PointName"] != DBNull.Value ? dr["PointName"].ToString() : "",
                        VehicleId = dr["VehicleId"] != DBNull.Value ? dr["VehicleId"].ToString() : "",
                        InitialPicture = dr["InitialPicture"] != DBNull.Value ? dr["InitialPicture"].ToString() : "",
                        AfterCleaningPicture = dr["AfterCleaningPicture"] != DBNull.Value ? dr["AfterCleaningPicture"].ToString() : "",
                        Latitude = dr["Latitude"] != DBNull.Value ? Convert.ToDouble(dr["Latitude"]) : 0,
                        Longitude = dr["Longitude"] != DBNull.Value ? Convert.ToDouble(dr["Longitude"]) : 0,
                        CollectionStartDateTime = dr["CreatedDate"] != DBNull.Value ? dr["CreatedDate"].ToString() : "",
                        CollectionEndDateTime = dr["LastUpdatedDate"] != DBNull.Value ? dr["LastUpdatedDate"].ToString() : ""
                    }).ToList();


            list.ToList().ForEach(cc => cc.CollectionEndDateTime = cc.CollectionEndDateTime.ToSafeDateTime().ToString("yyyy-MM-dd hh:mm:ss"));
            list.ToList().ForEach(cc => cc.CollectionStartDateTime = cc.CollectionStartDateTime.ToSafeDateTime().ToString("yyyy-MM-dd hh:mm:ss"));

            //foreach (var i in list)
            //{
            //    i.CollectionEndDateTime = i.CollectionEndDateTime.ToSafeDateTime().ToString("yyyy-MM-dd");
            //    i.CollectionStartDateTime = i.CollectionStartDateTime.ToSafeDateTime().ToString("yyyy-MM-dd");
            //}
            if (date.IsNotNull())
            {
                list = list.Where(x => x.CollectionStartDateTime.ToSafeDateTime().Date == date.ToDefaultDateFormat().ToSafeDateTime().Date).ToList();
            }
            return list;
        }

        public async Task<bool> ManageRefuseCompactor(JSCRefuseCompactorViewModel model)
        {
            var dustbin = await _cmsBusiness.GetDataListByTemplate("JSC_DUSTBIN", "", $@" and ""F_JSC_REV_Dustbin"".""Id""='{model.DustbinId}' ");
            var sCoord = new GeoCoordinate();
            foreach (DataRow dr in dustbin.Rows)
            {
                sCoord = new GeoCoordinate(Convert.ToDouble(dr["Latitude"]), Convert.ToDouble(dr["Longitude"]));
            }

            var eCoord = new GeoCoordinate(Convert.ToDouble(model.Latitude), Convert.ToDouble(model.Longitude));
            var dist = sCoord.GetDistanceTo(eCoord);
            if (dist < 20)
            {
                var temp = await base.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_REFUSE_COMPACTOR");
                if (temp.IsNotNull())
                {
                    var exist = await _cmsBusiness.GetDataListByTemplate("JSC_REFUSE_COMPACTOR", "", $@" and ""F_JSC_REV_JSCRefuseCompactor"".""DustbinId""='{model.DustbinId}' and ""F_JSC_REV_JSCRefuseCompactor"".""CollectionDateTime""::Date='{model.CollectionDateTime}'::Date ");
                    if (exist.Rows.Count > 0)
                    {
                        return false;
                    }
                    else
                    {
                        var formTemplate = await base.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == temp.Id);
                        formTemplate.TemplateCode = "JSC_REFUSE_COMPACTOR";
                        formTemplate.DataAction = DataActionEnum.Create;
                        formTemplate.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };

                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("DustbinId", model.DustbinId);
                        ((IDictionary<String, Object>)exo).Add("CollectionDateTime", model.CollectionDateTime);
                        ((IDictionary<String, Object>)exo).Add("Latitude", model.Latitude);
                        ((IDictionary<String, Object>)exo).Add("Longitude", model.Longitude);

                        formTemplate.Json = JsonConvert.SerializeObject(exo);
                        var result = await _cmsBusiness.ManageForm(formTemplate);
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }

        }

        public async Task<List<JSCRefuseCompactorViewModel>> GetRefuseCompactorData(DateTime? date=null)
        {
            var data = await _cmsBusiness.GetDataListByTemplate("JSC_REFUSE_COMPACTOR", "");
            List<JSCRefuseCompactorViewModel> list = new List<JSCRefuseCompactorViewModel>();
            list = (from DataRow dr in data.Rows
                    select new JSCRefuseCompactorViewModel()
                    {
                        Id = dr["Id"].ToString(),
                        DustbinId = dr["DustbinId"].ToString(),
                        Latitude = Convert.ToDouble(dr["Latitude"]),
                        Longitude = Convert.ToDouble(dr["Longitude"]),
                        CollectionDateTime = Convert.ToDateTime(dr["CollectionDateTime"]),
                    }).ToList();

            foreach (var a in list)
            {
                var data1 = await _cmsBusiness.GetDataListByTemplate("JSC_DUSTBIN", "", $@" and ""F_JSC_REV_Dustbin"".""Id""='{a.DustbinId}' ");
                a.DustbinName = data1.IsNotNull() ? data1.Rows[0]["DustbinID"].ToString() : "";
            }
            if (date.IsNotNull())
            {
                list = list.Where(x => x.CollectionDateTime.Value.Date == date).ToList();
            }

            return list;
        }

        public async Task<List<IdNameViewModel>> GetPointList(string vehicleId)
        {
            var data = await _smartCityQueryBusiness.GetPointsListByVehicle(vehicleId);
            return data;
        }

        public async Task<string> GetVehicleIdForLoggedInUser(string userId)
        {
            var data = await _smartCityQueryBusiness.GetVehicleIdForLoggedInUser(userId);
            return data;
        }
        public async Task<string> GetVehicleTypeForLoggedInUser(string userId)
        {
            var data = await _smartCityQueryBusiness.GetVehicleTypeForLoggedInUser(userId);
            return data;
        }

        public async Task<List<IdNameViewModel>> GetOutwardVehicleList(DateTime date)
        {
            var data = await _smartCityQueryBusiness.GetOutwardVehicleList(date);
            return data;
        }



        public async Task<List<JSCAssessmentViewModel>> GetViewAssessmentByDDNNO()
        {
            var data = await _smartCityQueryBusiness.GetViewAssessmentByDDNNO();
            return data;
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetJSCVehicleDetails(string vehicleId, DateTime? startDate, DateTime? endDate)
        {
            var data = await _smartCityQueryBusiness.GetJSCVehicleDetails(vehicleId, startDate, endDate);
            return data;
        }

        public async Task<List<PropertyTaxPaymentReceiptViewModel>> GetPropertyTaxPaymentReceiptByDDN(string DDNNO)    
        {
            var data = await _smartCityQueryBusiness.GetPropertyTaxPaymentReceiptByDDN(DDNNO);
            return data;
        }
        public async Task<PropertyTaxPaymentReceiptViewModel> GetPropertyTaxPaymentReceiptByReceiptId(string ReceiptId)   
        {
            var data = await _smartCityQueryBusiness.GetPropertyTaxPaymentReceiptByReceiptId(ReceiptId);
            return data;
        }

        public async Task<List<IdNameViewModel>> GetDustbinData()
        {
            var data = await _cmsBusiness.GetDataListByTemplate("JSC_DUSTBIN", "");
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            list = (from DataRow dr in data.Rows
                    select new IdNameViewModel()
                    {
                        Id = dr["Id"].ToString(),
                        Name = dr["DustbinID"].ToString()
                    }).ToList();
            return list;
        }

        public async Task<IdNameViewModel> GetTransferStationDetails(string transferStationId)
        {
            var model = new List<IdNameViewModel>();
            var details = new DataTable();
            if (transferStationId.IsNotNull())
            {
                details = await _cmsBusiness.GetDataListByTemplate("JSC_TRANSFER_STATION", "", $@" and ""F_JSC_REV_TransferStation"".""Id"" = '{transferStationId}'");
            }
            else
            {
                details = await _cmsBusiness.GetDataListByTemplate("JSC_TRANSFER_STATION", "", $@" and ""F_JSC_REV_TransferStation"".""UserId"" = '{_userContext.UserId}'");

            }
            model = (from DataRow dr in details.Rows
                     select new IdNameViewModel()
                     {
                         Id = dr["Name"].ToString(),
                         Name = dr["PersonName"].ToString(),
                         Code = dr["Location"].ToString(),
                     }).ToList();
            return model.FirstOrDefault();

        }

        public async Task<CommandResult<JSCDailyBasedActivityViewModel>> MapPointAndVehicle(JSCDailyBasedActivityViewModel model)
        {

            if (model.DataAction == DataActionEnum.Create)
            {
                var exist = await _cmsBusiness.GetDataListByTemplate("JSC_POINT_VEHICLE_MAPPING", "", $@" and ""F_JSC_REV_PointAndVehicleMapping"".""VehicleId"" = '{model.VehicleId}' and ""F_JSC_REV_PointAndVehicleMapping"".""PointName"" = '{model.PointName}'");
                if (exist.Rows.Count > 0)
                {
                    return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, false, "Given Point Name already mapped with selected Vehicle");
                }
                else
                {
                    var temp = await base.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_POINT_VEHICLE_MAPPING");
                    if (temp.IsNotNull())
                    {
                        var formTemplate = await base.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == temp.Id);
                        formTemplate.TemplateCode = "JSC_POINT_VEHICLE_MAPPING";
                        formTemplate.DataAction = DataActionEnum.Create;
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        formTemplate.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };

                        formTemplate.Json = JsonConvert.SerializeObject(model);
                        var result = await _cmsBusiness.ManageForm(formTemplate);
                        return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, result.IsSuccess, result.Messages);
                    }
                }
                return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, false, "");
            }
            else
            {
                var exist = await _cmsBusiness.GetDataListByTemplate("JSC_POINT_VEHICLE_MAPPING", "", $@" and ""F_JSC_REV_PointAndVehicleMapping"".""Id"" = '{model.Id}'");

                if (exist.Rows.Count > 0)
                {
                    var temp = await base.GetSingle<TemplateViewModel, Template>(x => x.Code == "JSC_GVP_DAILY_BASE_ACTIVITY");
                    if (temp.IsNotNull())
                    {
                        var formTemplate = await base.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == temp.Id);
                        formTemplate.TemplateCode = "JSC_POINT_VEHICLE_MAPPING";
                        formTemplate.DataAction = DataActionEnum.Edit;
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        formTemplate.Page = new PageViewModel { Template = new Template { TableMetadataId = temp.TableMetadataId } };

                        formTemplate.Json = JsonConvert.SerializeObject(model);
                        formTemplate.RecordId = model.Id;

                        var result = await _cmsBusiness.ManageForm(formTemplate);

                        return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, result.IsSuccess, result.Messages);
                    }
                }

                return CommandResult<JSCDailyBasedActivityViewModel>.Instance(model, false, "");
            }
        }

        public async Task<List<JSCDailyBasedActivityViewModel>> GetPointAndVehicleMappingData()
        {
            var data = await _smartCityQueryBusiness.GetPointAndVehicleMappingData();
            return data;
        }
        public async Task<List<JSCParcelViewModel>> GetViewPrpertyMapByDdnNoAndUser()
        {
            var data = await _smartCityQueryBusiness.GetViewPrpertyMapByDdnNoAndUser();
            return data;
        }
         public async Task<List<JSCParcelViewModel>> GetViewPrpertyForSelfAssessment()
        {
            var data= await _smartCityQueryBusiness.GetViewPrpertyForSelfAssessment();
            return data;
        }
        public async Task<List<JSCParcelViewModel>> GetAddPropertyExist(string ddnNo)
        {
            var data = await _smartCityQueryBusiness.GetAddPropertyExist(ddnNo);
            return data;
        }

        public async Task<JSCGarbageCollectionViewModel> GetBWGCollection()
        {
            var data = await _smartCityQueryBusiness.GetBWGCollection();
            return data;
        }

        public async Task<List<JSCInwardOutwardReportViewModel>> GetJSCOutwardReport(DateTime? date)
        {
            var data = await _smartCityQueryBusiness.GetJSCOutwardReport(date);
            return data;
        }

        public async Task<List<JSCInwardOutwardReportViewModel>> GetJSCInwardReport(DateTime? date)
        {
            var data = await _smartCityQueryBusiness.GetJSCInwardReport(date);
            return data;
        }

        public async Task<List<JSCSanitationReportViewModel>> GetBWGDetailsByUserId(string userId)
        {
            var data = await _smartCityQueryBusiness.GetBWGDetailsByUserId(userId);
            return data;
        }

        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDateByDate(DateTime? date)
        {
            var data = await _smartCityQueryBusiness.GetGarbageCollectionDateByDate(date);
            return data;
        }

        public async Task<CommandResult<JSCEnforcementUnAuthorizationViewModel>> InsertEnforcementUnAuthorization(JSCEnforcementUnAuthorizationViewModel model) 
        {
            await _smartCityQueryBusiness.InsertEnforcementUnAuthorization(model);
            return CommandResult<JSCEnforcementUnAuthorizationViewModel>.Instance(model); ;
        }

        public async Task<List<IdNameViewModel>> GetUnauthorizedCaseList()
        {
            var result = await _smartCityQueryBusiness.GetUnauthorizedCaseList();
            return result;
        }

        public async Task<List<IdNameViewModel>> GetEnforcementViolations()
        {
            var result = await _smartCityQueryBusiness.GetEnforcementViolations();
            return result;
        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetJSCUnauthorizedViolationsDetail(DateTime? date,string Ward, string UserId)
        {
            var data = await _smartCityQueryBusiness.GetJSCUnauthorizedViolationsDetail(date,Ward,UserId);
            return data;
        }
        
        public async Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDataByWardAndDate(DateTime? date, string ward)
        {
            var data = await _smartCityQueryBusiness.GetGarbageCollectionDataByWardAndDate(date, ward);
            return data;
        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetAuthorizationList()
        {
            var data = await _smartCityQueryBusiness.GetAuthorizationList();
            return data;
        }
        public async Task<CommandResult<JSCEnforcementUnAuthorizationViewModel>> InsertEnforcementAuthorization(JSCEnforcementUnAuthorizationViewModel model)
        {
            await _smartCityQueryBusiness.InsertEnforcementAuthorization(model);
            return CommandResult<JSCEnforcementUnAuthorizationViewModel>.Instance(model); ;
        }

        public async Task<List<IdNameViewModel>> GetAuthorizedCaseList()
        {
            var result = await _smartCityQueryBusiness.GetAuthorizedCaseList();
            return result;
        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetJSCAuthorizedViolationsDetail(DateTime? date, string Ward,string UserId)
        {
            var data = await _smartCityQueryBusiness.GetJSCAuthorizedViolationsDetail(date, Ward,UserId);
            return data;
        }

        public async Task<List<JSCEnforcementSubLoginViewModel>> GetSubLogin(string loginType)
        {
            var data = await _smartCityQueryBusiness.GetSubLogin(loginType);
            return data;
        }

        public async Task<IList<UserViewModel>> GetUserListForSubLogin()
        {
            var data = await _smartCityQueryBusiness.GetUserListForSubLogin();
            return data;
        }

        public async Task<List<PropertyTaxPaymentReceiptViewModel>> GetPropertyPaymentDetails()
        {
            var data = await _smartCityQueryBusiness.GetPropertyPaymentDetails();
            return data;
        }
        public async Task<JSCPropertyTaxInstallmentViewModel> GetPropertyPaymentDetailById(string id)
        {
            var data = await _smartCityQueryBusiness.GetPropertyPaymentDetailById(id);
            return data;
        } 
        public async Task<string> GetNextPropertyReceiptNumber()
        {
            var data = await _smartCityQueryBusiness.GetNextPropertyReceiptNumber();
            return data;
        }

        public async Task<string> GetWardListByUser(string userId)
        {
            var data = await _smartCityQueryBusiness.GetWardListByUser(userId);
            return data;
        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementAuthorization(string userId)
        {
            var data = await _smartCityQueryBusiness.GetEnforcementAuthorization(userId);
            return data;

        }
        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementUnAuthorization(string userId)
        {
            var data = await _smartCityQueryBusiness.GetEnforcementUnAuthorization(userId);
            return data;

        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementAuthorizationWeeklyReport()
        {
            var data = await _smartCityQueryBusiness.GetEnforcementAuthorizationWeeklyReport();
            return data;

        }
        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetJSCOBPSAuthorizedDetail(DateTime? date, string Ward)
        {
            var data = await _smartCityQueryBusiness.GetJSCOBPSAuthorizedDetail(date, Ward);
            return data;
        }
        public async Task<CommandResult<JSCEnforcementUnAuthorizationViewModel>> InsertEnforcementAuthorizedWeeklyReport(JSCEnforcementUnAuthorizationViewModel model)
        {
            await _smartCityQueryBusiness.InsertEnforcementAuthorizedWeeklyReport(model);
            return CommandResult<JSCEnforcementUnAuthorizationViewModel>.Instance(model); ;
        }

        public async Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementSubloginMappinglist()
        {
            var data = await _smartCityQueryBusiness.GetEnforcementSubloginMappinglist();
            return data;
        }
    }
}
