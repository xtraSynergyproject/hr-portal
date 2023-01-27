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
    public class PermissionBusiness : BusinessBase<PermissionViewModel, Permission>, IPermissionBusiness
    {
        public PermissionBusiness(IRepositoryBase<PermissionViewModel, Permission> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<PermissionViewModel>> Create(PermissionViewModel model)
        {
            var data = _autoMapper.Map<PermissionViewModel>(model);
            data.permissiontype = model.permissiontype;
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<PermissionViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data);

            return CommandResult<PermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PermissionViewModel>> Edit(PermissionViewModel model)
        {
            var data = _autoMapper.Map<PermissionViewModel>(model);
            data.permissiontype = model.permissiontype;
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<PermissionViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(data);

            return CommandResult<PermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<PermissionViewModel>> IsNameExists(PermissionViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "Name is required.");
            }
            else
            {
                var name = await _repo.GetSingle(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Name", "Name already exist.");
                }
            }
            if (model.Code.IsNullOrEmpty())
            {
                errorList.Add("Code", "Code is required.");
            }
            else { 
                var name = await _repo.GetSingle(x => x.Code.ToLower() == model.Code.ToLower() && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Code", "Code already exist.");
                }
            }
            if (model.permissiontype != "Permission")
            {
                if (model.GroupName.IsNullOrEmpty())
                {
                    errorList.Add("GroupName", "Group Name is required.");
                }
                if (!model.PageTypes.IsNotNull())
                {
                    errorList.Add("PageTypes", "Page type is required.");
                }
                if (!model.UserPermissionTypes.IsNotNull())
                {
                    errorList.Add("UserPermissionTypes", "User permission type is required.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<PermissionViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<PermissionViewModel>.Instance();
        }
    }
}
