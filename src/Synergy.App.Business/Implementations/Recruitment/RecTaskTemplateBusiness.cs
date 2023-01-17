using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class RecTaskTemplateBusiness : BusinessBase<RecTaskTemplateViewModel, RecTaskTemplate>, IRecTaskTemplateBusiness
    {
        private readonly IRepositoryQueryBase<RecTaskTemplateViewModel> _queryRepo;
        ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepoIdname;
        private readonly IRepositoryQueryBase<EmailViewModel> _queryRepoEmail;
        public RecTaskTemplateBusiness(IRepositoryQueryBase<IdNameViewModel> queryRepoIdname
            , IRepositoryBase<RecTaskTemplateViewModel, RecTaskTemplate> repo, IMapper autoMapper,
            ITableMetadataBusiness tableMetadataBusiness,
            IRepositoryQueryBase<RecTaskTemplateViewModel> queryRepo, IRepositoryQueryBase<EmailViewModel> queryRepoEmail) : base(repo, autoMapper)
        {
            _tableMetadataBusiness = tableMetadataBusiness;
            _queryRepo = queryRepo;
            _queryRepoIdname = queryRepoIdname;
            _queryRepoEmail = queryRepoEmail;
        }

        public async override Task<CommandResult<RecTaskTemplateViewModel>> Create(RecTaskTemplateViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<RecTaskTemplateViewModel>(model);
            var validateTemplateCode = await IsNameExists(data);
            if (!validateTemplateCode.IsSuccess)
            {
                return CommandResult<RecTaskTemplateViewModel>.Instance(model, false, validateTemplateCode.Messages);
            }
            var result = await base.Create(data, autoCommit);           
            return CommandResult<RecTaskTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<RecTaskTemplateViewModel>> Edit(RecTaskTemplateViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<RecTaskTemplateViewModel>(model);
            var validateTemplateCode = await IsNameExists(data);
            if (!validateTemplateCode.IsSuccess)
            {
                return CommandResult<RecTaskTemplateViewModel>.Instance(model, false, validateTemplateCode.Messages);
            }
            var result = await base.Edit(data,autoCommit);            
            return CommandResult<RecTaskTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<RecTaskTemplateViewModel>> ManageCreate(RecTaskTemplateViewModel model, bool autoCommit = true)
        {
           
            var result = await base.Create(model,autoCommit);
         
            return CommandResult<RecTaskTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<RecTaskTemplateViewModel>> ManageEdit(RecTaskTemplateViewModel model, bool autoCommit = true)
        {
            
            var result = await base.Edit(model,autoCommit);
           
            return CommandResult<RecTaskTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<RecTaskTemplateViewModel>> IsNameExists(RecTaskTemplateViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.TemplateCode == null)
            {
                errorList.Add("TemplateCode", "Template Code is required.");
            }
            else
            {
                var template = await _repo.GetSingle(x => x.TemplateCode == model.TemplateCode && x.Id != model.Id && x.IsDeleted == false);
                if (template != null)
                {
                    errorList.Add("TemplateCode", "Template Code already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<RecTaskTemplateViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<RecTaskTemplateViewModel>.Instance();
        }

        public async Task<IList<IdNameViewModel>> GetEmailSetting()
        {

            string query = @$"select ""Id"" as Id,""SmtpUserId"" as Name
                          from rec.""EmailSetting"" where ""IsDeleted""=false ";
            var list = await _queryRepoIdname.ExecuteQueryList(query, null);
            return list;
        }
        public async Task<EmailViewModel> GetEmailSettingById(string id)
        {
            string query = @$"select *,""SmtpSenderName"" as SenderName
                          from rec.""EmailSetting"" where ""Id""='{id}' ";
            var list = await _queryRepoEmail.ExecuteQuerySingle<EmailViewModel>(query, null);
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetTaskTemplateList()
        {
            string query = @$"select tt.""Id"", t.""DisplayName"" as Name
                            from public.""TaskTemplate"" as tt
                            join public.""Template"" as t on t.""Id""=tt.""TemplateId""
                            where tt.""TaskTemplateType""=2 and tt.""IsDeleted"" = false ";
            var list = await _queryRepoIdname.ExecuteQueryList(query, null);
            return list;
        }
        
        public async Task<IList<IdNameViewModel>> GetAdhocTaskTemplateList()
        {
            string query = @$"select tt.""Id"", t.""DisplayName"" as Name from public.""TaskTemplate"" as tt
                                inner join public.""Template"" as t on t.""Id""=tt.""TemplateId"" and t.""TemplateType""=5
                                where tt.""TaskTemplateType"" not in (1,2,3)
                                order by tt.""CreatedDate"" desc ";
            var list = await _queryRepoIdname.ExecuteQueryList(query, null);
            return list;
        }
    }
}
