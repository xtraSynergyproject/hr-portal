
using Newtonsoft.Json.Linq;
using Synergy.App.Business.Interface.BusinessScript.Service.General.BLS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Service.General.BLS
{
    public class ServiceGeneralBLSPostScript : IServiceGeneralBLSPostScript
    {
       

        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerBLSVisaApprovalService(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {                
                var serTempModel = new ServiceTemplateViewModel();
                serTempModel.ActiveUserId = uc.UserId;
                serTempModel.TemplateCode = "BLS_VISA_SCHENGEN_APPROVAL";
                var sermodel = await _serviceBusiness.GetServiceDetails(serTempModel);

                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("AppointmentId", udf.AppointmentId);
                ((IDictionary<String, Object>)exo).Add("FileNumber", udf.FileNumber);
                ((IDictionary<String, Object>)exo).Add("ApplicationId", viewModel.UdfNoteTableId);  
                ((IDictionary<String, Object>)exo).Add("ForwardedToMofa", false);  
                
                sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                sermodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                sermodel.DataAction = DataActionEnum.Create;
                var result = await _serviceBusiness.ManageService(sermodel);
                
            }            
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerEmailOnBLSVisaAppointmentComplete(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)

        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var _userBusiness = sp.GetService<IUserBusiness>();
                var _emailBusiness = sp.GetService<IEmailBusiness>();
                var _configuration = sp.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
                var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);

                var _lovBusiness = sp.GetService<ILOVBusiness>();
                var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                //var AppstatusId = rowData["AppointmentStatusId"];
                var lov = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPOINTMENT_CONFIRMED" && x.LOVType == "BLS_APPOINTMENT_STATUS");
                rowData["AppointmentStatusId"] = lov?.Id;
                var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "BLS_APPOINTMENT_COMPLETE");
                if (notificationTemplateModel.IsNotNull())
                {
                    var _serviceBusiness = sp.GetService<IServiceBusiness>();
                    var serviceData = await _serviceBusiness.GetSingleById(viewModel.ServiceId);
                    var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);

                    if (serviceData.IsNotNull())
                    {
                        viewModel.OwnerUserId = serviceData.OwnerUserId;
                    }
                    var userData = await _userBusiness.GetSingle(x => x.Id == viewModel.OwnerUserId);
                    //var recipients = await _companySettings.GetSingle(x => x.Code == "WEBSITE_SUPPORT_RECIPIENTS");
                    var recipient = userData.Email;

                    var attachments = new List<string>();
                    var client = new WebClient();
                    var uri = new Uri(string.Concat(baseurl, "Core/Cms/FastReportPdfFileId?rptName=BLS_VisaAppointmentReport&rptUrl=bls/query/GetAppointmentReceipt/",serviceData.Id,"/",serviceData.OwnerUserId,"&lo=Popup"));
                    client.Headers.Add("Content-Type:application/json");
                    client.Headers.Add("Accept:application/json");
                    var response = client.DownloadString(uri);
                    var json = JObject.Parse(response);
                    var atachIsSuccess = json.Value<bool>("success");
                    if (atachIsSuccess)
                    {
                        var fileid = json.Value<string>("fileId");
                        attachments.Add(fileid); 
                    }
                    if (recipient.IsNotNullAndNotEmpty())
                    {

                        var result = await _emailBusiness.SendMailAsync(
                            new EmailViewModel
                            {
                                Id = Guid.NewGuid().ToString(),
                                To = recipient,
                                Subject = notificationTemplateModel.Subject,
                                Body = notificationTemplateModel.Body,
                                DataAction = DataActionEnum.Create,
                                AttachmentIds = attachments.ToArray(),
                        });

                        if (result.IsSuccess)
                        {

                            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, true, "Email Send Successfully");
                        }
                        return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, result.Messages.ToHtmlError());
                    }
                    else
                    {
                        return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, "No support user created");
                    }

                }
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, "No email template created");
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, "");


        }
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerEmailOnBLSVisaServiceComplete(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var _userBusiness = sp.GetService<IUserBusiness>();
                var _emailBusiness = sp.GetService<IEmailBusiness>();
                var _serviceBusiness = sp.GetService<IServiceBusiness>();
                var _lovBusiness = sp.GetService<ILOVBusiness>();
                var _blsBusiness = sp.GetService<IBLSBusiness>();
                var _noteBusiness = sp.GetService<INoteBusiness>();

                //var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                //var appointmentId = Convert.ToString(rowData["AppointmentId"]);
                //var serviceData1 = await _blsBusiness.GetAppointmentDetailsById(appointmentId);
                LOVViewModel lov = null;
                lov = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPLICATION_APPROVED" && x.LOVType == "BLS_APPLICATION_STATUS");
                //var noteTempModel = new NoteTemplateViewModel();
                //noteTempModel.NoteId = serviceData1.NtsNoteId;
                //noteTempModel.SetUdfValue = true;
                //var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);
                //var rowData2 = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                //rowData2["AppointmentStatusId"] = lov?.Id;
                //var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData2);
                //var update = await _noteBusiness.EditNoteUdfTable(noteModel, data1, noteModel.UdfNoteTableId);

                await _blsBusiness.UpdateApplicationStatus(viewModel.UdfNoteTableId,lov.Id);

                var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "BLS_APPOINTMENT_COMPLETE");
                if (notificationTemplateModel.IsNotNull())
                {
                    var serviceData = await _serviceBusiness.GetSingleById(viewModel.Id);
                    var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);

                    if (serviceData.IsNotNull())
                    {
                        viewModel.OwnerUserId = serviceData.OwnerUserId;
                    }
                    var userData = await _userBusiness.GetSingle(x => x.Id == viewModel.OwnerUserId);
                    //var recipients = await _companySettings.GetSingle(x => x.Code == "WEBSITE_SUPPORT_RECIPIENTS");
                    var recipient = userData.Email;
                    // string[] attachments = { data.PageId };

                    if (recipient.IsNotNullAndNotEmpty())
                    {

                        var result = await _emailBusiness.SendMailAsync(
                            new EmailViewModel
                            {
                                Id = Guid.NewGuid().ToString(),
                                To = recipient,
                                Subject = notificationTemplateModel.Subject,
                                Body = notificationTemplateModel.Body,
                                DataAction = DataActionEnum.Create,
                                //AttachmentIds = attachments,
                            });

                        if (result.IsSuccess)
                        {

                            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, true, "Email Send Successfully");
                        }
                        return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, result.Messages.ToHtmlError());
                    }
                    else
                    {
                        return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, "No support user created");
                    }

                }
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, "No email template created");
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, "");

        }

        public async Task<CommandResult<ServiceTemplateViewModel>> CreateVisaApplicationService(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            if (viewModel.DataAction == DataActionEnum.Create)
            {
                var appCount = Convert.ToInt32(udf.ApplicantsNo);
                for (var i=0;i< appCount; i++)
                {
                    var serTempModel = new ServiceTemplateViewModel();
                    serTempModel.ActiveUserId = uc.UserId;
                    serTempModel.TemplateCode = "BLS_VISA_SCHENGEN";
                    var sermodel = await _serviceBusiness.GetServiceDetails(serTempModel);

                    dynamic exo = new System.Dynamic.ExpandoObject();
                    ((IDictionary<String, Object>)exo).Add("AppointmentId", udf.UdfNoteTableId);
                    ((IDictionary<String, Object>)exo).Add("Surname", null);
                    ((IDictionary<String, Object>)exo).Add("SurnameatBirth", null);
                    ((IDictionary<String, Object>)exo).Add("FirstName", null);
                    ((IDictionary<String, Object>)exo).Add("DateOfBirth", null);
                    ((IDictionary<String, Object>)exo).Add("PlaceOfBirth", null);
                    ((IDictionary<String, Object>)exo).Add("CountryOfBirthId", null);
                    ((IDictionary<String, Object>)exo).Add("CurrentNationalityId", null);
                    ((IDictionary<String, Object>)exo).Add("OriginalNationalityId", null);
                    ((IDictionary<String, Object>)exo).Add("GenderId", null);
                    ((IDictionary<String, Object>)exo).Add("MaritalStatusId", null);
                    ((IDictionary<String, Object>)exo).Add("ApplicantsEmailId", null);
                    ((IDictionary<String, Object>)exo).Add("TelephoneNo", null);
                    ((IDictionary<String, Object>)exo).Add("HomeAddress", null);
                    ((IDictionary<String, Object>)exo).Add("ResidenceInOtherCountry", null);
                    ((IDictionary<String, Object>)exo).Add("PurposeOfJourneyId", null);
                    ((IDictionary<String, Object>)exo).Add("PassportFrontId", null);
                    ((IDictionary<String, Object>)exo).Add("PassportBackId", null);

                    sermodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    sermodel.ServiceStatusCode = "SERVICE_STATUS_DRAFT";
                    sermodel.DataAction = DataActionEnum.Create;
                    var result = await _serviceBusiness.ManageService(sermodel);
                }
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

    }
}

