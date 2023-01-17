using CMS.Business.Interface.BusinessScript.Task.General.General;
using CMS.Common;
using CMS.UI.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Task.General.General
{
    public class TaskGeneralCaseManagementPostScript : ITaskGeneralCaseManagementPostScript
    {
        /// <summary>
        /// TriggerService - it will trigger service upon task completion
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
      
      
        public async Task<CommandResult<TaskTemplateViewModel>> InitiateServiceForExternalServiceOld(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _serviceTemplateBusiness = sp.GetService<IServiceTemplateBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_TASK_STATUS");
            if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            {
                var service = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
                // get the udf values from the service


                DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(service.TemplateId, service.UdfNoteId);
                if (row != null)
                {

                    string serviceTemplate1 = row["service1"].ToString();
                    string serviceTemplate2 = row["service2"].ToString();
                    string serviceTemplate3 = row["service3"].ToString();
                    if (serviceTemplate1.IsNotNullAndNotEmpty())
                    {
                        var service1 = new ServiceTemplateViewModel();
                        service1.TemplateId = serviceTemplate1;                       
                        var serviceModel1 = await _serviceBusiness.GetServiceDetails(service1);
                        serviceModel1.ServiceStatusCode = "SERVIE_STATUS_INPROGRESS";
                        await _serviceBusiness.ManageService(serviceModel1);
                    }
                    if (serviceTemplate1.IsNotNullAndNotEmpty())
                    {
                        var service1 = new ServiceTemplateViewModel();
                        var template = await _serviceTemplateBusiness.GetSingleById(serviceTemplate1);
                        if (template.IsNotNull())
                        {
                            var ownerType = await _lovBusiness.GetSingle(x => x.Id == template.DefaultServiceOwnerTypeId);
                            if (ownerType.Code == "SERVICE_OWNER_TYPE_USER")
                            {
                                service1.OwnerUserId = template.DefaultOwnerUserId;
                            }
                            if (ownerType.Code == "SERVICE_OWNER_TYPE_TEAM")
                            {
                                var _teamBusiness = sp.GetService<ITeamBusiness>();
                                var team = await _teamBusiness.GetSingleById(template.DefaultOwnerTeamId);
                                var teamOwner = await _teamBusiness.GetTeamOwner(team.Id);
                                service1.OwnerUserId = teamOwner.Id;
                            }
                        }
                        service1.TemplateId = serviceTemplate1;
                        service1.RequestedByUserId = viewModel.AssignedToUserId;
                        service1.DataAction = DataActionEnum.Create;
                        var serviceModel1 = await _serviceBusiness.GetServiceDetails(service1);
                        serviceModel1.ServiceStatusCode = "SERVIE_STATUS_INPROGRESS";
                        serviceModel1.StartDate = DateTime.Now;
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        // set the udf value
                        ((IDictionary<String, Object>)exo).Add("externalServiceRequest", viewModel.ParentServiceId);
                        serviceModel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _serviceBusiness.ManageService(serviceModel1);
                    }
                    if (serviceTemplate2.IsNotNullAndNotEmpty())
                    {
                        var service2 = new ServiceTemplateViewModel();
                        service2.TemplateId = serviceTemplate2;
                        var template = await _serviceTemplateBusiness.GetSingleById(serviceTemplate2);
                        if (template.IsNotNull())
                        {
                            var ownerType = await _lovBusiness.GetSingle(x => x.Id == template.DefaultServiceOwnerTypeId);
                            if (ownerType.Code == "SERVICE_OWNER_TYPE_USER")
                            {
                                service2.OwnerUserId = template.DefaultOwnerUserId;
                            }
                            if (ownerType.Code == "SERVICE_OWNER_TYPE_TEAM")
                            {
                                var _teamBusiness = sp.GetService<ITeamBusiness>();
                                var team = await _teamBusiness.GetSingleById(template.DefaultOwnerTeamId);
                                var teamOwner = await _teamBusiness.GetTeamOwner(team.Id);
                                service2.OwnerUserId = teamOwner.Id;
                            }
                        }
                        service2.RequestedByUserId = viewModel.AssignedToUserId;
                        service2.DataAction = DataActionEnum.Create;
                        var serviceModel2 = await _serviceBusiness.GetServiceDetails(service2);
                        serviceModel2.ServiceStatusCode = "SERVIE_STATUS_INPROGRESS";
                        serviceModel2.StartDate = DateTime.Now;
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        // set the udf value
                        ((IDictionary<String, Object>)exo).Add("externalServiceRequest", viewModel.ParentServiceId);
                        serviceModel2.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _serviceBusiness.ManageService(serviceModel2);
                    }
                    if (serviceTemplate3.IsNotNullAndNotEmpty())
                    {
                        var service3 = new ServiceTemplateViewModel();
                        service3.TemplateId = serviceTemplate3;
                        var template = await _serviceTemplateBusiness.GetSingleById(serviceTemplate3);
                        if (template.IsNotNull())
                        {
                            var ownerType = await _lovBusiness.GetSingle(x => x.Id == template.DefaultServiceOwnerTypeId);
                            if (ownerType.Code == "SERVICE_OWNER_TYPE_USER")
                            {
                                service3.OwnerUserId = template.DefaultOwnerUserId;
                            }
                            if (ownerType.Code == "SERVICE_OWNER_TYPE_TEAM")
                            {
                                var _teamBusiness = sp.GetService<ITeamBusiness>();
                                var team = await _teamBusiness.GetSingleById(template.DefaultOwnerTeamId);
                                var teamOwner = await _teamBusiness.GetTeamOwner(team.Id);
                                service3.OwnerUserId = teamOwner.Id;
                            }
                        }
                        service3.RequestedByUserId = viewModel.AssignedToUserId;
                        service3.DataAction = DataActionEnum.Create;
                        var serviceModel3 = await _serviceBusiness.GetServiceDetails(service3);
                        serviceModel3.ServiceStatusCode = "SERVIE_STATUS_INPROGRESS";
                        serviceModel3.StartDate = DateTime.Now;
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        // set the udf value
                        ((IDictionary<String, Object>)exo).Add("externalServiceRequest", viewModel.ParentServiceId);
                        serviceModel3.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _serviceBusiness.ManageService(serviceModel3);
                    }

                }
            }
              
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> InitiateServiceForExternalService(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _serviceTemplateBusiness = sp.GetService<IServiceTemplateBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_TASK_STATUS");
            if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            {
                var service = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
                // get the udf values from the service

                DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(service.TemplateId, service.UdfNoteId);
                if (row != null)
                {                   
                   
                    IList<DynamicGridViewModel> items = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DynamicGridViewModel>>(row["InternalServices"].ToString());
                    if (items != null && items.Any())
                    {
                        foreach (var item in items)
                        {
                            //if (item.service.IsNotNullAndNotEmpty())
                            //{
                            //    var service1 = new ServiceTemplateViewModel();
                            //    service1.TemplateId = item.service;
                            //    var serviceModel1 = await _serviceBusiness.GetServiceDetails(service1);
                            //    serviceModel1.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                            //    await _serviceBusiness.ManageService(serviceModel1);
                            //}
                            if (item.service.IsNotNullAndNotEmpty())
                            {
                                var service1 = new ServiceTemplateViewModel();
                                var template = await _serviceTemplateBusiness.GetSingleById(item.service);
                                if (template.IsNotNull())
                                {
                                    var ownerType = await _lovBusiness.GetSingle(x => x.Id == template.DefaultServiceOwnerTypeId);
                                    if (ownerType.Code == "SERVICE_OWNER_TYPE_USER")
                                    {
                                        service1.OwnerUserId = template.DefaultOwnerUserId;
                                    }
                                    if (ownerType.Code == "SERVICE_OWNER_TYPE_TEAM")
                                    {
                                        var _teamBusiness = sp.GetService<ITeamBusiness>();
                                        var team = await _teamBusiness.GetSingleById(template.DefaultOwnerTeamId);
                                        var teamOwner = await _teamBusiness.GetTeamOwner(team.Id);
                                        service1.OwnerUserId = teamOwner.Id;
                                    }
                                }
                                service1.TemplateId = item.service;
                                service1.RequestedByUserId = viewModel.AssignedToUserId;
                                var serviceModel1 = await _serviceBusiness.GetServiceDetails(service1);
                                serviceModel1.DataAction = DataActionEnum.Create;
                                serviceModel1.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                                serviceModel1.StartDate = DateTime.Now;
                                serviceModel1.ServiceSubject = service.ServiceSubject;
                                serviceModel1.ParentServiceId = viewModel.ParentServiceId;
                                dynamic exo = new System.Dynamic.ExpandoObject();
                                // set the udf value
                                ((IDictionary<String, Object>)exo).Add("ExternalServiceRequestId", viewModel.ParentServiceId);
                                serviceModel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                await _serviceBusiness.ManageService(serviceModel1);
                            }
                        }
                    }
                    
                }
            }

            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

    }
}
