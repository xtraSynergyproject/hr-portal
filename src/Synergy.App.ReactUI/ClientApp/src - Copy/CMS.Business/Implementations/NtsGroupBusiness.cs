using AutoMapper;
using CMS.Business.Interface;
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
    public class NtsGroupBusiness : BusinessBase<NtsGroupViewModel, NtsGroup>, INtsGroupBusiness
    {
        private INtsGroupTemplateBusiness _ntsGroupTemplateBusiness;
        private INtsGroupUserGroupBusiness _ntsGroupUserGroupBusiness;
        public NtsGroupBusiness(IRepositoryBase<NtsGroupViewModel, NtsGroup> repo, IMapper autoMapper, INtsGroupTemplateBusiness ntsGroupTemplateBusiness, INtsGroupUserGroupBusiness ntsGroupUserGroupBusiness) : base(repo, autoMapper)
        {
            _ntsGroupTemplateBusiness = ntsGroupTemplateBusiness;
            _ntsGroupUserGroupBusiness = ntsGroupUserGroupBusiness;
        }

        public async override Task<CommandResult<NtsGroupViewModel>> Create(NtsGroupViewModel model)
        {

            // var data = _autoMapper.Map<NtsGroupViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<NtsGroupViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model);
            if (model.TemplateIds != null && model.TemplateIds.Count() > 0)
            {
                foreach (var id in model.TemplateIds)
                {
                    var temp = new NtsGroupTemplateViewModel();
                    temp.NtsGroupId = result.Item.Id;
                    temp.TemplateId = id;
                    await _ntsGroupTemplateBusiness.Create(temp);

                }
            }
            if (model.UserGroupIds != null && model.UserGroupIds.Count() > 0)
            {
                foreach (var id in model.UserGroupIds)
                {
                    var temp1 = new NtsGroupUserGroupViewModel();
                    temp1.NtsGroupId = result.Item.Id;
                    temp1.UserGroupId = id;
                    await _ntsGroupUserGroupBusiness.Create(temp1);

                }
            }

            return CommandResult<NtsGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsGroupViewModel>> Edit(NtsGroupViewModel model)
        {

            // var data = _autoMapper.Map<NtsGroupViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<NtsGroupViewModel>.Instance(model, false, validateName.Messages);
            }

            var result = await base.Edit(model);
            var pagecol = await _ntsGroupTemplateBusiness.GetList(x => x.NtsGroupId == model.Id);
            var existingIds = pagecol.Select(x => x.TemplateId);
            var newIds = model.TemplateIds;
            var ToDelete = existingIds.Except(newIds).ToList();
            var ToAdd = newIds.Except(existingIds).ToList();
            var pagecol1 = await _ntsGroupUserGroupBusiness.GetList(x => x.NtsGroupId == model.Id);
            var existingIds1 = pagecol1.Select(x => x.UserGroupId);
            var newIds1 = model.UserGroupIds;
            var ToDelete1 = existingIds1.Except(newIds1).ToList();
            var ToAdd1 = newIds1.Except(existingIds1).ToList();
            // Add
            foreach (var id in ToAdd)
            {
                var temp = new NtsGroupTemplateViewModel();
                temp.NtsGroupId = result.Item.Id;
                temp.TemplateId = id;
                await _ntsGroupTemplateBusiness.Create(temp);
            }
            // Delete
            foreach (var id in ToDelete)
            {
                var role = await _ntsGroupTemplateBusiness.GetSingle(x => x.NtsGroupId == model.Id && x.TemplateId == id);
                await _ntsGroupTemplateBusiness.Delete(role.Id);
            }
            foreach (var id in ToAdd1)
            {
                var temp1 = new NtsGroupUserGroupViewModel();
                temp1.NtsGroupId = result.Item.Id;
                temp1.UserGroupId = id;
                await _ntsGroupUserGroupBusiness.Create(temp1);
            }
            // Delete
            foreach (var id in ToDelete1)
            {
                var role1 = await _ntsGroupUserGroupBusiness.GetSingle(x => x.NtsGroupId == model.Id && x.UserGroupId == id);
                await _ntsGroupUserGroupBusiness.Delete(role1.Id);
            }

            return CommandResult<NtsGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<NtsGroupViewModel>> IsNameExists(NtsGroupViewModel model)
        {

            var errorList = new Dictionary<string, string>();

            if (model.Name != null || model.Name != "")
            {
                var name = await _repo.GetSingle(x => x.Name == model.Name && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Name", "Name already exist.");
                }
            }
            if (model.Code != null || model.Code != "")
            {
                var name = await _repo.GetSingle(x => x.Code == model.Code && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Code", "Code already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<NtsGroupViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<NtsGroupViewModel>.Instance();
        }


    }
}
