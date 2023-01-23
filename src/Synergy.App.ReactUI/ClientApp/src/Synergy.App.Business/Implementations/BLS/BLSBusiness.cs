using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class BLSBusiness : BusinessBase<NoteViewModel, NtsNote>, IBLSBusiness
    {
        private readonly IBLSQueryBusiness _bLSQueryBusiness;
        private readonly IUserBusiness _userBusiness;
        public BLSBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IBLSQueryBusiness bLSQueryBusiness, IUserBusiness userBusiness) : base(repo, autoMapper)
        {
            _bLSQueryBusiness = bLSQueryBusiness;
            _userBusiness = userBusiness;
        }

        public async Task<List<IdNameViewModel>> getBLSLocationList(string userId = null)
        {
            var res = await _bLSQueryBusiness.getBLSLocationList(userId);
            return res;
        }

        public async Task<List<IdNameViewModel>> GetVisaTypes()
        {
            var res = await _bLSQueryBusiness.GetVisaTypes();
            return res;
        }
        public async Task<BLSVisaAppointmentViewModel> GetAppointmentDetails(string serviceId, string serviceType)
        {
            var result = await _bLSQueryBusiness.GetAppointmentDetails(serviceId, serviceType);
            return result;
        }
        public async Task<List<BLSVisaAppointmentViewModel>> GetAppointmentDetailsByServiceId(string serviceId)
        {
            var result = await _bLSQueryBusiness.GetAppointmentDetailsByServiceId(serviceId);
            return result;
        } 
        public async Task<BLSVisaAppointmentViewModel> GetAppointmentDetailsByServiceNo(string serviceNo)
        {
            var result = await _bLSQueryBusiness.GetAppointmentDetailsByServiceNo(serviceNo);
            return result;
        }  
        public async Task<BLSApplicantViewModel> GetPassportDetail(string passportNo)
        {
            var result = await _bLSQueryBusiness.GetPassportDetail(passportNo);
            return result;
        }  
        public async Task<BLSAPiViewModel> IntegratePassportDetail(string country)
        {
            var result = await _bLSQueryBusiness.IntegratePassportDetail(country);
            return result;
        }  
        public async Task UpdateApplicationStatus(string Id,string status)
        {
            await _bLSQueryBusiness.UpdateApplicationStatus(Id,status);
        }
        public async Task<BLSVisaAppointmentViewModel> GetVisaApplicationDetails(string serviceId)
        {
            var result = await _bLSQueryBusiness.GetVisaApplicationDetails(serviceId);
            return result;
        }
        public async Task<BLSVisaAppointmentViewModel> GetAppointmentDetailsById(string id)
        {
            var result = await _bLSQueryBusiness.GetAppointmentDetailsById(id);
            return result;
        }
        public async Task<List<BLSVisaAppointmentViewModel>> GetVisaApplicationDetailsByAppId(string appId)
        {
            var result = await _bLSQueryBusiness.GetVisaApplicationDetailsByAppId(appId);
            return result;
        }
        public async Task<List<ValueAddedServicesViewModel>> GetSelectedVAS(string appId)
        {
            var result = await _bLSQueryBusiness.GetSelectedVAS(appId);
            return result;
        }
        public async Task<List<BLSTimeSlotViewModel>> GetTimeSlotList(string location)
        {
            var result = await _bLSQueryBusiness.GetTimeSlotList(location);
            return result;
        }
        public async Task<List<Holiday>> GetHolidays(string location)
        {
            var result = await _bLSQueryBusiness.GetHolidays(location);
            return result;
        }
        public async Task<List<Holiday>> GetAppointmentDate()
        { 
            var result = await _bLSQueryBusiness.GetAppointmentDate();
            return result;
        }
        public async Task<List<BLSApplicantViewModel>> GetAppointmentSlotById(string appointmentId)
        {
            var result = await _bLSQueryBusiness.GetAppointmentSlotById(appointmentId);
            return result;
        }
        public async Task<List<IdNameViewModel>> GetSlotValues(DateTime date, string loc, string serviceType, string visaType,string category)
        {
            var list = new List<IdNameViewModel>();
            var appointDetails = await _bLSQueryBusiness.GetAppointmentSlotByDate(Convert.ToString(date),loc);
            var st = new List<string>();
            if (appointDetails.Count > 0)
            {
                st.AddRange(appointDetails.Select(x => x.AppointmentSlot));
            }
            var slotData = await _bLSQueryBusiness.GetTimeSlotByLocation(date,loc, category);
            if (slotData.IsNotNull())
            {
                var slots = await _bLSQueryBusiness.GetTimeSlotByParentId(slotData.Id);
                var i = 1;
                foreach(var item in slots)
                {
                    IdNameViewModel model = new()
                    {
                        Id = (i + 1).ToString(),
                        Name = item.StartTime.ToString(@"hh\:mm") + "-" + item.EndTime.ToString(@"hh\:mm")
                    };
                    var slotcount = st.Where(x => x == model.Name).Count();
                    if (slotcount == slotData.NoOfCounter)
                    {
                        model.Code = "Exist";
                    }
                    list.Add(model);
                    i++;
                }
            }
            //if (slotData.IsNotNull())
            //{
            //    var start = slotData.StartTime;
            //    var min = (slotData.EndTime - slotData.StartTime).TotalMinutes;
            //    var count = (int)(min / slotData.SlotDuration);
            //    for (var i = 0; i <= count; i++)
            //    {
            //        TimeSpan duration = new TimeSpan(0, slotData.SlotDuration, 0);
            //        var end = start.Add(duration);
            //        if (end <= slotData.EndTime && !(start >= slotData.BreakStartTime && end <= slotData.BreakEndTime))
            //        {
            //            IdNameViewModel model = new()
            //            {
            //                Id = (i + 1).ToString(),
            //                Name = start.ToString(@"hh\:mm") + "-" + end.ToString(@"hh\:mm")
            //                //Name = (i + 10).ToString() + ":00 - " + (i + 11).ToString() + ":00"
            //            };
            //            var slotcount = st.Where(x => x == model.Name).Count();
            //            if (slotcount == slotData.NoOfCounter)
            //            {
            //                model.Code = "Exist";
            //            }
            //            list.Add(model);
            //        }
            //        start = start.Add(duration);
            //    }


            //}
            return list;

        }

        public async Task<BLSVisaApplicationSettingsViewModel> GetSettingsData()
        {
            var res = await _bLSQueryBusiness.GetSettingsData();
            return res;
        }

        public async Task<BLSVisaAppointmentViewModel> CheckEmailandServiceNo(string applicantEmail, string serviceNo)
        {
            var res = await _bLSQueryBusiness.CheckEmailandServiceNo(applicantEmail, serviceNo);
            return res;
        }

        public async Task<BLSVisaAppointmentViewModel> GetDataById(string id)
        {
            var res = await _bLSQueryBusiness.GetDataById(id);
            return res;
        }

        public async Task<BLSVisaAppointmentViewModel> GetSchengenVisaApplicationDetailsById(string id)
        {
            var res = await _bLSQueryBusiness.GetSchengenVisaApplicationDetailsById(id);
            return res;
        }

        public async Task<CommandResult<OnlinePaymentViewModel>> UpdateOnlinePaymentDetails(OnlinePaymentViewModel model)
        {
            var result = await _bLSQueryBusiness.GetOnlinePaymentDetails(model);

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
                model.PaymentGatewayReturnUrl = companySettings.FirstOrDefault(x => x.Code == "BLS_PGWY_GATEWAY_RETURN_URL")?.Value;
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
                await _bLSQueryBusiness.UpdateOnlinePaymentDetailsData(model, date);
            }

            // return commandresult - if paymentstatus is having value then return message with payment initiated and status
            return CommandResult<OnlinePaymentViewModel>.Instance(model);

        }

        public async Task UpdateOnlinePayment(OnlinePaymentViewModel responseViewModel)
        {
            await _bLSQueryBusiness.UpdateOnlinePayment(responseViewModel);
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
            var result = await _bLSQueryBusiness.GetOnlinePayment(id);
            return result;
        }

        public async Task<BLSVisaAppointmentViewModel> GetVisaAppointmentByParams(string applicantEmail, string applicationNo)
        {
            var res = await _bLSQueryBusiness.GetVisaAppointmentByParams(applicantEmail, applicationNo);
            return res;
        }

        public async Task<VisaTypeViewModel> GetVisaTypeDetails(string id)
        {
            var res = await _bLSQueryBusiness.GetVisaTypeDetails(id);
            return res;
        }

        public async Task<List<IdNameViewModel>> GetAppointmentCategoryList(string userId = null, string Id = null)
        {
            var res = await _bLSQueryBusiness.GetAppointmentCategoryList(userId, Id);
            return res;
        }

        public async Task<List<ValueAddedServicesViewModel>> GetValueAddedServices()
        {
            var res = await _bLSQueryBusiness.GetValueAddedServices();
            return res;
        }
        public async Task<BLSTimeSlotViewModel> GetTimeSlotById(string Id)
        {
            var res = await _bLSQueryBusiness.GetTimeSlotById(Id);
            return res;
        }
        public async Task<List<BLSTimeSlotViewModel>> GetAllTimeSlotList()
        {
            var res = await _bLSQueryBusiness.GetAllTimeSlotList();
            return res;
        }
        public async Task<List<TimeSlot>> GetTimeSlotByParentId(string Id)
        {
            var res = await _bLSQueryBusiness.GetTimeSlotByParentId(Id);
            return res;
        }

        public async Task<List<BLSApplicantViewModel>> GetApplicantsList(string parentId)
        {
            var res = await _bLSQueryBusiness.GetApplicantsList(parentId);
            return res;
        }

        public async Task<List<BLSVisaAppointmentViewModel>> GetMyAppointmentsList(string serviceId = null)
        {
            var res = await _bLSQueryBusiness.GetMyAppointmentsList(serviceId);
            return res;
        }
        public async Task<List<BLSApplicantViewModel>> GetAppointmentDetailsWithApplicants(string parentId = null)
        {
            var res = await _bLSQueryBusiness.GetAppointmentDetailsWithApplicants(parentId);
            return res;
        }

        public async Task<BLSVisaApplicationViewModel> GetVisaApplicationDetailsByServiceNo(string serviceNo = null)
        {
            var res = await _bLSQueryBusiness.GetVisaApplicationDetailsByServiceNo(serviceNo);
            return res;
        }
    }
}
