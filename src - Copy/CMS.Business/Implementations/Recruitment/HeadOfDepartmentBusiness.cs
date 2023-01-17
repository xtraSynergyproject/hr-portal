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
    public class HeadOfDepartmentBusiness : BusinessBase<HeadOfDepartmentViewModel, HeadOfDepartment>, IHeadOfDepartmentBusiness
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IRepositoryQueryBase<HeadOfDepartmentViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryIdNameRepo;
        public HeadOfDepartmentBusiness(IRepositoryBase<HeadOfDepartmentViewModel, HeadOfDepartment> repo, IMapper autoMapper, IUserBusiness userBusiness,
            IRepositoryQueryBase<HeadOfDepartmentViewModel> queryRepo, IRepositoryQueryBase<IdNameViewModel> queryIdNameRepo) : base(repo, autoMapper)
        {            
            _queryRepo = queryRepo;
            _userBusiness = userBusiness;
            _queryIdNameRepo = queryIdNameRepo;
        }

        public async override Task<CommandResult<HeadOfDepartmentViewModel>> Create(HeadOfDepartmentViewModel model)
        {
            //var data = _autoMapper.Map<HiringManagerViewModel>(model);
            var validateEmail = await IsExists(model);
            if (!validateEmail.IsSuccess)
            {
                return CommandResult<HeadOfDepartmentViewModel>.Instance(model, false, validateEmail.Messages);
            }
            else
            {
                var useremail = await _userBusiness.GetSingle(x => x.Email == model.Email);
                if (useremail != null)
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
                        UserType = UserTypeEnum.ORG_UNIT,
                        Password = Pass,
                        ConfirmPassword = Pass,
                        SendWelcomeEmail = true,
                    };
                    var userResult = await _userBusiness.Create(usermodel);
                    model.UserId = userResult.Item.Id;
                }
            }
            var result = await base.Create(model);

            if (model.OrganizationId != null && model.OrganizationId.Count() > 0)
            {
                foreach (var org in model.OrganizationId)
                {
                    HeadOfDepartmentOrganizationViewModel data = new HeadOfDepartmentOrganizationViewModel();
                    data.OrganizationId = org;
                    data.HeadOfDepartmentId = result.Item.Id;
                    var userResult = await base.Create<HeadOfDepartmentOrganizationViewModel, HeadOfDepartmentOrganization>(data);
                }
            }

            return CommandResult<HeadOfDepartmentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<HeadOfDepartmentViewModel>> Edit(HeadOfDepartmentViewModel model)
        {
            var data = _autoMapper.Map<HeadOfDepartmentViewModel>(model);
            var validateEmail = await IsExists(data);
            if (!validateEmail.IsSuccess)
            {
                return CommandResult<HeadOfDepartmentViewModel>.Instance(model, false, validateEmail.Messages);
            }

            //var usermodel = await _userBusiness.GetSingleById(data.UserId);
            //usermodel.Name = data.Name;
            //usermodel.Email = data.Email;

            //var result = await _userBusiness.EditUser(usermodel);
            //if (result.IsSuccess)
            //{
                var hodresult = await base.Edit(data);
                var pagecol = await _repo.GetList<HeadOfDepartmentOrganizationViewModel, HeadOfDepartmentOrganization>(x => x.HeadOfDepartmentId == model.Id);
                var existingIds = pagecol.Select(x => x.OrganizationId);
                var newIds = model.OrganizationId;
                var ToDelete = existingIds.Except(newIds).ToList();
                var ToAdd = newIds.Except(existingIds).ToList();
                // Add
                foreach (var id in ToAdd)
                {
                    var Horg = new HeadOfDepartmentOrganizationViewModel();
                    Horg.OrganizationId = id;
                    Horg.HeadOfDepartmentId = hodresult.Item.Id;
                    await _repo.Create<HeadOfDepartmentOrganizationViewModel, HeadOfDepartmentOrganization>(Horg);
                }
                // Delete
                foreach (var id in ToDelete)
                {
                    var role = await _repo.GetSingle<HeadOfDepartmentOrganizationViewModel, HeadOfDepartmentOrganization>(x => x.HeadOfDepartmentId == model.Id && x.OrganizationId == id);
                    await _repo.Delete<HeadOfDepartmentOrganizationViewModel, HeadOfDepartmentOrganization>(role.Id);
                }

                return CommandResult<HeadOfDepartmentViewModel>.Instance(data, hodresult.IsSuccess, hodresult.Messages);
           // }
            
           // return CommandResult<HeadOfDepartmentViewModel>.Instance(data, result.IsSuccess, result.Messages);           

        }
        
        private async Task<CommandResult<HeadOfDepartmentViewModel>> IsExists(HeadOfDepartmentViewModel model)
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
                return CommandResult<HeadOfDepartmentViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<HeadOfDepartmentViewModel>.Instance();
        }

        public async Task<IList<HeadOfDepartmentViewModel>> GetHodList()
        {
            string query = @$"SELECT h.*, de.""Name"" as DepartmentName, d.""Name"" as DesignationName, string_agg(o.""Name"", ',') as organizationName
                                FROM rec.""HeadOfDepartment"" as h
                                left JOIN cms.""Job"" as d on d.""Id"" = h.""DesignationId""                               
                                LEFT JOIN rec.""ListOfValue"" as de on de.""Id"" = h.""DepartmentId""
                                LEFT JOIN rec.""HeadOfDepartmentOrganization"" as hmo on hmo.""HeadOfDepartmentId""=h.""Id"" and hmo.""IsDeleted"" = false
                                LEFT JOIN cms.""Organization"" as o ON o.""Id"" = hmo.""OrganizationId""
                                WHERE h.""IsDeleted"" = false group by h.""Id"", de.""Id"", d.""Id"" order by h.""CreatedDate"" desc";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }


    }
}
