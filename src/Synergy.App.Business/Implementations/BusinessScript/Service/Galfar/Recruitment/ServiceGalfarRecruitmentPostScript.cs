using Synergy.App.Business.Interface.BusinessScript.Service.Galfar.Recruitment;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;


namespace Synergy.App.Business.Implementations.BusinessScript.Service.Galfar.Recruitment
{
    public class ServiceGalfarRecruitmentPostScript : IServiceGalfarRecruitmentPostScript
    {
        /// <summary>
        /// This method used trigger mid year and end year review task to employee and manager
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerIntentToOffer(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode== "SERVICE_STATUS_COMPLETE")
            {
                var _recBusiness = sp.GetService<IRecruitmentTransactionBusiness>();
                await _recBusiness.TriggerIntentToOffer(viewModel,udf);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerWorkerAppointment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var _recBusiness = sp.GetService<IRecruitmentTransactionBusiness>();
                await _recBusiness.TriggerWorkerAppointment(viewModel, udf);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerFinalOffer(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var _recBusiness = sp.GetService<IRecruitmentTransactionBusiness>();
                await _recBusiness.TriggerFinalOffer(viewModel, udf);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggeVisa(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var _recBusiness = sp.GetService<IRecruitmentTransactionBusiness>();
                var _lovBusiness = sp.GetService<ILOVBusiness>();
                var Visa = _lovBusiness.GetSingleById(udf.VisaCategoryId);
                if (Visa == "BUSINESS_VISA" || Visa == "BUSINESS_VISA_MULTIPLE")
                {
                    await _recBusiness.TriggerOverseasBusinessVisa(viewModel, udf);
                }
                else if (Visa == "WORK_VISA")
                {
                    await _recBusiness.TriggerOverseasWorkVisa(viewModel, udf);
                }
                else if (Visa == "VISA_TRANSFER")
                {
                    await _recBusiness.TriggerVisaTransfer(viewModel, udf);
                }
                else if (Visa == "LOCAL_WORK_PERMIT")
                {
                    await _recBusiness.TriggerWorkPermit(viewModel, udf);
                }
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
       
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerTravelling(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var _recBusiness = sp.GetService<IRecruitmentTransactionBusiness>();
                await _recBusiness.TriggerTravelling(viewModel, udf);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerJoining(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var _recBusiness = sp.GetService<IRecruitmentTransactionBusiness>();
                var _appBusiness = sp.GetService<IRecQueryBusiness>();
                var application = await _appBusiness.GetApplicationDetail(udf.ApplicationId);
                if (application.ManpowerTypeCode == "Staff")
                {
                    await _recBusiness.TriggerStaffJoining(viewModel, udf);
                }
                else
                {
                    await _recBusiness.TriggerWorkerJoining(viewModel, udf);

                }
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }


        public async Task<CommandResult<ServiceTemplateViewModel>> ScheduleForInterview(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            
            var _recBusiness = sp.GetService<IRecQueryBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            //var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var status = await _lovBusiness.GetSingle(x => x.Code == "InterviewRequested");
            await _recBusiness.UpdateApplicationStatusForInterview(udf, status.Id);

            //viewModel.ApplicationStatusId = statusId;



            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerCRPFAppointment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var _recBusiness = sp.GetService<IRecruitmentTransactionBusiness>();
                await _recBusiness.TriggerCRPFAppointment(viewModel, udf);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> PublishToCareerPortal(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var _recBusiness = sp.GetService<IRecQueryBusiness>();
                await _recBusiness.UpdateJobAdvertisementStatus(viewModel.ServiceId);

            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> CreatePersonPositionandAssignment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _recruitmentBusiness = sp.GetService<IRecruitmentTransactionBusiness>();
            var _uc = sp.GetService<IUserContext>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _hrBusiness = sp.GetService<IHRCoreBusiness>();
            var _cmsBusiness = sp.GetService<ICmsBusiness>();

            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                //Person
                //var appointment = await _recruitmentBusiness.GetAppointmentDetailsById(viewModel.UdfNoteTableId);
                
                var app = new RecApplicationViewModel();
                app = await _recruitmentBusiness.GetApplicationDetailsById(udf.ApplicationId);
                var personmodel = new PersonViewModel()
                {
                    UserId = app.ApplicationUserId,
                    TitleId = app.TitleId,
                    FirstName = app.FirstName,
                    MiddleName = app.MiddleName,
                    LastName = app.LastName,
                    PersonFullName = app.MiddleName.IsNotNull() ? string.Concat(app.FirstName, "", app.MiddleName, "", app.LastName) : string.Concat(app.FirstName, "", app.LastName),
                    GenderId = app.GenderId,
                    DateOfBirth = app.BirthDate.Value,
                    MaritalStatusId = app.MaritalStatus,
                    NationalityId = app.NationalityId,
                    DateOfJoin = app.JoiningDate,
                    PresentAddressBuildingNumber = app.CurrentAddressHouse,
                    PresentAddressStreetName = app.CurrentAddressStreet,
                    PresentAddressCityOrTown = app.CurrentAddressCity,
                    PresentAddressCountryId = app.CurrentAddressCountryId,
                    PermanentAddressBuildingNumber = app.PermanentAddressHouse,
                    PermanentAddressStreetName = app.PermanentAddressStreet,
                    PermanentAddressCityOrTown = app.PermanentAddressCity,
                    PermanentAddressCountryId = app.PermanentAddressCountryId,
                    ContactPersonalEmail = app.Email,
                    MobileNumber = app.ContactPhoneLocal
                };

                var noteTemp = new NoteTemplateViewModel();
                noteTemp.ActiveUserId = _uc.UserId;
                noteTemp.TemplateCode = "HRPerson";
                noteTemp.DataAction = DataActionEnum.Create;

                var notemodel = await _noteBusiness.GetNoteDetails(noteTemp);

                notemodel.Json = JsonConvert.SerializeObject(personmodel);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var personresult = await _noteBusiness.ManageNote(notemodel);

                //Position
                //var job = await _hrBusiness.GetJobNameById(app.JobId);
                //var department = await _hrBusiness.GetDepartmentNameById("d4b52029-0d6f-49b6-bf19-4baf6f62c49e");

                var dept = app.OrganizationId.IsNotNullAndNotEmpty()? app.OrganizationId : "d4b52029-0d6f-49b6-bf19-4baf6f62c49e";
                
                var pos = await _hrBusiness.CreatePosition(dept, app.JobId);
                //var position = await _hrBusiness.GetPositionDetailsById(pos.Id);
                var positionModel = new PositionViewModel()
                {
                    DepartmentId = dept,
                    JobId = app.JobId,
                    PositionName = pos.PositionName,
                };

                var noteTemp1 = new NoteTemplateViewModel();
                noteTemp1.ActiveUserId = _uc.UserId;
                noteTemp1.TemplateCode = "HRPosition";
                noteTemp1.DataAction = DataActionEnum.Create;

                var notemodel1 = await _noteBusiness.GetNoteDetails(noteTemp1);

                notemodel1.Json = JsonConvert.SerializeObject(positionModel);
                notemodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var positionresult = await _noteBusiness.ManageNote(notemodel1);

                //Assignment
                var jobadv = await _cmsBusiness.GetDataListByTemplate("REC_JOB_ADVERTISEMENT", null, $@" and ""N_REC_JobAdvertisement"".""Id""='{app.JobAdvertisementId}' ");
                var locationId = "";
                if (jobadv.Rows.Count > 0)
                {
                    foreach (DataRow dt in jobadv.Rows)
                    {
                        locationId = dt["JobLocationId"].ToString();
                    }
                }

                var assGrade = app.OfferGrade.IsNotNullAndNotEmpty() ? app.OfferGrade : "a6fd32ff-c8a4-4c45-a362-eb526fe76184";
                var assignmentModel = new AssignmentViewModel()
                {
                    EmployeeId = personresult.Item.UdfNoteTableId,
                    DepartmentId = dept,
                    JobId = app.JobId,
                    PositionId = pos.Id,
                    LocationId = locationId,
                    AssignmentGradeId = assGrade,
                    DateOfJoin = app.JoiningDate.ToString()
                };

                var noteTemp2 = new NoteTemplateViewModel();
                noteTemp2.ActiveUserId = _uc.UserId;
                noteTemp2.TemplateCode = "HRAssignment";
                noteTemp2.DataAction = DataActionEnum.Create;

                var notemodel2 = await _noteBusiness.GetNoteDetails(noteTemp2);

                notemodel2.Json = JsonConvert.SerializeObject(assignmentModel);
                notemodel2.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var assignmentresult = await _noteBusiness.ManageNote(notemodel2);
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }
}
