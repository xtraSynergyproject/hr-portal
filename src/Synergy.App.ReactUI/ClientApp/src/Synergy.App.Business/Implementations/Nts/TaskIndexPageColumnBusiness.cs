using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class TaskIndexPageColumnBusiness : BusinessBase<TaskIndexPageColumnViewModel, TaskIndexPageColumn>, ITaskIndexPageColumnBusiness
    {
        public TaskIndexPageColumnBusiness(IRepositoryBase<TaskIndexPageColumnViewModel, TaskIndexPageColumn> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<TaskIndexPageColumnViewModel>> Create(TaskIndexPageColumnViewModel model, bool autoCommit = true)
        {
         
            var result = await base.Create(model,autoCommit);
            return CommandResult<TaskIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TaskIndexPageColumnViewModel>> Edit(TaskIndexPageColumnViewModel model, bool autoCommit = true)
        {
           
            var result = await base.Edit(model,autoCommit);
            return CommandResult<TaskIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
