using Synergy.App.Business.Interface.BusinessScript.Task.General.PerformanceManagement;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.PerformanceManagement
{
    public class TaskGeneralPerformanceManagementPreScript : ITaskGeneralPerformanceManagementPreScript
    {
        /// <summary>
        /// Stop all action of task for Freezed Performance Document Master
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> FreezePerformanceDocumentTask(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            var data = await _pmsBusiness.GetPerformanceDocumentMasterByServiceId(viewModel.ParentServiceId);
            if (data != null)
            {
                if (data.DocumentStatus == PerformanceDocumentStatusEnum.Freezed)
                {
                    var errorList = new Dictionary<string, string>();
                    errorList.Add("Freeze", "The Performance Document Master is Freezed.");
                    return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, errorList);
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        /// <summary>
        /// Update ServiceOwnerUser as StepTaskAssignee
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateStepTaskAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serBus = sp.GetService<IServiceBusiness>();
            var _taskBus = sp.GetService<ITaskBusiness>();
            var data = await _serBus.GetSingleById(viewModel.ParentServiceId);
            if (data != null)
            {
                //if (data.OwnerUserId.IsNullOrEmpty())
                //{
                //    var errorList = new Dictionary<string, string>();
                //    errorList.Add("Freeze", "The Performance Document Master is Freezed.");
                //    return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, errorList);
                //}
                await _taskBus.UpdateStepTaskAssignee(viewModel.TaskId, data.OwnerUserId);
                return CommandResult<TaskTemplateViewModel>.Instance(viewModel, true, "");
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> CheckWeightageOfEmployeeReview(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            var stage = await _pmsBusiness.GetPerformanceDocumentStage(Convert.ToString(rowData["DocumentStageId"]));           
            var goals = await _pmsBusiness.GetAllApprovedGoals(stage.OwnerUserId, stage.ParentServiceId,Convert.ToString(rowData["DocumentStageId"]));
            var Competencies = await _pmsBusiness.GetAllApprovedCompetencies(stage.OwnerUserId, stage.ParentServiceId, Convert.ToString(rowData["DocumentStageId"]));
            var goalWeightage = goals.Count() > 0 ? goals.Sum(x => x.Weightage) : 0;
            var competencyWeightage = Competencies.Count() > 0 ? Competencies.Sum(x => x.Weightage) : 0;
            var errorList = new Dictionary<string, string>();
            bool IsSuccess = true;
            if (goalWeightage!=100) 
            {
                IsSuccess = false;
                errorList.Add("Weightage", "The Sum of the Weightage of goals should be 100");
            }
            if (competencyWeightage != 100) 
            {
                IsSuccess = false;
                errorList.Add("Weightage", "The Sum of the Weightage of competencies should be 100");
            }
            if (goals.Any(x=>x.EmployeeRating == null || x.ManagerRating == ""))
            {
                IsSuccess = false;
                errorList.Add("Weightage", "Please review all the goals");
            }
            if (Competencies.Any(x => x.EmployeeRating == null || x.ManagerRating == ""))
            {
                IsSuccess = false;
                errorList.Add("Weightage", "Please review all the competencies");
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel, IsSuccess, errorList);          
        }
        public async Task<CommandResult<TaskTemplateViewModel>> CheckWeightageOfManagerReview(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            var stage = await _pmsBusiness.GetPerformanceDocumentStage(Convert.ToString(rowData["DocumentStageId"]));
            var goals = await _pmsBusiness.GetAllApprovedGoals(stage.OwnerUserId, stage.ParentServiceId, Convert.ToString(rowData["DocumentStageId"]));
            var Competencies = await _pmsBusiness.GetAllApprovedCompetencies(stage.OwnerUserId, stage.ParentServiceId, Convert.ToString(rowData["DocumentStageId"]));
            var goalWeightage = goals.Count()>0? goals.Sum(x => x.Weightage):0;
            var competencyWeightage = Competencies.Count() > 0 ? Competencies.Sum(x => x.Weightage):0;
            var errorList = new Dictionary<string, string>();
            bool IsSuccess = true;
            if (goalWeightage != 100)
            {
                IsSuccess = false;
                errorList.Add("Weightage", "The Sum of the Weightage of goals should be 100");
            }
            if (competencyWeightage != 100)
            {
                IsSuccess = false;
                errorList.Add("Weightage", "The Sum of the Weightage of competencies should be 100");
            }
            if (goals.Any(x => x.ManagerRating==null || x.ManagerRating==""))
            {
                IsSuccess = false;
                errorList.Add("Weightage", "Please review all the goals");
            }
            if (Competencies.Any(x => x.ManagerRating == null || x.ManagerRating == ""))
            {
                IsSuccess = false;
                errorList.Add("Weightage", "Please review all the competencies");
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel, IsSuccess, errorList);          
        }

    }
}
