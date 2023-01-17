using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class NoteIndexPageColumnBusiness : BusinessBase<NoteIndexPageColumnViewModel, NoteIndexPageColumn>, INoteIndexPageColumnBusiness
    {
        public NoteIndexPageColumnBusiness(IRepositoryBase<NoteIndexPageColumnViewModel, NoteIndexPageColumn> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<NoteIndexPageColumnViewModel>> Create(NoteIndexPageColumnViewModel model, bool autoCommit = true)
        {
         
            var result = await base.Create(model,autoCommit);
            return CommandResult<NoteIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NoteIndexPageColumnViewModel>> Edit(NoteIndexPageColumnViewModel model, bool autoCommit = true)
        {
           
            var result = await base.Edit(model,autoCommit);
            return CommandResult<NoteIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
