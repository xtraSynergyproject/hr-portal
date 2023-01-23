using CMS.Business.Interface.BusinessScript.Task.General.PerformanceManagement;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Task.General.PerformanceManagement
{
    public class TaskGeneralPerformanceManagementPostScript : ITaskGeneralPerformanceManagementPostScript
    {
        /// <summary>
        /// AddTagging - it will tag task to relevent goal and compentency
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> AddTagging(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _ntsTagBusiness = sp.GetService<INtsTagBusiness>();
            var model = new NtsTagViewModel();
            model.TagId = viewModel.ParentServiceId;
            model.NtsType = NtsTypeEnum.Task;
            model.NtsId = viewModel.TaskId;
            //var tC = await _noteBusiness.GetCategoryByTagId(viewModel.ParentServiceId);
            //model.TagCategoryId = tC.Id;

            var result = await _ntsTagBusiness.Create(model);
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Employee Goal Mid Year Rating
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateEmpGoalMidYearRating(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {           
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            //DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var goalService = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var goaludftabledata = await _tableMetadataBusiness.GetTableDataByColumn("SN_PMS_GOAL",null,"NtsNoteId", goalService.UdfNoteId);
            var GoalId = Convert.ToString(goaludftabledata["Id"]);
            DataRow row = await _tableMetadataBusiness.GetTableDataByColumn("PMS_GOAL_RESULT", null, "GoalId", GoalId);
            if (row != null)
            {
                // Update the result table for Employee Rating and its Comment
                var noteTempModel = new NoteTemplateViewModel();                
                noteTempModel.NoteId =Convert.ToString(row["NtsNoteId"]);
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var k = rowData1.FirstOrDefault(x => x.Key == "EmployeeRatingId" || x.Key == "EmployeeRatingId");
                if (rowData1.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k.Key] = Convert.ToString(rowData["RatingId"]);    
                }
                var k1 = rowData1.FirstOrDefault(x => x.Key == "EmployeeComments" || x.Key == "EmployeeComments");
                if (rowData1.IsNotNull() && k.IsNotNull() && k1.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k1.Key] = Convert.ToString(rowData["Comment"]);
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
               
            }
            else
            {
                // Create goal result table for Employee
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = "PMS_GOAL_RESULT";               
                noteTempModel.ActiveUserId = uc.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.DataAction = DataActionEnum.Create;
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("GoalId", GoalId);
                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", rowData["DocumentMasterId"]);
                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowData["DocumentMasterStageId"]);
                ((IDictionary<String, Object>)exo).Add("EmployeeRatingId", rowData["RatingId"]);
                ((IDictionary<String, Object>)exo).Add("EmployeeComments", rowData["Comment"]);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var result = await _noteBusiness.ManageNote(notemodel);
            }
            // Update Goal Table For Employee Rating
            if (goalService.UdfNoteId.IsNotNullAndNotEmpty()) 
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = goalService.UdfNoteId;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var k = rowData1.FirstOrDefault(x => x.Key == "EmployeeRating" || x.Key == "EmployeeRating");
                if (rowData1.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k.Key] = Convert.ToString(rowData["RatingId"]);
                }
                var k1 = rowData1.FirstOrDefault(x => x.Key == "EmployeeComments" || x.Key == "EmployeeComments");
                if (rowData1.IsNotNull() && k.IsNotNull() && k1.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k1.Key] = Convert.ToString(rowData["Comment"]);
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            }

            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Employee Compentency Mid Year Rating
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateEmpCompentencyMidYearRating(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            //DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var CompetencyService = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var Competencyudftabledata = await _tableMetadataBusiness.GetTableDataByColumn("SN_PMS_COMPENTENCY", null, "NtsNoteId", CompetencyService.UdfNoteId);
            var CompetencyId = Convert.ToString(Competencyudftabledata["Id"]);
            DataRow row = await _tableMetadataBusiness.GetTableDataByColumn("PMS_COMPETENCY_RESULT", null, "CompetencyId", CompetencyId);
            if (row != null)
            {
                // Update the result table for Employee Rating and its Comment
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = Convert.ToString(row["NtsNoteId"]);
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var k = rowData1.FirstOrDefault(x => x.Key == "EmployeeRatingId" || x.Key == "EmployeeRatingId");
                if (rowData1.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k.Key] = Convert.ToString(rowData["RatingId"]);
                }
                var k1 = rowData1.FirstOrDefault(x => x.Key == "EmployeeComments" || x.Key == "EmployeeComments");
                if (rowData1.IsNotNull() && k.IsNotNull() && k1.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k1.Key] = Convert.ToString(rowData["Comment"]);
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            }
            else
            {
                // Create goal result table for Employee
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = "PMS_COMPETENCY_RESULT";
                noteTempModel.ActiveUserId = uc.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.DataAction = DataActionEnum.Create;
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("CompetencyId", CompetencyId);
                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", rowData["DocumentMasterId"]);
                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowData["DocumentMasterStageId"]);
                ((IDictionary<String, Object>)exo).Add("EmployeeRatingId", rowData["RatingId"]);
                ((IDictionary<String, Object>)exo).Add("EmployeeComments", rowData["Comment"]);
                notemodel.Json= Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var result = await _noteBusiness.ManageNote(notemodel);
            }
            // Update Competency Table For Employee Rating
            if (CompetencyService.UdfNoteId.IsNotNullAndNotEmpty())
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = CompetencyService.UdfNoteId;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var k = rowData1.FirstOrDefault(x => x.Key == "EmployeeRating" || x.Key == "EmployeeRating");
                if (rowData1.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k.Key] = Convert.ToString(rowData["RatingId"]);
                }
                var k1 = rowData1.FirstOrDefault(x => x.Key == "EmployeeComment" || x.Key == "EmployeeComment");
                if (rowData1.IsNotNull() && k.IsNotNull() && k1.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k1.Key] = Convert.ToString(rowData["Comment"]);
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
            #region Previous Code
            //var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            //var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();

            //DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            //if (row != null)
            //{
            //    string rowValue = row["RatingId"].ToString();
            //    await _pmsBusiness.updateCompentancyRating(viewModel.UdfNoteId, rowValue, "EMP_MID_YEAR");
            //}
            //return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
            #endregion
        }
        /// <summary>
        /// Update Manager Goal Mid Year Rating
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateManagerGoalMidYearRating(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            //DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var goalService = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var goaludftabledata = await _tableMetadataBusiness.GetTableDataByColumn("SN_PMS_GOAL", null, "NtsNoteId", goalService.UdfNoteId);
            var GoalId = Convert.ToString(goaludftabledata["Id"]);
            DataRow row = await _tableMetadataBusiness.GetTableDataByColumn("PMS_GOAL_RESULT", null, "GoalId", GoalId);
            if (row != null)
            {
                // Update the result table for Employee Rating and its Comment
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = Convert.ToString(row["NtsNoteId"]);
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var k = rowData1.FirstOrDefault(x => x.Key == "ManagerRatingId" || x.Key == "ManagerRatingId");
                if (rowData1.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k.Key] = Convert.ToString(rowData["RatingId"]);
                }
                var k1 = rowData1.FirstOrDefault(x => x.Key == "ManagerComments" || x.Key == "ManagerComments");
                if (rowData1.IsNotNull() && k.IsNotNull() && k1.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k1.Key] = Convert.ToString(rowData["Comment"]);
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            }
            else
            {
                // Create goal result table for Employee
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = "PMS_GOAL_RESULT";
                noteTempModel.ActiveUserId = uc.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.DataAction = DataActionEnum.Create;
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("GoalId", GoalId);
                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", rowData["DocumentMasterId"]);
                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowData["DocumentMasterStageId"]);
                ((IDictionary<String, Object>)exo).Add("ManagerRatingId", rowData["RatingId"]);
                ((IDictionary<String, Object>)exo).Add("ManagerComments", rowData["Comment"]);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var result = await _noteBusiness.ManageNote(notemodel);
            }
            // Update Goal Table For Manager Rating
            if (goalService.UdfNoteId.IsNotNullAndNotEmpty())
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = goalService.UdfNoteId;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var k = rowData1.FirstOrDefault(x => x.Key == "ManagerRating" || x.Key == "ManagerRating");
                if (rowData1.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k.Key] = Convert.ToString(rowData["RatingId"]);
                }
                var k1 = rowData1.FirstOrDefault(x => x.Key == "ManagerComments" || x.Key == "ManagerComments");
                if (rowData1.IsNotNull() && k.IsNotNull() && k1.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k1.Key] = Convert.ToString(rowData["Comment"]);
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
            //var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            //var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();

            //DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            //if (row != null)
            //{
            //    string rowValue = row["RatingId"].ToString();
            //    await _pmsBusiness.updateGoalRating(viewModel.UdfNoteId, rowValue, "MNG_MID_YEAR");
            //}
            //return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Manager Compentency Mid Year Rating
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateManagerCompentencyMidYearRating(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            //DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var CompetencyService = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var Competencyudftabledata = await _tableMetadataBusiness.GetTableDataByColumn("SN_PMS_COMPENTENCY", null, "NtsNoteId", CompetencyService.UdfNoteId);
            var CompetencyId = Convert.ToString(Competencyudftabledata["Id"]);
            DataRow row = await _tableMetadataBusiness.GetTableDataByColumn("PMS_COMPETENCY_RESULT", null, "CompetencyId", CompetencyId);
            if (row != null)
            {
                // Update the result table for Employee Rating and its Comment
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = Convert.ToString(row["NtsNoteId"]);
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var k = rowData1.FirstOrDefault(x => x.Key == "ManagerRatingId" || x.Key == "ManagerRatingId");
                if (rowData1.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k.Key] = Convert.ToString(rowData["RatingId"]);
                }
                var k1 = rowData1.FirstOrDefault(x => x.Key == "ManagerComments" || x.Key == "ManagerComments");
                if (rowData1.IsNotNull() && k.IsNotNull() && k1.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k1.Key] = Convert.ToString(rowData["Comment"]);
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            }
            else
            {
                // Create goal result table for Employee
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = "PMS_COMPETENCY_RESULT";
                noteTempModel.ActiveUserId = uc.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.DataAction = DataActionEnum.Create;
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("CompetencyId", CompetencyId);
                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", rowData["DocumentMasterId"]);
                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowData["DocumentMasterStageId"]);
                ((IDictionary<String, Object>)exo).Add("ManagerRatingId", rowData["RatingId"]);
                ((IDictionary<String, Object>)exo).Add("ManagerComments", rowData["Comment"]);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var result = await _noteBusiness.ManageNote(notemodel);
            }
            // Update Competency Table For Manager Rating
            if (CompetencyService.UdfNoteId.IsNotNullAndNotEmpty())
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = CompetencyService.UdfNoteId;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var k = rowData1.FirstOrDefault(x => x.Key == "ManagerRating" || x.Key == "ManagerRating");
                if (rowData1.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k.Key] = Convert.ToString(rowData["RatingId"]);
                }
                var k1 = rowData1.FirstOrDefault(x => x.Key == "ManagerComment" || x.Key == "ManagerComment");
                if (rowData1.IsNotNull() && k.IsNotNull() && k1.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k1.Key] = Convert.ToString(rowData["Comment"]);
                }
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
            #region
            //var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            //var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();

            //DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            //if (row != null)
            //{
            //    string rowValue = row["RatingId"].ToString();
            //    await _pmsBusiness.updateCompentancyRating(viewModel.UdfNoteId, rowValue, "MNG_MID_YEAR");
            //}
            //return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
            #endregion
        }

        /// <summary>
        ///  Update Employee Goal End Year Rating
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateEmpGoalEndYearRating(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();

            DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            if (row != null)
            {
                string rowValue = row["RatingId"].ToString();
                await _pmsBusiness.updateGoalRating(viewModel.UdfNoteId, rowValue, "EMP_END_YEAR");
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Employee Compentency End Year Rating
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateEmpCompentencyEndYearRating(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();

            DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            if (row != null)
            {
                string rowValue = row["RatingId"].ToString();
                await _pmsBusiness.updateCompentancyRating(viewModel.UdfNoteId, rowValue, "EMP_END_YEAR");
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Manager Goal End Year Rating
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateManagerGoalEndYearRating(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();

            DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            if (row != null)
            {
                string rowValue = row["RatingId"].ToString();
                await _pmsBusiness.updateGoalRating(viewModel.UdfNoteId, rowValue, "MNG_END_YEAR");
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Manager Compentency End Year Rating
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> UpdateManagerCompentencyEndYearRating(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();

            DataRow row = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            if (row != null)
            {
                string rowValue = row["RatingId"].ToString();
                await _pmsBusiness.updateCompentancyRating(viewModel.UdfNoteId, rowValue, "MNG_END_YEAR");
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
      
        public async Task<CommandResult<TaskTemplateViewModel>> FreezePerformanceDocumentTask(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _pmsBusiness = sp.GetService<IPerformanceManagementBusiness>();
            var data = await _pmsBusiness.GetPerformanceDocumentMasterByServiceId(viewModel.ParentServiceId);
            if(data!=null)
            {
                if (data.DocumentStatus == PerformanceDocumentStatusEnum.Freezed)
                {
                    var errorList = new Dictionary<string, string>();
                    errorList.Add("Freeze", "The Performance Document is Freezed.");
                    return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, errorList);
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
    }
}
