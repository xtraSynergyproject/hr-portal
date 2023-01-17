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
    public class UserEntityPermissionBusiness : BusinessBase<UserEntityPermissionViewModel, UserEntityPermission>, IUserEntityPermissionBusiness
    {
        private readonly IRepositoryQueryBase<UserEntityPermissionViewModel> _queryRepo;
       

        public UserEntityPermissionBusiness(IRepositoryBase<UserEntityPermissionViewModel, UserEntityPermission> repo, IMapper autoMapper,
            IRepositoryQueryBase<UserEntityPermissionViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
          
        }

        public async override Task<CommandResult<UserEntityPermissionViewModel>> Create(UserEntityPermissionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserEntityPermissionViewModel>(model);


            if (model.LegalEntityId1 != null && model.LegalEntityId1.Count() > 0)
            {
                foreach(var lid in model.LegalEntityId1)
                {
                    var model1 = new UserEntityPermissionViewModel() {
                        UserEntityType = data.UserEntityType,
                        UserEntityId = data.UserEntityId,
                        EntityModelType = EntityModelTypeEnum.LegalEntity,
                        EntityModelId = lid                    
                    };
                    var result = await base.Create(model1);
                }
            }
            if (model.OrganisationId != null && model.OrganisationId.Count() > 0)
            {           

                foreach (var oid in model.OrganisationId)
                {
                    var model1 = new UserEntityPermissionViewModel()
                    {
                        UserEntityType = data.UserEntityType,
                        UserEntityId = data.UserEntityId,
                        EntityModelType = EntityModelTypeEnum.Organization,
                        EntityModelId = oid
                    };
                    var result = await base.Create(model1);
                }
            }
            if (model.TemplateId != null && model.TemplateId.Count() > 0)
            {

                foreach (var tid in model.TemplateId)
                {
                    var model1 = new UserEntityPermissionViewModel()
                    {
                        UserEntityType = data.UserEntityType,
                        UserEntityId = data.UserEntityId,
                        EntityModelType = EntityModelTypeEnum.Template,
                        EntityModelId = tid
                    };
                    var result = await base.Create(model1);
                }
            }
          

            return CommandResult<UserEntityPermissionViewModel>.Instance(model);
        }

        public async override Task<CommandResult<UserEntityPermissionViewModel>> Edit(UserEntityPermissionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserEntityPermissionViewModel>(model);

            // var existingIds = pagecol.Select(x => x.EntityModelId);
            //var newIds = model.LegalEntityId1;
            //var ToDelete = existingIds.Except(newIds).ToList();
            //var ToAdd = newIds.Except(existingIds).ToList();
            if (model.LegalEntityId1 != null && model.LegalEntityId1.Count() > 0)
            {

                var pagecol = await base.GetList(x => x.UserEntityId == model.UserEntityId && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.LegalEntity );
                var existingIds = pagecol.Select(x => x.EntityModelId);
                var newIds = model.LegalEntityId1;
                var ToDelete = existingIds.Except(newIds).ToList();
                var ToAdd = newIds.Except(existingIds).ToList();
                // Add
                foreach (var lid in ToAdd)
                {
                    var model1 = new UserEntityPermissionViewModel()
                    {
                        UserEntityType = data.UserEntityType,
                        UserEntityId = data.UserEntityId,
                        EntityModelType = EntityModelTypeEnum.LegalEntity,
                        EntityModelId = lid,
                        //Id = data.Id   
                    };
                    await base.Create(model1);
                } 
                    // Delete
                    foreach (var lid in ToDelete)
                    {
                        var role = await base.GetSingle(x => x.UserEntityId == model.UserEntityId && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.LegalEntity);
                        await base.Delete(role.Id);
                    }
                //foreach (var lid in existingIds)
                //{

                //    var model1 = new UserEntityPermissionViewModel()
                //    {
                //        UserEntityType = data.UserEntityType,
                //        UserEntityId = data.UserEntityId,
                //        EntityModelType = EntityModelTypeEnum.LegalEntity,
                //        EntityModelId = lid,
                      
                //    };
                //    var result = await base.Edit(model1);
                //}
            }
            if (model.OrganisationId != null && model.OrganisationId.Count() > 0)
            {

                var pagecol = await base.GetList(x => x.UserEntityId == model.UserEntityId && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.Organization);
                var existingIds = pagecol.Select(x => x.EntityModelId);
                var newIds = model.OrganisationId;
                var ToDelete = existingIds.Except(newIds).ToList();
                var ToAdd = newIds.Except(existingIds).ToList();
                // Add
                foreach (var lid in ToAdd)
                {
                    var model1 = new UserEntityPermissionViewModel()
                    {
                        UserEntityType = data.UserEntityType,
                        UserEntityId = data.UserEntityId,
                        EntityModelType = EntityModelTypeEnum.Organization,
                        EntityModelId = lid,
                       // Id = data.Id
                    };
                    await base.Create(model1);
                }
                // Delete
                foreach (var lid in ToDelete)
                {
                    var role = await base.GetSingle(x => x.UserEntityId == model.UserEntityId && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.Organization);
                    await base.Delete(role.Id);
                }
                //foreach (var lid in model.OrganisationId)
                //{

                //    var model1 = new UserEntityPermissionViewModel()
                //    {
                //        UserEntityType = data.UserEntityType,
                //        UserEntityId = data.UserEntityId,
                //        EntityModelType = EntityModelTypeEnum.Organization,
                //        EntityModelId = lid,
                //        Id = data.Id
                //    };
                //    var result = await base.Edit(model1);
                //}
            }
            if (model.TemplateId != null && model.TemplateId.Count() > 0)
            {

                var pagecol = await base.GetList(x => x.UserEntityId == model.UserEntityId && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.Template);
                var existingIds = pagecol.Select(x => x.EntityModelId);
                var newIds = model.TemplateId;
                var ToDelete = existingIds.Except(newIds).ToList();
                var ToAdd = newIds.Except(existingIds).ToList();
                // Add
                foreach (var lid in ToAdd)
                {
                    var model1 = new UserEntityPermissionViewModel()
                    {
                        UserEntityType = data.UserEntityType,
                        UserEntityId = data.UserEntityId,
                        EntityModelType = EntityModelTypeEnum.Template,
                        EntityModelId = lid,
                       // Id = data.Id
                    };
                    await base.Create(model1);
                }
                // Delete
                foreach (var lid in ToDelete)
                {
                    var role = await base.GetSingle(x => x.UserEntityId == model.UserEntityId && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.Template);
                    await base.Delete(role.Id);
                }
                //foreach (var lid in model.TemplateId)
                //{

                //    var model1 = new UserEntityPermissionViewModel()
                //    {
                //        UserEntityType = data.UserEntityType,
                //        UserEntityId = data.UserEntityId,
                //        EntityModelType = EntityModelTypeEnum.Template,
                //        EntityModelId = lid,
                //        Id = data.Id
                //    };
                //    var result = await base.Edit(model1);
                //}
            }

            



            // Add

            return CommandResult<UserEntityPermissionViewModel>.Instance(model);
        }

        //public async Task<List<UserEntityPermissionViewModel>> GetUserPermissionHierarchy(string userId)
        //{
        //    var query = @$"SELECT up.*,h.""Name"" as HierarchyName
        //           FROM public.""UserEntityPermission"" as up 

        //           left join public.""HierarchyMaster"" as h on h.""Id""= up.""HierarchyId""
        //           where up.""UserId"" = '{userId}' and up.""IsDeleted"" = false";
        //    var queryData = await _queryRepo.ExecuteQueryList<UserEntityPermissionViewModel>(query, null);
        //    return queryData;
        //}
        //var existingelist = await base.GetList(x => x.UserEntityId == model.UserEntityId && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.LegalEntity);
        //var existingorglist = await base.GetList(x => x.UserEntityId == model.UserEntityId && x.UserEntityType == model.UserEntityType && x.EntityModelType == EntityModelTypeEnum.Organization);
        //var delist = existingelist.Except(model.LegalEntityId1)

    }
}
