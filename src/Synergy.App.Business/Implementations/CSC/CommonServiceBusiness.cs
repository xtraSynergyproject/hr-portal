using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Synergy.App.ViewModel.EGOV;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class CommonServiceBusiness : BusinessBase<NoteViewModel, NtsNote>, ICommonServiceBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _querydata;
        private readonly IRepositoryQueryBase<EGovCommunityHallViewModel> _querych;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<ServiceTemplateViewModel> _query;
        private readonly INoteBusiness _noteBusiness;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryRepo1;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IUserBusiness _userBusiness;        
        private readonly ICommonServiceQueryBusiness _queryCSCBusiness;
        public CommonServiceBusiness(IRepositoryQueryBase<IdNameViewModel> querydata,
            IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IRepositoryQueryBase<EGovCommunityHallViewModel> querych
            , IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<TaskViewModel> queryRepo1,
            IRepositoryQueryBase<ServiceTemplateViewModel> query, INoteBusiness noteBusiness,
             ICommonServiceQueryBusiness queryCSCBusiness,
            ILOVBusiness lOVBusiness, IServiceBusiness serviceBusiness, IUserBusiness userBusiness) : base(repo, autoMapper)
        {
            _querych = querych;
            _queryRepo = queryRepo;
            _query = query;
            _noteBusiness = noteBusiness;
            _queryRepo1 = queryRepo1;
            _lOVBusiness = lOVBusiness;
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _querydata = querydata;            
            _queryCSCBusiness = queryCSCBusiness;

        }
               

        public async Task<CommandResult<OnlinePaymentViewModel>> UpdateOnlinePaymentDetails(OnlinePaymentViewModel model)
        {
            var result = await _queryCSCBusiness.UpdateOnlinePaymentDetails(model);            

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
                model.PaymentGatewayReturnUrl = companySettings.FirstOrDefault(x => x.Code == "CSC_PGW_GATEWAY_RETURN_URL")?.Value;
                // model.PaymentGatewayReturnUrl = "https://localhost:44389/egov/egovernment/paymentresponse";
            }
            model.TypeField2 = "NA";
            model.Filler1 = "NA";
            model.AdditionalInfo1 = "NA";
            model.AdditionalInfo2 = "NA";
            model.AdditionalInfo3 = "NA";
            model.AdditionalInfo4 = "NA";
            model.AdditionalInfo5 = "NA";
            model.AdditionalInfo6 = "NA";
            model.AdditionalInfo7 = "NA";
            model.Message = String.Concat(model.MerchantID, "|", model.Id, "|", model.Filler1, "|", model.Amount.ToString("#.00"), "|NA|NA|NA|", model.CurrencyType, "|NA|", model.TypeField1, "|", model.SecurityID, "|NA|NA|F|", model.MobileNumber, "|", model.EmailId, "|NA|NA|NA|NA|NA|", model.PaymentGatewayReturnUrl);

            model.ChecksumValue = await GenerateCheckSum(model.ChecksumKey, model.Message);

            model.RequestUrl = String.Concat(model.PaymentGatewayUrl, "?msg=", model.Message, "|", model.ChecksumValue);
            if (result.IsNotNull())
            {
                await UpdateOnlinePayment(model);
            }
            else
            {
                await _queryCSCBusiness.UpdateOnlinePaymentDetailsData(model, date);
            }



            // return commandresult - if paymentstatus is having value then return message with payment initiated and status
            return CommandResult<OnlinePaymentViewModel>.Instance(model);

        }
        public async Task UpdateOnlinePayment(OnlinePaymentViewModel responseViewModel)
        {
            await _queryCSCBusiness.UpdateOnlinePayment(responseViewModel);
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

        public async Task<OnlinePaymentViewModel> GetOnlinePayment(string id)
        {
            var result = await _queryCSCBusiness.GetOnlinePayment(id);
            return result;
        }
        public async Task<CSCReportViewModel> GetCSCBirthCertificateData(string serviceId)
        {
            var data = await _queryCSCBusiness.GetCSCBirthCertificateData(serviceId);
            return data;
        }
        public async Task<CSCReportMarriageCertificateViewModel> GetCSCMarriageCertificateData(string serviceId)
        {
            var data = await _queryCSCBusiness.GetCSCMarriageCertificateData(serviceId);
            return data;
        }
        public async Task<CSCReportOBCCertificateViewModel> GetCSCOBCCertificateData(string serviceId)
        {
            var data = await _queryCSCBusiness.GetCSCOBCCertificateData(serviceId);
            return data;
        }
        public async Task<CSCReportAcknowledgementViewModel> GetCSCAcknowledgementData(string serviceId)
        {
            var data = await _queryCSCBusiness.GetCSCAcknowledgementData(serviceId);
            return data;
        }
        public async Task<List<ServiceChargeViewModel>> GetServiceChargeData(string serviceId)
        {
            var data =  await _queryCSCBusiness.GetServiceChargeData(serviceId);
            return data;
        }
        public async Task<List<CSCTrackApplicationViewModel>> GetTrackApplicationList(string applicationNo)
        {
            var data = await _queryCSCBusiness.GetTrackApplicationList(applicationNo);
            return data;
        }
        public async Task<IList<TaskViewModel>> GetCSCTaskList(string portalId)
        {
            var result = await _queryCSCBusiness.GetCSCTaskList(portalId);
            //foreach (var i in result)
            //{
            //    i.DisplayDueDate = i.DueDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            //    i.DisplayStartDate = i.StartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);
            //}
            return result;
        }

        public async Task<IList<ServiceChargeViewModel>> GetServiceChargeDetails(string serviceId)
        {
            return await _queryCSCBusiness.GetServiceChargeDetails(serviceId);
        }

        public async Task<long> GetDocumentsCount(string udfnotetableId)
        {
            return await _queryCSCBusiness.GetDocumentsCount(udfnotetableId);
        }

    }
}
