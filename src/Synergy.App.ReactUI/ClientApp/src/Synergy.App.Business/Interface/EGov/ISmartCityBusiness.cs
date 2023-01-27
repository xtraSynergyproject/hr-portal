using Microsoft.AspNetCore.Mvc;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Synergy.App.ViewModel.EGOV;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ISmartCityBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<JSCAssetConsumerViewModel> ReadJSCAssetConsumerData(string consumerId);
        Task<List<TreeViewViewModel>> GetJSCMapViewTreeList();
        Task<List<JSCGrievanceWorkflow>> GetGrievanceWorkflowList();
        Task<IList<JSCAssetConsumerViewModel>> GetAssetCountByWard(string wardId = null, string collectorId = null, string revType = null);
        Task<IList<JSCAssetConsumerViewModel>> GetAssetAllotmentCountByWard(string wardId = null, string collectorId = null, string revType = null);
        Task<List<TreeViewViewModel>> GetAssetMapViewTreeList();
        Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByWard(string wardId = null, string collectorId = null);
        Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByAssetType(string wardId = null, string collectorId = null);
        Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByCollector(string wardId = null, string collectorId = null);
        Task<IList<JMCAssetPaymentViewModel>> GetAssetPaymentCountByPaymentStatus(string wardId = null, string collectorId = null);
        Task<IdNameViewModel> GetAssetTypeForJammuById(string assetTypeId);
        Task<IdNameViewModel> GetWardForJammuById(string wardId);
        Task<JSCAssetViewModel> GetJSCAssetDetailsById(string assetId);
        Task<JSCBillPaymentReportViewModel> GetJSCBillPaymentDetails(string serviceId);
        Task<PropertyTaxPaymentViewModel> GetPropertyTaxReportData(string ddno, string year);
        Task<List<PropertyTaxPaymentViewModel>> GetPropertyTaxReportFloorData(string ddno, string year);
        Task<IdNameViewModel> GetBuildingCategory(string buildingType);
        Task<List<JSCPropertyFloorViewModel>> GetAssessmentFloorData(string assessmentId);
        Task<JSCPropertySelfAssessmentViewModel> GetSelfAssessmentData(string assessmentId);
        Task GenerateAssetBillPayment();
        Task<string> GetJSCConsumerUserId(string consumerId);
        Task<List<JSCColonyViewModel>> GetJSCColonyMapViewList();
        Task<JMCAssetViewModel> GetAssetByServiceNo(string serviceNo);
        Task<JSCAssetConsumerViewModel> GetConsumerByConsumerNo(string consumerNo);
        Task<JMCAssetViewModel> GetAssetById(string id);
        Task<List<JSCAssetConsumerViewModel>> GetAssetConsumerData(string assetId);
        Task<List<JSCAssetConsumerViewModel>> GetAssetPaymentData(string assetId);
        Task<List<JSCAssetConsumerViewModel>> GetConsumerPaymentData(string consumerId);
        Task<List<JMCAssetViewModel>> GetConsumerAssetData(string consumerId);
        Task<JSCAssetConsumerViewModel> GetConsumerById(string id);
        Task<List<JSCParcelViewModel>> GetJSCParcelMapViewList(string colName, string colText);
        Task<List<IdNameViewModel>> GetParcelColumnList();
        Task<List<IdNameViewModel>> GetColonyList();
        Task<List<IdNameViewModel>> GetParcelTypeList();
        Task<List<IdNameViewModel>> GetWardList();
        Task<List<IdNameViewModel>> GetJSCZoneList();
        Task<List<IdNameViewModel>> GetJSCZoneListByDepartment(string departmentId);
        Task<List<IdNameViewModel>> GetParcelIdNameList();
        Task<List<IdNameViewModel>> GetBinCollectorNameList();
        Task<JSCCollectorViewModel> GetBinCollectorMobile(string userId);
        Task<List<IdNameViewModel>> GetJSCOwnerList();

        Task<List<JSCParcelViewModel>> GetParcelSearchByWardandType(string ward, string parcelType);
        Task<bool> ValidateStartDateandEndDate(ServiceTemplateViewModel viewModel);
        Task<List<JSCParcelViewModel>> GetJSCWardMapViewList(string wardNo);
        Task GenerateRevenueCollectionBillForJammu();
        Task<List<JSCParcelViewModel>> GetJSCParcelListByUser(string userId);
        Task<JSCParcelViewModel> GetParcelDataByPclId(string id);
        Task UpdatePaymentDetails(dynamic udf, TaskTemplateViewModel viewModel);
        Task<IList<IdNameViewModel>> GetJSCSubLocalityList(string wardNo, string loc);
        Task<IList<IdNameViewModel>> GetJSCSubLocalityIdNameList();
        Task<IList<IdNameViewModel>> GetJSCLocalityList(string wardNo);
        Task<List<JSCParcelViewModel>> GetGrievanceReportGISBasedData(DateTime fromDate, DateTime toDate);
        Task<List<JSCParcelViewModel>> GetGrievanceReportWardHeatMapData(DateTime fromDate, DateTime toDate, string departmentId);
        Task<List<JSCParcelViewModel>> GetJSCParcelDataForGarbageCollection(string wardId = null, string locality = null, string ddn = null, string autoId = null, DateTime? date = null);
        Task<IList<JSCPaymentViewModel>> GetJSCPaymentsList(string portalIds = null, string userId = null);
        Task<bool> ManageGarbageCollection(string parcelIds, string userId, string latitude, string longitude, string garbageType);

        Task<bool> ManageSingleGarbageCollection(string parcelId, string userId, string latitude, string longitude);
        Task<double> GetJSCBinFeeAmount(DateTime bookingFromDate, DateTime bookingToDate, string binTypeId, string binSizeId, long binNumber);
        Task<CommandResult<OnlinePaymentViewModel>> UpdateOnlinePaymentDetailsJSC(OnlinePaymentViewModel model);
        Task UpdateOnlinePaymentJSC(OnlinePaymentViewModel responseViewModel);
        Task<List<JSCParcelViewModel>> GetJSCAssetParcelListByUser(string userId);
        Task<OnlinePaymentViewModel> GetOnlinePaymentJSC(string id);
        Task<JSCParcelViewModel> GetPropertyById(string propertyId);
        Task<List<JSCPaymentViewModel>> GetPaymentDetailsByPropertyId(string gid);
        Task<List<JSCPaymentViewModel>> GetPaymentDetailsByServiceId(string serviceId);
        Task UpdatePropertyTax(string paymentStatus, string reference, string id);
        Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDetailsByPropertyId(string gid);
        Task<List<JSCPaymentViewModel>> GetPaymentDetailsForConsumer(string mobileNo, string aadhar);
        Task<List<JSCParcelViewModel>> GetPropertyDetailsForConsumer(string mobileNo, string aadhar);
        Task<JSCParcelViewModel> GetParcelByMobileOrAadhar(string value);
        Task<JSCParcelViewModel> GetParcelByPropertyId(string propId, string userId = null, string latitude = null, string longitude = null);
        Task<List<ProjectDashboardChartViewModel>> GetTotalRevenue(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null);
        Task<List<ProjectDashboardChartViewModel>> GetRevenueByWard(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null);
        Task<List<ProjectDashboardChartViewModel>> GetRevenueByAssetType(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null);
        Task<List<ProjectDashboardChartViewModel>> GetRevenueByRevenueType(int? year, string months = null, string wards = null, string assetTypes = null, string revenueTypes = null);
        Task<IList<ServiceTemplateViewModel>> GetMyAllRequestList(bool showAllOwnersService, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string searchtext = null, DateTime? From = null, DateTime? To = null, string statusIds = null, string templateIds = null);
        Task<JSCPropertySelfAssessmentViewModel> GetCalculatedPropertyTaxAmount(JSCPropertySelfAssessmentViewModel model, string assmentId);
        Task<IList<IdNameViewModel>> GetAssetTypeIdNameList();
        Task<List<PropertyTaxPaymentViewModel>> GetPropertyTaxPaymentDetails(string propNo = null, string taskNo = null);
        Task<IdNameViewModel> GetFinancialYearDetailsById(string year);
        Task<List<IdNameViewModel>> GetTransferStationList();
        Task<List<IdNameViewModel>> GetParcelSearchByWard(string ward);
        Task<JSCCollectorViewModel> GetCollectorDetailsByUserId(string userId);
        Task<bool> ManageGarbageCollectorProperty(JSCCollectorPropertyViewModel model);

        Task<JSCParcelViewModel> IsParcelIdValid(string parcelId);
        Task<JSCGrievanceWorkflow> GetGrievanceWorkflowById(string Id);
        Task<JSCFormulaViewModel> GetFormulaById(string Id);
        Task<List<JSCFormulaViewModel>> GetFormulaList(string type);
        Task<List<IdNameViewModel>> GetFormulaType();
        Task<JSCFormulaViewModel> GetLatestFormula();
        Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDetailsByUserId(string userId, DateTime? date = null, string mobileNo = null, string userName = null, string ddnNo = null);
        Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDetailsByCitizen(string userId, DateTime? date = null, string ddnNo = null);
        //Task<List<JSCGarbageCollection>> GetGarbageCollectionDetailsByUserId(string userId, DateTime? date = null);
        Task UpdateCollectorUserId(string id, string userId);
        Task UpdateDriverUserId(string id, string userId);
        Task UpdateComplaintOperatorUserId(string id, string userId);
        Task UpdateTransferStationUserId(string id, string userId);
        Task<List<JSCParcelViewModel>> GetJSCParcelDataForGarbageCollectionByWard(string wardId = null);
        Task<List<JSCGarbageCollectionViewModel>> GetAllGarbageCollectionData(string autoNo, string wardNo, string collector, DateTime? collectionDate);
        Task<List<IdNameViewModel>> GetGrievanceTypeByDepartment(string department);
        Task<List<JSCGarbageCollectionViewModel>> GetUserDoorToDoorGarbageCollectionData(string userId, string garbageType, DateTime? collectionDate = null);
        Task<List<JSCComplaintViewModel>> GetGrievanceReportData(GrievanceDatefilters datefilters, string startDate, string endDate);
        Task<List<JSCComplaintViewModel>> GetGrievanceReportTurnaroundTimeData(string department, DateTime fromDate, DateTime toDate);
        Task<JSCGrievanceWorkflow> GetGrievanceWorkflow(string ward, string dept);
        Task<JSCParcelViewModel> GetParcelByDDNNO(string ddnNo);
        Task<List<JSCParcelViewModel>> GetPropertiesByDDNNO(string ddnNo);

        Task<List<IdNameViewModel>> GetViolationData();
        Task<List<JSCParcelViewModel>> GetJSCRegisteredAssetsList(string userId);
        Task<CommandResult<ServiceTemplateViewModel>> RegisterNewAsset(JSCAssetViewModel model);
        Task<CommandResult<ServiceTemplateViewModel>> CreateLodgeComplaintService(string ddnNo, string userId);
        Task<List<JSCComplaintViewModel>> GetJSCMyComplaint();
        Task<IList<JSCComplaintViewModel>> GetComplaintslist(string templateCodes, string userId);
        Task<List<BBPSRegisterViewModel>> GetBBPSRegisterList(string serviceType);
        Task<IList<IdNameViewModel>> GetJSCDepartmentList();
        Task<IList<IdNameViewModel>> GetJSCRevenueTypeList();
        Task<IList<IdNameViewModel>> GetJSCGrievanceTypeList();
        Task<List<JSCCommunityHallViewModel>> GetJSCCommunityHallIdNameList(string wardId);
        Task<List<IdNameViewModel>> GetJSCFunctionTypeIdNameList();
        Task<JSCCommunityHallViewModel> GetJSCCommunityHallDetailsById(string communityHallId);
        Task<JSCCommunityHallViewModel> GetJSCCommunityHallPhotos(string communityHallId);
        Task<List<JSCCommunityHallViewModel>> GetJSCCommunityHallServiceChildData(string parentId);
        Task<List<JSCCommunityHallViewModel>> SearchJSCCommunityHallList(string communityHallId, string wardId);
        Task<List<JSCCommunityHallViewModel>> GetCommunityHallList(string type, DateTime? st = null, DateTime? en = null, string[] dates = null);
        Task<JSCComplaintViewModel> GetJSCMyComplaintById(string serviceId);
        Task<List<JSCComplaintViewModel>> GetJSCComplaintByDDN(string ddn);
        Task<List<JSCComplaintViewModel>> GetJSCComplaintForResolver(bool isAdmin, bool IsUpperLevel);
        Task<List<JSCComplaintViewModel>> GetGrievanceReportComplaintListData(string wardId, string departmentId, string complaintTypeId, string statusCode, DateTime fromDate, DateTime toDate, string complaintNo);
        Task<IList<JSCComplaintViewModel>> UpdateResolverInput(string id, string status, string documentId);

        Task<IList<JSCComplaintViewModel>> ComplaintMarkFlag(string id);
        Task<IList<JSCComplaintViewModel>> ReopenComplaint(string parentId, string documents);
        Task<IList<JSCComplaintViewModel>> GetReopenComplaintDetails(string parentId);
        Task<List<NtsServiceCommentViewModel>> GetAllGrievanceComment(string serviceId, bool isLevelUser);
        Task<JSCParcelViewModel> CheckIfDDNExist(string ddn);
        Task<IList<JSCComplaintViewModel>> GetFlagComplaintDetails(string parentId);
        Task<bool> UpdateDepartmentByOperator(string id, string department, string grievanceTypeId);
        Task<bool> MarkDisposedByOperator(string id);
        Task<JSCSanitationReportViewModel> GetMSWAutoDetails(string id);
        Task<JSCSanitationReportViewModel> GetBWGAutoDetails(string id);
        Task<List<IdNameViewModel>> GetJSCAutoListByTransferStation(string transferStationId);
        Task<List<JSCSanitationReportViewModel>> GetMSWReportDetails(string autoId, DateTime startDate, DateTime endDate, string transferStationId);
        Task<List<JSCSanitationReportViewModel>> GetBWGReportDetails(string autoId, DateTime startDate, DateTime endDate, string transferStationId);
        Task<List<IdNameViewModel>> GetJSCAutoList();
        Task<List<JSCAutoViewModel>> GetAutoListByUserId(string userId);
        Task<List<JSCComplaintViewModel>> GetGrievanceReportDeptWardData();
        Task<List<JSCComplaintViewModel>> GetGrievanceReportDeptTurnaroundTimeData(string typeCode, string departmentId, string wardId, DateTime fromDate, DateTime toDate);
        Task<List<JSCComplaintViewModel>> GetGrievanceReportAgingData(string typeCode, DateTime fromDate, DateTime toDate);
        Task<List<JSCComplaintViewModel>> GetGrievanceReportDepartmentWiseData(string typeCode, DateTime fromDate, DateTime toDate);
        Task<List<JSCComplaintViewModel>> GetComplaintZoneStatusData(string zone, string status, DateTime fromDate, DateTime toDate);
        Task<List<JSCComplaintViewModel>> GetComplaintWardDepartmentStatusData(string warddept, string status, DateTime fromDate, DateTime toDate);
        Task<List<JSCComplaintViewModel>> GetComplaintByWardAndDepartmentWithStatusDetails(string department, string status);
        Task<List<JSCComplaintViewModel>> GetComplaintReportData(string name, string reportType,DateTime fromDate,DateTime toDate);
        Task<List<JSCCollectorViewModel>> GetCollectorWithWardByCollectorId(string collectorId);
        Task<List<JSCParcelViewModel>> GetGISDataByAutoWise(string autoId = null, DateTime? date = null);
        Task<List<IdNameViewModel>> GetCollectorListByWard(string wardId);
        Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectedAndNotCollectedList();
        Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectedAndNotCollectedListByWard(string wardId);
        Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDateByPropertyType();
        Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDateByPropertyTypeAndWard(string wardId);
        Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgs();
        Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgsByWard(string wardId);
        Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgsByPropertyType();
        Task<JSCGarbageCollectionViewModel> GetGarbageWetAndDryWasteInKgsByPropertyTypeByWard(string wardId);

        Task<List<JSCGarbageCollectionViewModel>> GetJSCVehicleDetails(string vehicleId, DateTime? startDate, DateTime? endDate);
        Task<List<JSCParcelViewModel>> GetParcelForPropertyTaxCal();

        Task<List<JSCPropertyRegistrationViewModel>> GetPropertyRegistrationStatusWise();
        Task<CommandResult<JSCDailyBasedActivityViewModel>> ManageDailyBasedActivity(JSCDailyBasedActivityViewModel model);
        Task<List<JSCDailyBasedActivityViewModel>> GetJSCGVPData(DateTime? date);
        Task<bool> ManageRefuseCompactor(JSCRefuseCompactorViewModel model);
        Task<List<JSCRefuseCompactorViewModel>> GetRefuseCompactorData(DateTime? date = null);
        Task<List<IdNameViewModel>> GetPointList(string vehicleId);
        Task<List<JSCAssessmentViewModel>> GetViewAssessmentByDDNNO();

        Task<List<PropertyTaxPaymentReceiptViewModel>> GetPropertyTaxPaymentReceiptByDDN(string DDNNO); 
        Task<PropertyTaxPaymentReceiptViewModel> GetPropertyTaxPaymentReceiptByReceiptId(string ReceiptId);
        Task<List<IdNameViewModel>> GetDustbinData();
        Task<IdNameViewModel> GetTransferStationDetails(string transferStationId);
        Task<List<JSCParcelViewModel>> GetViewPrpertyMapByDdnNoAndUser();
        Task<List<JSCParcelViewModel>> GetViewPrpertyForSelfAssessment();
        Task<List<JSCParcelViewModel>> GetAddPropertyExist(string ddnNo);

        Task<CommandResult<JSCDailyBasedActivityViewModel>> MapPointAndVehicle(JSCDailyBasedActivityViewModel model);
        Task<List<JSCDailyBasedActivityViewModel>> GetPointAndVehicleMappingData();
        Task<JSCGarbageCollectionViewModel> GetBWGCollection();
        Task<string> GetVehicleIdForLoggedInUser(string userId);
        Task<string> GetVehicleTypeForLoggedInUser(string userId);
        Task<List<IdNameViewModel>> GetOutwardVehicleList(DateTime date);
        Task<List<JSCInwardOutwardReportViewModel>> GetJSCOutwardReport(DateTime? date);
        Task<List<JSCInwardOutwardReportViewModel>> GetJSCInwardReport(DateTime? date);
        Task UpdateWTPUserId(string id, string userId);
        Task UpdateSubLoginUserId(string id, string userId);
        Task<List<JSCSanitationReportViewModel>> GetBWGDetailsByUserId(string userId);
        Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDateByDate(DateTime? date);
        Task<CommandResult<JSCEnforcementUnAuthorizationViewModel>> InsertEnforcementUnAuthorization(JSCEnforcementUnAuthorizationViewModel model);
        Task<List<IdNameViewModel>> GetUnauthorizedCaseList();
        Task<List<IdNameViewModel>> GetEnforcementViolations();
        Task<List<JSCEnforcementUnAuthorizationViewModel>> GetJSCUnauthorizedViolationsDetail(DateTime? date,string Ward, string userId);
        Task<List<JSCGarbageCollectionViewModel>> GetGarbageCollectionDataByWardAndDate(DateTime? date, string ward);
        Task<List<JSCEnforcementUnAuthorizationViewModel>> GetAuthorizationList();
        Task<CommandResult<JSCEnforcementUnAuthorizationViewModel>> InsertEnforcementAuthorization(JSCEnforcementUnAuthorizationViewModel model);
        Task<List<IdNameViewModel>> GetAuthorizedCaseList();
        Task<List<JSCEnforcementSubLoginViewModel>> GetSubLogin(string loginType);

            Task<string> GetWardListByUser(string userId);
        Task<List<JSCEnforcementUnAuthorizationViewModel>> GetJSCAuthorizedViolationsDetail(DateTime? date, string Ward, string userId);

        Task<IList<UserViewModel>> GetUserListForSubLogin();
        Task<List<JSCEnforcementUnAuthorizationViewModel>> GetJSCOBPSAuthorizedDetail(DateTime? date, string Ward);

        Task<List<PropertyTaxPaymentReceiptViewModel>> GetPropertyPaymentDetails();
        Task<JSCPropertyTaxInstallmentViewModel> GetPropertyPaymentDetailById(string id);

        Task<string> GetNextPropertyReceiptNumber();
        Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementAuthorization(string userId);

        Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementUnAuthorization(string userId);

        Task<CommandResult<JSCEnforcementUnAuthorizationViewModel>> InsertEnforcementAuthorizedWeeklyReport(JSCEnforcementUnAuthorizationViewModel model);

        Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementAuthorizationWeeklyReport();
        Task<List<JSCEnforcementUnAuthorizationViewModel>> GetEnforcementSubloginMappinglist();

    }
}
