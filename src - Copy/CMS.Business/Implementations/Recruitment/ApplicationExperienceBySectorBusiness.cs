using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ApplicationExperienceBySectorBusiness : BusinessBase<ApplicationExperienceBySectorViewModel, ApplicationExperienceBySector>, IApplicationExperienceBySectorBusiness
    {
        private readonly IRepositoryQueryBase<ApplicationExperienceBySectorViewModel> _queryRepo;
        public ApplicationExperienceBySectorBusiness(IRepositoryBase<ApplicationExperienceBySectorViewModel, ApplicationExperienceBySector> repo, IMapper autoMapper,
            IRepositoryQueryBase<ApplicationExperienceBySectorViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<ApplicationExperienceBySectorViewModel>> Create(ApplicationExperienceBySectorViewModel model)
        {
            var data = _autoMapper.Map<ApplicationExperienceBySectorViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateEducationalViewModel>.Instance(model, false, validateName.Messages);
            //}
            //var res = await base.GetList(x => x.CandidateProfileId == model.CandidateProfileId);
            //if (res.Count == 0)
            //{
            //    data.IsLatest = true;
            //}
            var result = await base.Create(data);

            return CommandResult<ApplicationExperienceBySectorViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationExperienceBySectorViewModel>> Edit(ApplicationExperienceBySectorViewModel model)
        {
            //var data = _autoMapper.Map<CandidateExperienceBySectorViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateExperienceBySectorViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model);

            return CommandResult<ApplicationExperienceBySectorViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationExperienceBySectorViewModel>> IsNameExists(ApplicationExperienceBySectorViewModel model)
        {
                        
            return CommandResult<ApplicationExperienceBySectorViewModel>.Instance();
        }

        public async Task<IList<ApplicationExperienceBySectorViewModel>> GetListByApplication(string candidateProfileId)
        {
            string query = @$"SELECT l.*,
                            s.""Name"" as SectorName, i.""Name"" as IndustryName, c.""Name"" as CategoryName
                                FROM rec.""ApplicationExperienceBySector"" as l
                                LEFT JOIN rec.""ListOfValue"" as s ON s.""Id"" = l.""Sector""
                                LEFT JOIN rec.""ListOfValue"" as i ON i.""Id"" = l.""Industry""
                                LEFT JOIN rec.""ListOfValue"" as c ON c.""Id"" = l.""Category""
                                WHERE l.""ApplicationId"" = '{candidateProfileId}' and l.""IsDeleted"" = false";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
    }
}
