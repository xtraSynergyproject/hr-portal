using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class TaskIndexPageColumnBusiness : BusinessBase<TaskIndexPageColumnViewModel, TaskIndexPageColumn>, ITaskIndexPageColumnBusiness
    {
        public TaskIndexPageColumnBusiness(IRepositoryBase<TaskIndexPageColumnViewModel, TaskIndexPageColumn> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<TaskIndexPageColumnViewModel>> Create(TaskIndexPageColumnViewModel model)
        {
         
            var result = await base.Create(model);
            return CommandResult<TaskIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TaskIndexPageColumnViewModel>> Edit(TaskIndexPageColumnViewModel model)
        {
           
            var result = await base.Edit(model);
            return CommandResult<TaskIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
