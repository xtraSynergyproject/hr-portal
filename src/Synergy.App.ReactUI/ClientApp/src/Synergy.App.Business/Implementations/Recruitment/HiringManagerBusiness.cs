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
    public class HiringManagerBusiness : BusinessBase<HiringManagerViewModel, HiringManager>, IHiringManagerBusiness
    {
        IUserBusiness _userBusiness;
        private readonly IRepositoryQueryBase<HiringManagerViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryIdNameRepo;
        public HiringManagerBusiness(IRepositoryBase<HiringManagerViewModel, HiringManager> repo, IMapper autoMapper, IUserBusiness userBusiness,
            IRepositoryQueryBase<HiringManagerViewModel> queryRepo, IRepositoryQueryBase<IdNameViewModel> queryIdNameRepo) : base(repo, autoMapper)
        {
            _userBusiness = userBusiness;
            _queryRepo = queryRepo;
            _queryIdNameRepo = queryIdNameRepo;
    }

        public async override Task<CommandResult<HiringManagerViewModel>> Create(HiringManagerViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<HiringManagerViewModel>(model);
            var validateEmail = await IsExists(model);
            if (!validateEmail.IsSuccess)
            {
                return CommandResult<HiringManagerViewModel>.Instance(model, false, validateEmail.Messages);
            }
            else 
            { 
            var useremail = await _userBusiness.GetSingle(x => x.Email == model.Email);
            if(useremail !=null)
            {
                model.UserId = useremail.Id;
            }
                else
                {
                    var random = new Random();
                    var Pass = Convert.ToString(random.Next(10000000, 99999999));
                    var usermodel = new UserViewModel
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = Pass,
                        UserType = UserTypeEnum.HM,
                        ConfirmPassword = Pass,
                        SendWelcomeEmail = true,
                    };
                    var userResult = await _userBusiness.Create(usermodel);
                    model.UserId = userResult.Item.Id;
                }
            }
            var result = await base.Create(model,autoCommit);
            if (model.OrganizationId!=null && model.OrganizationId.Count()>0) 
            {
                foreach (var org in model.OrganizationId)
                {
                    HiringManagerOrganizationViewModel data = new HiringManagerOrganizationViewModel();
                    data.OrganizationId = org;
                    data.HiringManagerId = result.Item.Id;
                    var userResult = await base.Create<HiringManagerOrganizationViewModel, HiringManagerOrganization>(data);
                }               
            }
            return CommandResult<HiringManagerViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<HiringManagerViewModel>> Edit(HiringManagerViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<HiringManagerViewModel>(model);
            var validateEmail = await IsExists(data);
            if (!validateEmail.IsSuccess)
            {
                return CommandResult<HiringManagerViewModel>.Instance(model, false, validateEmail.Messages);
            }

            //var usermodel = await _userBusiness.GetSingleById(data.UserId);
            //usermodel.Name = data.Name;
            //usermodel.Email = data.Email;
            
            //var userResult = await _userBusiness.EditUser(usermodel);
            //if (userResult.IsSuccess)
            //{
                var result = await base.Edit(data,autoCommit);
                var pagecol = await _repo.GetList<HiringManagerOrganizationViewModel,HiringManagerOrganization>(x => x.HiringManagerId == model.Id);
                var existingIds = pagecol.Select(x => x.OrganizationId);
                var newIds = model.OrganizationId;
                var ToDelete = existingIds.Except(newIds).ToList();
                var ToAdd = newIds.Except(existingIds).ToList();
                // Add
                foreach (var id in ToAdd)
                {
                    var Horg = new HiringManagerOrganizationViewModel();
                    Horg.OrganizationId = id;
                    Horg.HiringManagerId = result.Item.Id;
                    await _repo.Create<HiringManagerOrganizationViewModel, HiringManagerOrganization>(Horg);
                }
                // Delete
                foreach (var id in ToDelete)
                {
                    var role = await _repo.GetSingle<HiringManagerOrganizationViewModel, HiringManagerOrganization>(x => x.HiringManagerId == model.Id && x.OrganizationId == id);
                    await _repo.Delete<HiringManagerOrganizationViewModel, HiringManagerOrganization>(role.Id);
                }
                return CommandResult<HiringManagerViewModel>.Instance(data, result.IsSuccess, result.Messages);
            //}
            //else
            //{
            //    return CommandResult<HiringManagerViewModel>.Instance(data, false, validateEmail.Messages);
            //}
        }
        
        private async Task<CommandResult<HiringManagerViewModel>> IsExists(HiringManagerViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.Email == null)
            {
                errorList.Add("Email", "Email is required.");
            }
            else
            {
                var email = await _repo.GetSingle(x => x.Email == model.Email && x.Id != model.Id && x.IsDeleted == false);
                if (email != null)
                {
                    errorList.Add("Email", "Email Id already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<HiringManagerViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<HiringManagerViewModel>.Instance();
        }

        public async Task<IList<HiringManagerViewModel>> GetHiringManagersList()
        {
            string query = @$"SELECT l.*, d.""Name"" as DepartmentName,p.""Name"" as DesignationName,string_agg(o.""Name"", ',') as organizationName
                                FROM rec.""HiringManager"" as l
left JOIN cms.""Job"" as p on p.""Id"" = l.""DesignationId""
                                 LEFT JOIN  rec.""HiringManagerOrganization"" as hmo on hmo.""HiringManagerId""=l.""Id"" and hmo.""IsDeleted"" = false
                                LEFT JOIN cms.""Organization"" as o ON o.""Id"" = hmo.""OrganizationId"" 
                                LEFT JOIN rec.""ListOfValue"" as d on d.""Id"" = l.""DepartmentId""
                                WHERE l.""IsDeleted"" = false  group by l.""Id"",d.""Id"",p.""Id"" order by l.""CreatedDate"" desc";
                        
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
        public async Task<List<HiringManagerViewModel>> GetUserIdList()
        {
            string query = @$"SELECT c.*
                                FROM rec.""HiringManager"" as c
                                LEFT JOIN public.""User"" as u ON u.""Id"" = c.""UserId""
                                WHERE c.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<List<HiringManagerViewModel>> GetHMListByOrgId(string orgId)
        {
            string query = @$"SELECT c.*,p.""Name"" as DesignationName
                                FROM rec.""HiringManager"" as c
                                left JOIN cms.""Job"" as p on p.""Id"" = c.""DesignationId""
                                inner join rec.""HiringManagerOrganization"" as hmo on c.""Id""=hmo.""HiringManagerId""
                                LEFT JOIN public.""User"" as u ON u.""Id"" = c.""UserId""
                                WHERE c.""IsDeleted""=false and hmo.""OrganizationId""='{orgId}'";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            return queryData;
        }
            public async Task<List<HiringManagerViewModel>> GetHODListByOrgId(string orgId)
            {
                string query = @$"SELECT c.*,p.""Name"" as DesignationName
                                FROM rec.""HeadOfDepartment"" as c
                                left JOIN cms.""Job"" as p on p.""Id"" = c.""DesignationId""
                                inner join rec.""HeadOfDepartmentOrganization"" as hmo on c.""Id""=hmo.""HeadOfDepartmentId""
                                LEFT JOIN public.""User"" as u ON u.""Id"" = c.""UserId""
                                WHERE c.""IsDeleted""=false and hmo.""OrganizationId""='{orgId}'";

                var queryData = await _queryRepo.ExecuteQueryList(query, null);
                return queryData;
            }
        public async Task<List<IdNameViewModel>> GetHmOrg(string userId)
        {
            string query = @$"SELECT o.""Id"",o.""Name"" as DesignationName
from rec.""HiringManagerOrganization"" as hmo 
                                join  cms.""Organization"" as o ON o.""Id"" = hmo.""OrganizationId""
                                join rec.""HiringManager"" as c on  c.""Id""=hmo.""HiringManagerId""                                 
                                WHERE c.""IsDeleted""=false and c.""UserId""='{userId}'";

            var queryData = await _queryIdNameRepo.ExecuteQueryList(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetHODOrg(string userId)
        {
            string query = @$"SELECT o.""Id"",o.""Name"" as DesignationName
from rec.""HeadOfDepartmentOrganization"" as hmo 
                                join  cms.""Organization"" as o ON o.""Id"" = hmo.""OrganizationId""
                                join rec.""HeadOfDepartment"" as c on  c.""Id""=hmo.""HeadOfDepartmentId""                                 
                                WHERE c.""IsDeleted""=false and c.""UserId""='{userId}'";

            var queryData = await _queryIdNameRepo.ExecuteQueryList(query, null);
            return queryData;           
        }

    }
}
