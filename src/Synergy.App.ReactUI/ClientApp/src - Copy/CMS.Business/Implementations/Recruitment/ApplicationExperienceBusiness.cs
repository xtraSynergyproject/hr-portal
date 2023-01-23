using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ApplicationExperienceBusiness : BusinessBase<ApplicationExperienceViewModel, ApplicationExperience>, IApplicationExperienceBusiness
    {
        private readonly IRepositoryQueryBase<ApplicationExperienceViewModel> _queryRepo;
        public ApplicationExperienceBusiness(IRepositoryBase<ApplicationExperienceViewModel, ApplicationExperience> repo, IMapper autoMapper,
            IRepositoryQueryBase<ApplicationExperienceViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<ApplicationExperienceViewModel>> Create(ApplicationExperienceViewModel model)
        {
            var data = _autoMapper.Map<ApplicationExperienceViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateEducationalViewModel>.Instance(model, false, validateName.Messages);
            //}
            //var res = await base.GetList(x => x.CandidateProfileId == model.CandidateProfileId);
            //if(res.Count == 0)
            //{
            //    data.IsLatest = true;
            //}
            var result = await base.Create(data);

            return CommandResult<ApplicationExperienceViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationExperienceViewModel>> Edit(ApplicationExperienceViewModel model)
        {
            var data = _autoMapper.Map<ApplicationExperienceViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateEducationalViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(data);

            return CommandResult<ApplicationExperienceViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationExperienceViewModel>> IsNameExists(ApplicationExperienceViewModel model)
        {                        
            return CommandResult<ApplicationExperienceViewModel>.Instance();
        }
        public async Task<IList<ApplicationExperienceViewModel>> GetListByApplication(string candidateProfileId)
        {
            string query = @$"SELECT a.*, f.""FileName"" as AttachmentName	
                                FROM rec.""ApplicationExperience"" as a                                
                                LEFT JOIN public.""File"" as f ON f.""Id"" = a.""AttachmentId""
                                where a.""ApplicationId""='{candidateProfileId}' and a.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            //var list = new List<CandidateEducationalViewModel>();

            return queryData;
        }
    }
}
