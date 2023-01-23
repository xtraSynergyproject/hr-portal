using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class NoteIndexPageColumnBusiness : BusinessBase<NoteIndexPageColumnViewModel, NoteIndexPageColumn>, INoteIndexPageColumnBusiness
    {
        public NoteIndexPageColumnBusiness(IRepositoryBase<NoteIndexPageColumnViewModel, NoteIndexPageColumn> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<NoteIndexPageColumnViewModel>> Create(NoteIndexPageColumnViewModel model)
        {
         
            var result = await base.Create(model);
            return CommandResult<NoteIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NoteIndexPageColumnViewModel>> Edit(NoteIndexPageColumnViewModel model)
        {
           
            var result = await base.Edit(model);
            return CommandResult<NoteIndexPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
