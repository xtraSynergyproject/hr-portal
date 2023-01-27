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
    public class UserRoleDataPermissionBusiness : BusinessBase<UserRoleDataPermissionViewModel, UserRoleDataPermission>, IUserRoleDataPermissionBusiness
    {
        private readonly IRepositoryQueryBase<UserDataPermissionViewModel> _apstqueryRepo;

        public UserRoleDataPermissionBusiness(IRepositoryBase<UserRoleDataPermissionViewModel, UserRoleDataPermission> repo, IMapper autoMapper,
            IRepositoryQueryBase<UserDataPermissionViewModel> apstqueryRepo) : base(repo, autoMapper)
        {
            _apstqueryRepo = apstqueryRepo;
        }

        public async Task<IList<IdNameViewModel>> GetColunmMetadataListByPageForUserRole(string pageId)
        {
            string query = @$"select cm.""Id"" as Id, cm.""Name"" as Name from public.""Page"" as pg
                            left join public.""Template"" as pt on pt.""Id"" = pg.""TemplateId"" and pt.""IsDeleted""=false
                            left join public.""TableMetadata"" as tm on tm.""Id"" = pt.""TableMetadataId"" and tm.""IsDeleted""=false
                            left join public.""ColumnMetadata"" as cm on cm.""TableMetadataId"" = tm.""Id"" and cm.""IsDeleted""=false
                            where pg.""Id"" ='{pageId}' and pg.""IsDeleted""=false";

            var list = await _apstqueryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }

        public async Task<IList<UserRoleDataPermissionViewModel>> GetUserRoleDataPermissionByPageId(string pageId,string portalId)
        {
            string query = @$"select udp.*, u.""Name"" as UserRoleName, pa.""Name"" as PageName, cm.""Name"" as ColumnMetadataName,
                                cm2.""Name"" as ColumnMetadataName2
                                from public.""UserRoleDataPermission"" as udp
                                left join public.""UserRole"" as u on u.""Id"" = udp.""UserRoleId"" and u.""IsDeleted""=false
                                left join public.""Page"" as pa on pa.""Id"" = udp.""PageId"" and pa.""IsDeleted""=false
                                left join public.""ColumnMetadata"" as cm on cm.""Id""  = udp.""ColumnMetadataId"" and cm.""IsDeleted""=false
                                left join public.""ColumnMetadata"" as cm2 on cm2.""Id""  = udp.""ColumnMetadataId2"" and cm2.""IsDeleted""=false
                                where udp.""PageId"" ='{pageId}' and udp.""IsDeleted"" = false";

            var list = await _apstqueryRepo.ExecuteQueryList<UserRoleDataPermissionViewModel>(query, null);
            return list;
        }

        public async override Task<CommandResult<UserRoleDataPermissionViewModel>> Create(UserRoleDataPermissionViewModel model)
        {
            var data = _autoMapper.Map<UserRoleDataPermissionViewModel>(model);
            var result = await base.Create(data);

            return CommandResult<UserRoleDataPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserRoleDataPermissionViewModel>> Edit(UserRoleDataPermissionViewModel model)
        {
            var data = _autoMapper.Map<UserRoleDataPermissionViewModel>(model);
            var result = await base.Edit(data);

            return CommandResult<UserRoleDataPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
