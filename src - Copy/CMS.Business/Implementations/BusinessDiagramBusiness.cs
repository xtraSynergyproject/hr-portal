using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class BusinessDiagramBusiness : BusinessBase<TaskViewModel, NtsTask>, IBusinessDiagramBusiness
    {
        private readonly ITaskBusiness _taskBusiness;
        private readonly INtsTaskPrecedenceBusiness _ntsTaskPrecedenceBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;

        public BusinessDiagramBusiness(IRepositoryBase<TaskViewModel, NtsTask> repo, IMapper autoMapper,
            ITaskBusiness taskBusiness,
            INtsTaskPrecedenceBusiness ntsTaskPrecedenceBusiness,
            ITableMetadataBusiness tableMetadataBusiness) : base(repo, autoMapper)
        {
            _taskBusiness = taskBusiness;
            _ntsTaskPrecedenceBusiness = ntsTaskPrecedenceBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
        }

        public Task<CommandResult<TaskViewModel>> Create(TaskViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<VM>> Create<VM, DM>(VM model)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<TaskViewModel>> CreateGlobal(TaskViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<VM>> CreateGlobal<VM, DM>(VM model)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskViewModel>> CreateMany(List<TaskViewModel> models)
        {
            throw new NotImplementedException();
        }

        public Task<List<VM>> CreateMany<VM, DM>(List<VM> models)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<TaskViewModel>> CreateMigrate(TaskViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<VM>> CreateMigrate<VM, DM>(VM model)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task Delete<VM, DM>(string id)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<TaskViewModel>> Edit(TaskViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<VM>> Edit<VM, DM>(VM model)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<TaskViewModel>> EditGlobal(TaskViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<VM>> EditGlobal<VM, DM>(VM model)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskViewModel>> GetActiveList()
        {
            throw new NotImplementedException();
        }

        public Task<List<VM>> GetActiveList<VM, DM>()
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskViewModel>> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskViewModel>> GetList(Expression<Func<NtsTask, bool>> where, params Expression<Func<NtsTask, object>>[] include)
        {
            throw new NotImplementedException();
        }

        public Task<List<VM>> GetList<VM, DM>()
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<List<VM>> GetList<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskViewModel>> GetListGlobal()
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskViewModel>> GetListGlobal(Expression<Func<NtsTask, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<List<VM>> GetListGlobal<VM, DM>()
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<List<VM>> GetListGlobal<VM, DM>(Expression<Func<DM, bool>> where)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<TaskViewModel> GetSingle(Expression<Func<NtsTask, bool>> where, params Expression<Func<NtsTask, object>>[] include)
        {
            throw new NotImplementedException();
        }

        public Task<VM> GetSingle<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<TaskViewModel> GetSingleById(string id, params Expression<Func<NtsTask, object>>[] include)
        {
            throw new NotImplementedException();
        }

        public Task<VM> GetSingleById<VM, DM>(string id, params Expression<Func<DM, object>>[] include)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public Task<TaskViewModel> GetSingleGlobal(Expression<Func<NtsTask, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<VM> GetSingleGlobal<VM, DM>(Expression<Func<DM, bool>> where)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ManageGenericDiagramTask(BusinessDiagramViewModel model)
        {
            try
            {
                var taskId = "";
                var data = await _tableMetadataBusiness.GetTableDataByColumn("BUSINESS_DIAGRAM", null, "diagramTemplateId", model.diagramTemplateId);
                if (data != null)
                {
                    var noteId = Convert.ToString(data["NtsNoteId"]);
                    if (noteId.IsNotNullAndNotEmpty())
                    {
                        var task = await _repo.GetSingle<TaskViewModel, NtsTask>(x => x.UdfNoteId == noteId);
                        if (task.IsNotNull())
                        {
                            taskId = task.Id;
                        }
                    }
                }

                var mappingParent = new List<IdNameViewModel>();
                var bDiagram = new TaskTemplateViewModel
                {
                    ActiveUserId = _repo.UserContext.UserId,
                    TemplateCode = "BUSINESS_DIAGRAM",
                    TaskId = taskId
                };
                var bDiagramTask = await _taskBusiness.GetTaskDetails(bDiagram);
                bDiagramTask.TaskSubject = model.TaskSubject;
                bDiagramTask.OwnerUserId = _repo.UserContext.UserId;
                bDiagramTask.StartDate = DateTime.Now;
                bDiagramTask.DueDate = DateTime.Now;
                bDiagramTask.AssignedToUserId = _repo.UserContext.UserId;
                if (taskId.IsNullOrEmpty())
                {
                    bDiagramTask.DataAction = DataActionEnum.Create;
                }
                else
                {
                    bDiagramTask.DataAction = DataActionEnum.Edit;
                }
                model.CreatedBy = _repo.UserContext.UserId;
                model.CompanyId = _repo.UserContext.CompanyId;
                model.CreatedDate = DateTime.Now;
                model.LastUpdatedBy = _repo.UserContext.UserId;
                model.LastUpdatedDate = DateTime.Now;
                bDiagramTask.Json = JsonConvert.SerializeObject(model); //model.Json;
                var busDiagram = await _taskBusiness.ManageTask(bDiagramTask);

                foreach (var n in model.nodeIds)
                {
                    var details = await _taskBusiness.GetSingleById(n.Id);
                    var nbDiagram = new TaskTemplateViewModel
                    {
                        ActiveUserId = _repo.UserContext.UserId,
                        TemplateCode = "BUSINESS_DIAGRAM_NODE",
                        TaskId = n.Id
                    };

                    var nbDiagramTask = await _taskBusiness.GetTaskDetails(nbDiagram);
                    nbDiagramTask.TaskSubject = n.Name;
                    nbDiagramTask.OwnerUserId = _repo.UserContext.UserId;
                    nbDiagramTask.StartDate = DateTime.Now;
                    nbDiagramTask.DueDate = DateTime.Now;
                    nbDiagramTask.ParentTaskId = busDiagram.Item.TaskId;
                    nbDiagramTask.AssignedToUserId = _repo.UserContext.UserId;
                    if (details.IsNotNull())
                    {
                        nbDiagramTask.DataAction = DataActionEnum.Edit;
                    }
                    else
                    {
                        nbDiagramTask.DataAction = DataActionEnum.Create;
                    }
                    model.CreatedBy = _repo.UserContext.UserId;
                    model.CompanyId = _repo.UserContext.CompanyId;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdatedBy = _repo.UserContext.UserId;
                    model.LastUpdatedDate = DateTime.Now;
                    nbDiagramTask.Json = "{}"; //model.Json;
                    nbDiagramTask.TaskId = n.Id;
                    var nbusDiagram = await _taskBusiness.ManageTask(nbDiagramTask);
                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return true;
        }

        public async Task<bool> ManageBusinessDiagramTask(BusinessDiagramViewModel model)
        {
            try
            {
                var taskId = "";
                var data = await _tableMetadataBusiness.GetTableDataByColumn("BUSINESS_DIAGRAM", null, "diagramTemplateId", model.diagramTemplateId);
                if (data != null)
                {
                    var noteId = Convert.ToString(data["NtsNoteId"]);
                    if (noteId.IsNotNullAndNotEmpty())
                    {
                        var task = await _repo.GetSingle<TaskViewModel, NtsTask>(x => x.UdfNoteId == noteId);
                        if (task.IsNotNull())
                        {
                            taskId = task.Id;
                        }
                    }
                }

                var mappingParent = new List<IdNameViewModel>();
                var bDiagram = new TaskTemplateViewModel
                {
                    ActiveUserId = _repo.UserContext.UserId,
                    TemplateCode = "BUSINESS_DIAGRAM",
                    TaskId = taskId
                };
                var bDiagramTask = await _taskBusiness.GetTaskDetails(bDiagram);
                bDiagramTask.TaskSubject = model.TaskSubject;
                bDiagramTask.OwnerUserId = _repo.UserContext.UserId;
                bDiagramTask.StartDate = DateTime.Now;
                bDiagramTask.DueDate = DateTime.Now;
                bDiagramTask.AssignedToUserId = _repo.UserContext.UserId;
                if (taskId.IsNullOrEmpty())
                {
                    bDiagramTask.DataAction = DataActionEnum.Create;
                }
                else
                {
                    bDiagramTask.DataAction = DataActionEnum.Edit;
                }
                model.CreatedBy = _repo.UserContext.UserId;
                model.CompanyId = _repo.UserContext.CompanyId;
                model.CreatedDate = DateTime.Now;
                model.LastUpdatedBy = _repo.UserContext.UserId;
                model.LastUpdatedDate = DateTime.Now;
                bDiagramTask.Json = JsonConvert.SerializeObject(model); //model.Json;

                var busDiagram = await _taskBusiness.ManageTask(bDiagramTask);
            }
            catch (Exception ex)
            {
                throw;

            }
            return true;
        }
    }
}
