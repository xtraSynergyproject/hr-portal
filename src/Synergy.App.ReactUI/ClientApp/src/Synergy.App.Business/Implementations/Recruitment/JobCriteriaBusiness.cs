using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class JobCriteriaBusiness : BusinessBase<JobCriteriaViewModel, JobCriteria>, IJobCriteriaBusiness
    {
        private readonly IRepositoryQueryBase<JobCriteriaViewModel> _queryRepo;
        private IListOfValueBusiness _lovBusiness;
        public JobCriteriaBusiness(IRepositoryBase<JobCriteriaViewModel, JobCriteria> repo, IMapper autoMapper, IRepositoryQueryBase<JobCriteriaViewModel> queryRepo, IListOfValueBusiness lovBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _lovBusiness = lovBusiness;
        }

        public async override Task<CommandResult<JobCriteriaViewModel>> Create(JobCriteriaViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<JobCriteriaViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobCriteriaViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model,autoCommit);

            return CommandResult<JobCriteriaViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<JobCriteriaViewModel>> Edit(JobCriteriaViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<JobCriteriaViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobCriteriaViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model,autoCommit);

            return CommandResult<JobCriteriaViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<JobCriteriaViewModel>> IsNameExists(JobCriteriaViewModel model)
        {
                        
            return CommandResult<JobCriteriaViewModel>.Instance();
        }
        public async Task<IList<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByJobAndType(string JobAdvertisementId,string type)
        {

            string query1 = @$"SELECT j.""Id"" as Id,j.""Criteria"" as Criteria,j.""CriteriaType"" as CriteriaType,g.""Code"" as CriteriaTypeCode,j.""Type"" as Type,j.""Weightage"" as Weightage,l.""Code"" as ListOfValueType              
                            ,j.""ListOfValueTypeId"" as ListOfValueTypeId,l.""Description"" as Description, l.""EnableDescription"" as EnableDescription
                            FROM rec.""JobCriteria"" as j
                            LEFT JOIN rec.""ListOfValue"" as g ON g.""Id"" = j.""CriteriaType""
                            LEFT JOIN rec.""ListOfValue"" as l ON l.""Id"" = j.""ListOfValueTypeId""
                            where j.""JobAdvertisementId""='{JobAdvertisementId}' and j.""Type""='{type}' and j.""IsDeleted"" = false
                            ";
            var list = await _queryRepo.ExecuteQueryList<ApplicationJobCriteriaViewModel>(query1, null);
            if(type== "OtherInformation")
            {
                var criterialist = new List<ApplicationJobCriteriaViewModel>();
                var i = 1;
                foreach(var item in list)
                {
                    item.SequenceOrder = i;
                    if(item.CriteriaTypeCode == "LISTOFVALUE")
                    {
                        if (item.ListOfValueTypeId != null)
                        {
                            var lov = await _lovBusiness.GetList(x => x.ParentId == item.ListOfValueTypeId);
                            var temp = lov.Select(x => x.Id);
                            var child = await _lovBusiness.GetList(x => temp.Contains(x.ParentId));
                            if (child.Count > 0)
                            {
                                var criteria = new ApplicationJobCriteriaViewModel
                                {
                                    CriteriaTypeCode = "LISTOFVALUE",
                                    Type= "OtherInformation",
                                    SequenceOrder = ++i,
                                };
                                criterialist.Add(criteria);
                            }
                        }
                    }
                    i++;
                }
                if(criterialist.Count>0)
                list.AddRange(criterialist);
            }
            return list.OrderBy(x=>x.SequenceOrder).ToList();
        }
        public async Task<IList<ApplicationJobCriteriaViewModel>> GetApplicationJobCriteriaByApplicationIdAndType(string ApplicationId, string type)
        {

            string query1 = @$"SELECT a.*,g.""Code"" as CriteriaTypeCode            
                            FROM rec.""ApplicationJobCriteria"" as a
                            LEFT JOIN rec.""ListOfValue"" as g ON g.""Id"" = a.""CriteriaType""
                            where a.""ApplicationId""='{ApplicationId}' and a.""Type""='{type}'
                            ";
            var list = await _queryRepo.ExecuteQueryList<ApplicationJobCriteriaViewModel>(query1, null);
            return list;
        }
    }
}
