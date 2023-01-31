
using Newtonsoft.Json.Linq;
using Synergy.App.Business.Interface.BusinessScript.Service.General.BLS;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Service.General.BLS
{
    public class ServiceGeneralBLSPreScript : IServiceGeneralBLSPreScript
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>    
        public async Task<CommandResult<ServiceTemplateViewModel>> BLSValidatePassportExpiry(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var passportExpiry = Convert.ToDateTime(rowData["ConnectionCategoryId"]);
            var vaidateDate = DateTime.Now.Date.AddMonths(6);

            if (passportExpiry.Date <= vaidateDate.Date)
            {
                Dictionary<string, string> message = new Dictionary<string, string>();
                message.Add("PassportExpiry", "Passport should be validate for 6 months from today date.");
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, message);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> ChangeAppoinmentApplicationStatusCode(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                var _lovBusiness = sp.GetService<ILOVBusiness>();
                var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                
                var lov = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPOINTMENT_CONFIRMED" && x.LOVType == "BLS_APPOINTMENT_STATUS");
                rowData["AppointmentStatusId"] = lov?.Id;

            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
       
        public async Task<CommandResult<ServiceTemplateViewModel>> ChangeVisaApplicationStatusCode(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_DRAFT" || viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                var _lovBusiness = sp.GetService<ILOVBusiness>();
                var _blsBusiness = sp.GetService<IBLSBusiness>();
                var _noteBusiness = sp.GetService<INoteBusiness>();
                var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                //var appointmentId = Convert.ToString(rowData["AppointmentId"]);
                //var serviceData = await _blsBusiness.GetAppointmentDetailsById(appointmentId);
                //var serviceData = await _blsBusiness.GetVisaApplicationDetails(viewModel.ServiceId);

                LOVViewModel lov = null;
                if(viewModel.ServiceStatusCode == "SERVICE_STATUS_DRAFT")
                {
                    lov = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPLICATION_DRAFTED" && x.LOVType == "BLS_APPLICATION_STATUS");
                }
                else if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
                {
                    lov = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPLICATION_UPDATED" && x.LOVType == "BLS_APPLICATION_STATUS");
                }

                //await _blsBusiness.UpdateApplicationStatus(viewModel.UdfNoteTableId,lov.Id);
                rowData["ApplicationStatusId"] = lov.Id;
                viewModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                // serviceData.AppointmentStatusId = lov?.Id;

                //var noteTempModel = new NoteTemplateViewModel();
                //noteTempModel.NoteId = serviceData.NtsNoteId;
                //noteTempModel.SetUdfValue = true;
                //var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);
                //var rowData2 = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);

                //rowData2["ApplicationStatusId"] = lov?.Id;

                //var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData2);
                //var update = await _noteBusiness.EditNoteUdfTable(noteModel, data1, noteModel.UdfNoteTableId);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> GenerateFileNumber(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            
            var _ntsBusiness = sp.GetService<INtsQueryBusiness>();
            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var fileNum = rowdata["FileNumber"].ToString();

            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_DRAFT" || viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                if (fileNum.IsNotNullAndNotEmpty())
                {
                    var date = DateTime.Today;
                    //var first = new DateTime(date.Year,1,1);

                    var seq = await _ntsBusiness.GetNextBLSFileNumber(date.Year);
                    var sequence = seq.PadLeft(8, '0');
                    var fileno = $"{date.Year}{sequence}";
                    rowdata["FileNumber"] = fileno;
                    viewModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowdata);
                }                
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> SetAttachmentGridData(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);

            //string attachments = "{FileName:Passport Front,FileId:" + rowData["PassportFrontId"]+ "},{FileName:Passport Back,FileId:" + rowData["PassportBackId"]+ "}";
            //JObject json = JObject.Parse(obj);
            if(viewModel.ServiceStatusCode == "SERVICE_STATUS_DRAFT")
            {
                var obj = new[] { new { FileName="Passport Front",FileId=rowData["PassportFrontId"].ToString() },
                            new {FileName="Passport Back",FileId=rowData["PassportBackId"].ToString() } };

                rowData["Application_Document_Attachment"] = obj;

                var appStatus = "";
                if (viewModel.DataAction == DataActionEnum.Create)
                {
                    var status = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPLICATION_DRAFTED");
                    appStatus = status.Id;
                }
                else if (viewModel.DataAction != DataActionEnum.Create && viewModel.ServiceStatusCode == "SERVICE_STATUS_DRAFT")
                {
                    var status = await _lovBusiness.GetSingle(x => x.Code == "BLS_APPLICATION_DRAFTED");
                    appStatus = status.Id;
                }

                rowData["ApplicationStatusId"] = appStatus;

                var statusObj = new[] { new { StatusId = appStatus, UpdatedById = viewModel.OwnerUserId } };

                rowData["BLSAPPLICATIONSTATUS"] = statusObj;

                viewModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            }  

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }
}
