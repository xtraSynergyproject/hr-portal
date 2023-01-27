using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ApplicationDocumentBusiness : BusinessBase<ApplicationDocumentViewModel, ApplicationDocument>, IApplicationDocumentBusiness
    {
        IRepositoryQueryBase<ApplicationDocumentViewModel> _repoQuery;
        public ApplicationDocumentBusiness(IRepositoryBase<ApplicationDocumentViewModel, ApplicationDocument> repo, IMapper autoMapper
            , IRepositoryQueryBase<ApplicationDocumentViewModel> repoQuery) : base(repo, autoMapper)
        {
            _repoQuery = repoQuery;
        }

        public async override Task<CommandResult<ApplicationDocumentViewModel>> Create(ApplicationDocumentViewModel model)
        {
            var data = _autoMapper.Map<ApplicationDocumentViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<ApplicationDocumentViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data);

            return CommandResult<ApplicationDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationDocumentViewModel>> Edit(ApplicationDocumentViewModel model)
        {
            var data = _autoMapper.Map<ApplicationDocumentViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<ApplicationDocumentViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(data);

            return CommandResult<ApplicationDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

            public async Task<List<ApplicationDocumentViewModel>> GetApplicationDocumentList()
            {
                var query = @$"select ad.*,f.""FileName"" as DocumentFileName
                from public.""ApplicationDocument"" as ad
                left join public.""File"" as f on ad.""DocumentId""=f.""Id"" and f.""IsDeleted""=false            
                where ad.""IsDeleted""=false";
                return await _repoQuery.ExecuteQueryList<ApplicationDocumentViewModel>(query, null);
            }
        public async Task<ApplicationDocumentViewModel> GetApplicationDocument(string documentCode)
        {
            var query = @$"select ad.*,f.""FileName"" as DocumentFileName
            from public.""ApplicationDocument"" as ad
            left join public.""File"" as f on ad.""DocumentId""=f.""Id"" and f.""IsDeleted""=false            
            where ad.""IsDeleted""=false";
            return null;
        }

        private async Task<CommandResult<ApplicationDocumentViewModel>> IsNameExists(ApplicationDocumentViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.DocumentName.IsNullOrEmpty())
            {
                errorList.Add("Name", "Document Name is required.");
            }
            else
            {
                var name = await _repo.GetSingle(x => x.Code.ToLower() == model.Code.ToLower() && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Name", "Code already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<ApplicationDocumentViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<ApplicationDocumentViewModel>.Instance();
        }
    }
}
