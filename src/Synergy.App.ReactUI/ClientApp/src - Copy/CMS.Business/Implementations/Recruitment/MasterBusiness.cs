using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class MasterBusiness : IMasterBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo;

        public MasterBusiness(IMapper autoMapper, IRepositoryQueryBase<IdNameViewModel> queryRepo) 
        {
            _queryRepo = queryRepo;

        }
        public async Task<IList<IdNameViewModel>> GetIdNameList(string Type)
        {
            var query = "";
            var list = new List<IdNameViewModel>();
            try
            {
                //if (Type == "Position")
                //{
                //    query = @$"SELECT ""Id"",""Name"" FROM cms.""Position"" where ""IsDeleted""=false and ""Status""=1 order by ""Name""";
                //}
                if (Type == "Organization")
                {
                    query = @$"SELECT ""Id"",""Name"" FROM cms.""Organization"" where ""IsDeleted""=false and ""Status""=1 order by ""Name""";
                }
                else if (Type == "Country")
                {
                    query = @$"SELECT ""Id"",""Name"", ""Code"" FROM cms.""Country"" where ""IsDeleted""=false and ""Status""=1 order by ""Name""";
                }
                else if (Type == "Job")
                {
                    query = @$"SELECT ""Id"",""Name"" FROM cms.""Job"" where ""IsDeleted""=false and ""Status""=1 order by ""Name""";
                }
                else if (Type == "Position")
                {
                    query = @$"SELECT ""Id"",""Name"" FROM cms.""Position"" where ""IsDeleted""=false and ""Status""=1 order by ""Name""";
                }
                else if (Type == "Location")
                {
                    query = @$"SELECT ""Id"",""Name"" FROM cms.""Location"" where ""IsDeleted""=false and ""Status""=1 order by ""Name""";
                }
                else if (Type == "Nationality")
                {
                    query = @$"SELECT ""Id"",""Name"" FROM cms.""Nationality"" where ""IsDeleted""=false and ""Status""=1 order by ""Name""";
                }
                else if (Type == "Currency")
                {
                    query = @$"SELECT ""Id"",""Name"" FROM cms.""Currency"" where ""IsDeleted""=false and ""Status""=1 order by ""Name""";
                }

                list = await _queryRepo.ExecuteQueryList(query, null);
            }
            catch(Exception)
            {

            }
            return list;
        }

        public async Task<IdNameViewModel> GetJobNameById(string Id)
        {
            var query = "";
            //var list = new List<IdNameViewModel>();
            var name = new IdNameViewModel();
            try
            {
                query = @$"SELECT ""Name"" FROM cms.""Job"" where ""Id""='{Id}' and ""IsDeleted""=false and ""Status""=1";
               // list = await _queryRepo.ExecuteQueryList(query, null);
                name = await _queryRepo.ExecuteQuerySingle(query,null);
            }
            catch (Exception)
            {

            }
            return name;
        }
        public async Task<IdNameViewModel> GetOrgNameById(string Id)
        {
            var query = "";
            //var list = new List<IdNameViewModel>();
            var name = new IdNameViewModel();
            try
            {
                query = @$"SELECT ""Name"" FROM cms.""Organization"" where ""Id""='{Id}' ";
                // list = await _queryRepo.ExecuteQueryList(query, null);
                name = await _queryRepo.ExecuteQuerySingle(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }
        public async Task<IdNameViewModel> GetOrgNameByBatchId(string Id)
        {                       
            var name = new IdNameViewModel();
            try
            {
                var query = @$"SELECT org.""Name"" as Name,org.""Id"" as Id FROM rec.""Batch"" as bt
                                inner join cms.""Organization"" as org on org.""Id""=bt.""OrganizationId""
                                 where bt.""Id""='{Id}'";                
                name = await _queryRepo.ExecuteQuerySingle(query, null);
            }
            catch (Exception)
            {

            }
            return name;
        }

        public async Task<IdNameViewModel> GetNationalityIdByName()
        {
            var query = @$"SELECT ""Id"" FROM cms.""Country"" where ""Code""='India' and ""IsDeleted""=false and ""Status""=1";
            var name = await _queryRepo.ExecuteQuerySingle(query, null);
            return name;
        }
        public async Task<List<IdNameViewModel>> GetOrgByJobAddId(string JobId)
        {
            var query = "";

            var list = new List<IdNameViewModel>();
            try
            {
                query = @$"SELECT org.""Name"" ,org.""Id""
FROM cms.""Job"" as ja
inner join rec.""ManpowerRecruitmentSummary"" as mrc on mrc.""JobId""=ja.""Id""
inner join cms.""Organization"" as org on org.""Id""=mrc.""OrganizationId""
where ja.""Id""='{JobId}' and ja.""IsDeleted""=false and ja.""Status""=1";
              
                 list = await _queryRepo.ExecuteQueryList(query, null);
            }
            catch (Exception e)
            {

            }
            return list;
        }

    }
}