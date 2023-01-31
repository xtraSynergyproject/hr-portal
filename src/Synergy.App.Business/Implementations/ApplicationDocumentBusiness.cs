using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ApplicationDocumentBusiness : BusinessBase<ApplicationDocumentViewModel, ApplicationDocument>, IApplicationDocumentBusiness
    {
        IRepositoryQueryBase<ApplicationDocumentViewModel> _repoQuery;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public ApplicationDocumentBusiness(IRepositoryBase<ApplicationDocumentViewModel, ApplicationDocument> repo, IMapper autoMapper
            , IRepositoryQueryBase<ApplicationDocumentViewModel> repoQuery, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _repoQuery = repoQuery;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<ApplicationDocumentViewModel>> Create(ApplicationDocumentViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<ApplicationDocumentViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<ApplicationDocumentViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data, autoCommit);

            return CommandResult<ApplicationDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationDocumentViewModel>> Edit(ApplicationDocumentViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<ApplicationDocumentViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<ApplicationDocumentViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(data, autoCommit);

            return CommandResult<ApplicationDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<ApplicationDocumentViewModel>> GetApplicationDocumentList()
        {

            var data = await _cmsQueryBusiness.GetApplicationDocumentListData();
            return data;
        }
        public async Task<ApplicationDocumentViewModel> GetApplicationDocument(string documentCode)
        {
            var data = await _cmsQueryBusiness.GetApplicationDocumentData(documentCode);
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
