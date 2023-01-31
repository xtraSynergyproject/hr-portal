using Synergy.App.Business.Interface.BusinessScript.Task.General.BLS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.BLS
{
    public class TaskGeneralBLSPostScript : ITaskGeneralBLSPostScript
    {
        public async Task<CommandResult<TaskTemplateViewModel>> TriggerEmailOnBLSVisaAppointment(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)

        {
            if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            {
                var _userBusiness = sp.GetService<IUserBusiness>();
                var _emailBusiness = sp.GetService<IEmailBusiness>();
                var _serviceBusiness = sp.GetService<IServiceBusiness>();
                var _lovBusiness = sp.GetService<ILOVBusiness>();
                var _blsBusiness = sp.GetService<IBLSBusiness>();
                var _noteBusiness = sp.GetService<INoteBusiness>();
                var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                // var appointmentId = Convert.ToString(rowData["AppointmentId"]);
                var serviceData3 = await _blsBusiness.GetVisaApplicationDetails(viewModel.ParentServiceId);

                var appointmentId = serviceData3.AppointmentId;

                var serviceData1 = await _blsBusiness.GetAppointmentDetailsById(appointmentId);
                LOVViewModel lov = null;
                lov = await _lovBusiness.GetSingle(x => x.Code == "APPLICATION_APPROVED" && x.LOVType == "BLS_APPOINTMENT_STATUS");
                 var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = serviceData1.NtsNoteId;
                noteTempModel.SetUdfValue = true;
                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData2 = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                rowData2["AppointmentStatusId"] = lov?.Id;

                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData2);
                var update = await _noteBusiness.EditNoteUdfTable(noteModel, data1, noteModel.UdfNoteTableId);
                var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "BLS_VISA_APPOINTMENT_APPROVED");
                if (notificationTemplateModel.IsNotNull())
                {
                    var serviceData = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
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

                            return CommandResult<TaskTemplateViewModel>.Instance(viewModel, true, "Email Send Successfully");
                        }
                        return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, result.Messages.ToHtmlError());
                    }
                    else
                    {
                        return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, "No support user created");
                    }

                }
                return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, "No email template created");
            }
           else if (viewModel.TaskStatusCode == "TASK_STATUS_REJECT")
            {
                var _userBusiness = sp.GetService<IUserBusiness>();
                var _emailBusiness = sp.GetService<IEmailBusiness>();
                var _serviceBusiness = sp.GetService<IServiceBusiness>();
                var _lovBusiness = sp.GetService<ILOVBusiness>();
                var _blsBusiness = sp.GetService<IBLSBusiness>();
                var _noteBusiness = sp.GetService<INoteBusiness>();
                var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                //var appointmentId = Convert.ToString(rowData["AppointmentId"]);
                var serviceData3 = await _blsBusiness.GetVisaApplicationDetails(viewModel.ParentServiceId);

                var appointmentId = serviceData3.AppointmentId;
                var serviceData1 = await _blsBusiness.GetAppointmentDetailsById(appointmentId);
                LOVViewModel lov = null;
                lov = await _lovBusiness.GetSingle(x => x.Code == "APPLICATION_REJECTED" && x.LOVType == "BLS_APPOINTMENT_STATUS");
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = serviceData1.NtsNoteId;
                noteTempModel.SetUdfValue = true;
                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData2 = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                rowData2["AppointmentStatusId"] = lov?.Id;

                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData2);
                var update = await _noteBusiness.EditNoteUdfTable(noteModel, data1, noteModel.UdfNoteTableId);
                var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "BLS_VISA_APPOINTMENT_REJECTED");
                if (notificationTemplateModel.IsNotNull())
                {
                    var serviceData = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
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

                            return CommandResult<TaskTemplateViewModel>.Instance(viewModel, true, "Email Send Successfully");
                        }
                        return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, result.Messages.ToHtmlError());
                    }
                    else
                    {
                        return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, "No support user created");
                    }

                }
                return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, "No email template created");
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, "");


        }
        public async Task<CommandResult<TaskTemplateViewModel>> TriggerEmailOnBLSVisaStamping(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)

        {
            if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            {
                var _userBusiness = sp.GetService<IUserBusiness>();
                var _emailBusiness = sp.GetService<IEmailBusiness>();
                var _serviceBusiness = sp.GetService<IServiceBusiness>();
                var _lovBusiness = sp.GetService<ILOVBusiness>();
                var _blsBusiness = sp.GetService<IBLSBusiness>();
                var _noteBusiness = sp.GetService<INoteBusiness>();
                var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                //var appointmentId = Convert.ToString(rowData["AppointmentId"]);
                var serviceData3 = await _blsBusiness.GetVisaApplicationDetails(viewModel.ParentServiceId);

                var appointmentId = serviceData3.AppointmentId;
                var serviceData1 = await _blsBusiness.GetAppointmentDetailsById(appointmentId);
                LOVViewModel lov = null;
                lov = await _lovBusiness.GetSingle(x => x.Code == "VISA_STAMPED" && x.LOVType == "BLS_APPOINTMENT_STATUS");
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = serviceData1.NtsNoteId;
                noteTempModel.SetUdfValue = true;
                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData2 = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                rowData2["AppointmentStatusId"] = lov?.Id;

                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData2);
                var update = await _noteBusiness.EditNoteUdfTable(noteModel, data1, noteModel.UdfNoteTableId);
                var notificationTemplateModel = await _userBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "BLS_VISA_STAMPING");
                if (notificationTemplateModel.IsNotNull())
                {
                    var serviceData = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
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

                            return CommandResult<TaskTemplateViewModel>.Instance(viewModel, true, "Email Send Successfully");
                        }
                        return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, result.Messages.ToHtmlError());
                    }
                    else
                    {
                        return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, "No support user created");
                    }

                }
                return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, "No email template created");
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, "");


        }



    }
}
