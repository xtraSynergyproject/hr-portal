﻿using AutoMapper;
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
    public class ApplicationExperienceByCountryBusiness : BusinessBase<ApplicationExperienceByCountryViewModel, ApplicationExperienceByCountry>, IApplicationExperienceByCountryBusiness
    {
        private readonly IRepositoryQueryBase<ApplicationExperienceByCountryViewModel> _queryRepo;
        public ApplicationExperienceByCountryBusiness(IRepositoryBase<ApplicationExperienceByCountryViewModel, ApplicationExperienceByCountry> repo, IMapper autoMapper,
            IRepositoryQueryBase<ApplicationExperienceByCountryViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<ApplicationExperienceByCountryViewModel>> Create(ApplicationExperienceByCountryViewModel model)
        {           
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateExperienceByCountryViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model);

            return CommandResult<ApplicationExperienceByCountryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationExperienceByCountryViewModel>> Edit(ApplicationExperienceByCountryViewModel model)
        {
            //var data = _autoMapper.Map<CandidateExperienceByCountryViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateExperienceByCountryViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model);

            return CommandResult<ApplicationExperienceByCountryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationExperienceByCountryViewModel>> IsNameExists(ApplicationExperienceByCountryViewModel model)
        {
                        
            return CommandResult<ApplicationExperienceByCountryViewModel>.Instance();
        }

        public async Task<IList<ApplicationExperienceByCountryViewModel>> GetListByApplication(string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                                ct.""Name"" as CountryName	
                                FROM rec.""ApplicationExperienceByCountry"" as c
                                LEFT JOIN cms.""Country"" as ct ON ct.""Id"" = c.""CountryId""
                                WHERE c.""IsDeleted""=false AND c.""ApplicationId""='" + candidateProfileId + "'";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
    }
}