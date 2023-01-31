using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class PortalBusiness : BusinessBase<PortalViewModel, Portal>, IPortalBusiness
    {
        private readonly IRepositoryQueryBase<PortalViewModel> _queryRepo;
        private readonly IPageBusiness _pageBusiness;
        private readonly IMenuGroupBusiness _menuGroupBusiness;
        private readonly ISubModuleBusiness _subModuleBusiness;
        private readonly IModuleBusiness _moduleBusiness;
        private readonly IColumnMetadataBusiness _columnMetadataBusiness;
        private readonly ITemplateCategoryBusiness _categoryBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        public PortalBusiness(IRepositoryBase<PortalViewModel, Portal> repo, IMapper autoMapper,
            IRepositoryQueryBase<PortalViewModel> queryRepo, ITemplateCategoryBusiness categoryBusiness, ITemplateBusiness templateBusiness,
            IPageBusiness pageBusiness, IMenuGroupBusiness menuGroupBusiness, ISubModuleBusiness subModuleBusiness, IModuleBusiness moduleBusiness,
            IColumnMetadataBusiness columnMetadataBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _pageBusiness = pageBusiness;
            _menuGroupBusiness = menuGroupBusiness;
            _subModuleBusiness = subModuleBusiness;
            _moduleBusiness = moduleBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
            _categoryBusiness = categoryBusiness;
            _templateBusiness = templateBusiness;
        }

        public async override Task<CommandResult<PortalViewModel>> Create(PortalViewModel model)
        {
            if (model.Name.IsNullOrEmpty())
            {
                return CommandResult<PortalViewModel>.Instance(model, x => x.Name, "Portal name is required.");
            }
            else
            {
                if (model.Name.ToLower() == "cms")
                {
                    return CommandResult<PortalViewModel>.Instance(model, x => x.Name, "Portal name should not be cms.");
                }

                var regexName = new Regex("^[a-zA-Z0-9]*$");
                if (!regexName.IsMatch(model.Name.ToString()))
                {
                    return CommandResult<PortalViewModel>.Instance(model, x => x.Name, "Special character and space not allowed in name.");
                }

                //string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                ////string invalid = new string(Path.GetInvalidPathChars());
                ////string invalid = new string(Path.GetInvalidFileNameChars());
                //foreach (char c in invalid)
                //{
                //    var cccc = c;
                //    if (model.Name.ToLower().Contains(c.ToString()))
                //    {
                //        return CommandResult<PortalViewModel>.Instance(model, x => x.Name, "Invalid character in name.");
                //    }
                //}

                var existingPortal = await _repo.GetSingle(x => x.Name == model.Name);
                if (existingPortal != null)
                {
                    return CommandResult<PortalViewModel>.Instance(model, x => x.Name, "Portal name already exist");
                }
            }
            if (!model.DomainName.IsNullOrEmpty())
            {
                //var regexDomain = new Regex("^https?://[a-zA-Z0-9-.]+.[a-zA-Z]{2,}$");
                //var regexDomain = new Regex("^((http|https)://)(www.)?[a-zA-Z0-9-.]+.[a-zA-Z]{2,}+(:[0-9])?$");
                //var flg = regexDomain.IsMatch(model.DomainName.ToString());
                //if (!regexDomain.IsMatch(model.DomainName.ToString()))
                //{
                //    return CommandResult<PortalViewModel>.Instance(model, x => x.DomainName, "Invalid domain name.");
                //}
                var portaldata = await _repo.GetSingle(x => x.DomainName == model.DomainName);
                if (portaldata != null)
                {
                    return CommandResult<PortalViewModel>.Instance(model, x => x.DomainName, "Domain name already exist");
                }
            }
            if (model.BannerHeight < 65 || model.BannerHeight > 200)
            {
                return CommandResult<PortalViewModel>.Instance(model, x => x.BannerHeight, "Banner height must be between 65 and 200");
            }
            var data = _autoMapper.Map<PortalViewModel>(model);
            var result = await base.Create(data);

            return CommandResult<PortalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PortalViewModel>> Edit(PortalViewModel model)
        {
            if (model.Name.IsNullOrEmpty())
            {
                return CommandResult<PortalViewModel>.Instance(model, x => x.Name, "Portal name is required.");
            }
            else
            {
                if (model.Name.ToLower() == "cms")
                {
                    return CommandResult<PortalViewModel>.Instance(model, x => x.Name, "The portal name should not be cms.");
                }

                var regexName = new Regex("^[a-zA-Z0-9]*$");
                if (!regexName.IsMatch(model.Name.ToString()))
                {
                    return CommandResult<PortalViewModel>.Instance(model, x => x.Name, "Special character and space not allowed in name.");
                }

                var portaldata = await _repo.GetSingle(x => x.Id != model.Id && x.Name == model.Name);
                if (portaldata != null)
                {
                    return CommandResult<PortalViewModel>.Instance(model, x => x.Name, "Portal Name already exist");
                }
            }

            if (!model.DomainName.IsNullOrEmpty())
            {
                //var regexDomain = new Regex("^https?://[a-zA-Z0-9-.]+.[a-zA-Z]{2,}$");
                //var regexDomain = new Regex("^((http|https)://)(www.)?[a-zA-Z0-9-.]+.[a-zA-Z]{2,}+(:[0-9])?$");
                //if (!regexDomain.IsMatch(model.DomainName.ToString()))
                //{
                //    return CommandResult<PortalViewModel>.Instance(model, x => x.DomainName, "Invalid domain name.");
                //}
                var portaldata = await _repo.GetSingle(x => x.Id != model.Id && x.DomainName == model.DomainName);
                if (portaldata != null && portaldata.Id.ToLower() != model.Id.ToLower() && portaldata.DomainName.ToLower() == model.DomainName.ToLower())
                {
                    return CommandResult<PortalViewModel>.Instance(model, x => x.DomainName, "Domain name already exist");
                }
            }
            if (model.BannerHeight < 65 || model.BannerHeight > 200)
            {
                return CommandResult<PortalViewModel>.Instance(model, x => x.BannerHeight, "Banner height must be between 65 and 200");
            }
            var data = _autoMapper.Map<PortalViewModel>(model);
            var result = await base.Edit(data);

            return CommandResult<PortalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<MenuViewModel>> GetMenuItems(Portal portal, string userId, string companyId)
        {
            var language = Helper.GetLanguage(_repo.UserContext.CultureName);
            var uId = _repo.UserContext.UserId.IsNullOrEmpty() ? userId : _repo.UserContext.UserId;
            var cId = _repo.UserContext.CompanyId.IsNullOrEmpty() ? companyId : _repo.UserContext.CompanyId;
            var query = @$"select pa.*,upe.""Id"" as UserPermissionId
                ,coalesce(""rlTitle"".""{language}"",pa.""Title"") as ""Title""
                ,coalesce(""rlMenuName"".""{language}"",pa.""MenuName"") as ""MenuName""
                ,coalesce(""rlMenuGroupName"".""{language}"",mg.""Name"") as ""MenuGroupName""
                ,up.""Id"" as UserPortalId,upe.""Permissions"" as Permissions
                ,mg.""MenuGroupIconFileId"" as ""MenuGroupIconFileId"",pa.""IconFileId"" as ""IconFileId""
                ,mg.""MenuGroupDetails"" as ""MenuGroupDetails"",pa.""PageDetails"" as ""PageDetails""
                ,pa.""MenuGroupId"" as MenuGroupId,mg.""ParentId"" as ""ParentId"",
                mg.""SequenceOrder"" as MenuGroupSequenceOrder,mg.""IconCss"" as MenuGroupIconCss,mg.""ShortName"" as ""MenuGroupShortName""
                from public.""Page"" as pa 
                join public.""Portal"" as po on pa.""PortalId""=po.""Id""
                left join public.""ResourceLanguage"" as ""rlTitle"" on pa.""Id""=""rlTitle"".""TemplateId"" 
                and ""rlTitle"".""TemplateType""=13 and ""rlTitle"".""Code""='PortalPageTitle' and ""rlTitle"".""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuName"" on pa.""Id""=""rlMenuName"".""TemplateId"" 
                and ""rlMenuName"".""TemplateType""=13 and ""rlMenuName"".""Code""='PortalPageMenuName' and ""rlMenuName"".""IsDeleted""=false
                
                left join public.""UserPortal"" as up on po.""Id""=up.""PortalId""
                and up.""IsDeleted""=false and up.""UserId""='{uId}' 
                left join public.""MenuGroup"" as mg on pa.""MenuGroupId""=mg.""Id"" and mg.""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuGroupName"" on mg.""Id""=""rlMenuGroupName"".""TemplateId"" 
                and ""rlMenuGroupName"".""TemplateType""=14 and ""rlMenuGroupName"".""Code""='MenuGroupName' and ""rlMenuGroupName"".""IsDeleted""=false
                left join public.""UserPermission"" as upe on pa.""Id""=upe.""PageId"" 
                and upe.""IsDeleted""=false and upe.""UserId""='{uId}'
                where  pa.""PortalId""='{portal.Id}' and pa.""CompanyId""='{cId}' 
                and pa.""IsDeleted""=false and po.""IsDeleted""=false
                union
                select pa.*,upe.""Id"" as UserPermissionId
                ,coalesce(""rlTitle"".""{language}"",pa.""Title"") as ""Title""
                ,coalesce(""rlMenuName"".""{language}"",pa.""MenuName"") as ""MenuName""
                ,coalesce(""rlMenuGroupName"".""{language}"",mg.""Name"") as ""MenuGroupName""
                ,up.""Id"" as UserPortalId,upe.""Permissions"" as Permissions
                ,mg.""MenuGroupIconFileId"" as ""MenuGroupIconFileId"",pa.""IconFileId"" as ""IconFileId""
                ,mg.""MenuGroupDetails"" as ""MenuGroupDetails"",pa.""PageDetails"" as ""PageDetails""
                ,pa.""MenuGroupId"" as MenuGroupId,mg.""ParentId"" as ""ParentId"",
                mg.""SequenceOrder"" as MenuGroupSequenceOrder,mg.""IconCss"" as MenuGroupIconCss,mg.""ShortName"" as ""MenuGroupShortName""
                from public.""Page"" as pa 
                join public.""Portal"" as po on pa.""PortalId""=po.""Id""
                left join public.""ResourceLanguage"" as ""rlTitle"" on pa.""Id""=""rlTitle"".""TemplateId"" 
                and ""rlTitle"".""TemplateType""=13 and ""rlTitle"".""Code""='PortalPageTitle'  and ""rlTitle"".""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuName"" on pa.""Id""=""rlMenuName"".""TemplateId"" 
                and ""rlMenuName"".""TemplateType""=13 and ""rlMenuName"".""Code""='PortalPageMenuName'  and ""rlMenuName"".""IsDeleted""=false
                
                left join 
                ( select up.* from 
                public.""UserRolePortal"" as up 
                join public.""UserRole"" as ur on up.""UserRoleId""=ur.""Id""
                and ur.""IsDeleted""=false 
                join public.""UserRoleUser"" as uru on ur.""Id""=uru.""UserRoleId""
                and uru.""IsDeleted""=false and uru.""UserId""='{uId}' 
                where up.""PortalId""='{portal.Id}' and up.""IsDeleted""=false 
                ) as up on po.""Id""=up.""PortalId""
                left join public.""MenuGroup"" as mg on pa.""MenuGroupId""=mg.""Id"" and mg.""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuGroupName"" on mg.""Id""=""rlMenuGroupName"".""TemplateId"" 
                and ""rlMenuGroupName"".""TemplateType""=14 and ""rlMenuGroupName"".""Code""='MenuGroupName'  and ""rlMenuGroupName"".""IsDeleted""=false
                left join 
                ( select upe.*
                from public.""UserRolePermission"" as upe 
                join public.""UserRole"" as ur2 on upe.""UserRoleId""=ur2.""Id""
                and ur2.""IsDeleted""=false 
                join public.""UserRoleUser"" as uru2 on ur2.""Id""=uru2.""UserRoleId""
                and uru2.""IsDeleted""=false and uru2.""UserId""='{uId}' 
                where  upe.""IsDeleted""=false 
                )upe on pa.""Id""=upe.""PageId""
                where  pa.""PortalId""='{portal.Id}' and pa.""CompanyId""='{cId}' 
                and pa.""IsDeleted""=false and po.""IsDeleted""=false";
            if (_repo.UserContext.IsSystemAdmin)
            {
                query = @$"select pa.*,null as UserPermissionId,true as ""IsSystemAdmin""
                ,coalesce(""rlTitle"".""{language}"",pa.""Title"") as ""Title""
                ,coalesce(""rlMenuName"".""{language}"",pa.""MenuName"") as ""MenuName""
                ,coalesce(""rlMenuGroupName"".""{language}"",mg.""Name"") as ""MenuGroupName""
                ,po.""Id"" as UserPortalId,null as Permissions
                ,mg.""MenuGroupIconFileId"" as ""MenuGroupIconFileId"",pa.""IconFileId"" as ""IconFileId""
                ,mg.""MenuGroupDetails"" as ""MenuGroupDetails"",pa.""PageDetails"" as ""PageDetails""
                ,pa.""MenuGroupId"" as MenuGroupId,mg.""ParentId"" as ""ParentId"",
                mg.""SequenceOrder"" as MenuGroupSequenceOrder,mg.""IconCss"" as MenuGroupIconCss,mg.""ShortName"" as ""MenuGroupShortName""
                from public.""Page"" as pa 
                join public.""Portal"" as po on pa.""PortalId""=po.""Id""
                left join public.""ResourceLanguage"" as ""rlTitle"" on pa.""Id""=""rlTitle"".""TemplateId"" 
                and ""rlTitle"".""TemplateType""=13 and ""rlTitle"".""Code""='PortalPageTitle' and ""rlTitle"".""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuName"" on pa.""Id""=""rlMenuName"".""TemplateId"" 
                and ""rlMenuName"".""TemplateType""=13 and ""rlMenuName"".""Code""='PortalPageMenuName' and ""rlMenuName"".""IsDeleted""=false
                left join public.""MenuGroup"" as mg on pa.""MenuGroupId""=mg.""Id"" and mg.""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuGroupName"" on mg.""Id""=""rlMenuGroupName"".""TemplateId"" 
                and ""rlMenuGroupName"".""TemplateType""=14 and ""rlMenuGroupName"".""Code""='MenuGroupName' and ""rlMenuGroupName"".""IsDeleted""=false
                --left join  public.""UserPermission"" as upe on pa.""Id""=upe.""PageId"" 
                --and upe.""IsDeleted""=false and upe.""UserId""='{uId}'
                where  pa.""PortalId""='{portal.Id}' and pa.""CompanyId""='{cId}' 
                and pa.""IsDeleted""=false and po.""IsDeleted""=false";
            }

            var pages = await _queryRepo.ExecuteQueryList<PageViewModel>(query, null);



            pages = pages.Where(x => x.IsAuthorized).ToList();
            var menuGroups = pages.Where(x => x.MenuGroupName != null).GroupBy(x => x.MenuGroupId).Select(x => x.First()).ToList();
            var menGroupSlides = new List<MenuGroupDetailsViewModel>();
            if (menuGroups != null && menuGroups.Any())
            {
                var ids = string.Join(',', menuGroups.Select(x => $"'{x.MenuGroupId}'"));
                menGroupSlides = await _queryRepo.ExecuteQueryList<MenuGroupDetailsViewModel>(
              $@"select * from public.""MenuGroupDetails""  where ""IsDeleted""=false and ""MenuGroupId"" in ({ids})", null);
            }


            var menus = menuGroups.OrderBy(x => x.MenuGroupSequenceOrder).Select(
                x => new MenuViewModel
                {
                    Id = x.MenuGroupId,
                    IsPageRedirect = x.IsPageRedirect,
                    Name = x.MenuGroupName,
                    MenuGroupShortName = x.MenuGroupShortName,
                    DisplayName = x.MenuGroupName,
                    ParentId = x.ParentId.Coalesce(portal.Id),
                    PortalId = portal.Id,
                    HasChild = true,
                    IconCss = x.MenuGroupIconCss.Coalesce("fa fa-user-circle"),
                    IconFileId = x.MenuGroupIconFileId,
                    MenuDetails = x.MenuGroupDetails,
                    PageType = TemplateTypeEnum.MenuGroup,
                    MenuGroupSequenceOrder = x.MenuGroupSequenceOrder,
                    MenuDetailSlides = menGroupSlides.Where(y => y.MenuGroupId == x.MenuGroupId).OrderBy(z => z.SequenceOrder)
                    .Select(a => new PageDetailsViewModel { SequenceOrder = a.SequenceOrder, Details = a.Details }).ToList()
                }
                ).ToList();
            var menuPages = pages.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            var menuPageSlides = new List<PageDetailsViewModel>();
            if (menuPages != null && menuPages.Any())
            {
                var ids = string.Join(',', menuPages.Select(x => $"'{x.Id}'"));
                menuPageSlides = await _queryRepo.ExecuteQueryList<PageDetailsViewModel>(
              $@"select * from public.""PageDetails""  where  ""IsDeleted""=false and ""PageId"" in ({ids})", null);
            }


            menus.AddRange(menuPages.OrderBy(x => x.SequenceOrder).Select(x =>
              new MenuViewModel
              {
                  Id = x.Id,
                  IsPageRedirect = x.IsPageRedirect,
                  Name = x.Name,
                  MenuGroupShortName = x.MenuGroupShortName,
                  DisplayName = x.MenuName.Coalesce(x.Title).Coalesce(x.Name),
                  ParentId = x.MenuGroupId,
                  IconColor = x.IconColor.Coalesce("#fff"),
                  IconCss = x.IconCss.Coalesce("fa fa-user-circle"),
                  IsRoot = x.IsRootPage,
                  Url = string.Concat("/Portal/", portal.Name, "/", x.Name),
                  IsHidden = x.HideInMenu,
                  PortalId = x.PortalId,
                  HasChild = false,
                  PageType = x.PageType,
                  HideInMenu = x.HideInMenu,
                  ShowMenuWhenAuthorized = x.ShowMenuWhenAuthorized,
                  ShowMenuWhenNotAuthorized = x.ShowMenuWhenNotAuthorized,
                  AuthorizationNotRequired = x.AuthorizationNotRequired,
                  ShowOutsideMenuGroup = x.ShowOutsideMenuGroup,
                  Layout = x.Layout,
                  IconFileId = x.IconFileId,
                  MenuDetails = x.PageDetails,
                  MenuGroupSequenceOrder = x.MenuGroupSequenceOrder,
                  SequenceOrder = x.SequenceOrder,
                  MenuDetailSlides = menuPageSlides.Where(y => y.PageId == x.Id).OrderBy(z => z.SequenceOrder).ToList()
              }
                ));
            //var k = menus.Where(x => x.MenuDetailSlides.Any()).ToList();
            return menus;
        }
        public async Task<List<MenuViewModel>> GetPortalDiagramData(PortalViewModel portal, string userId, string companyId)
        {
            var pages = await _queryRepo.ExecuteQueryList<PageViewModel>(
            @$"select pa.*,upe.""Id"" as UserPermissionId
                ,up.""Id"" as UserPortalId,upe.""Permissions"" as Permissions
                ,mg.""MenuGroupIconFileId"" as ""MenuGroupIconFileId"",pa.""IconFileId"" as ""IconFileId""
                ,mg.""MenuGroupDetails"" as ""MenuGroupDetails"",mg.""Name"" as ""MenuGroupName"",pa.""PageDetails"" as ""PageDetails""
                ,pa.""MenuGroupId"" as MenuGroupId,mg.""ParentId"" as ""ParentId"",
                mg.""SequenceOrder"" as MenuGroupSequenceOrder,mg.""IconCss"" as MenuGroupIconCss
                from public.""Page"" as pa 
                join public.""Portal"" as po on pa.""PortalId""=po.""Id""
                left join public.""ResourceLanguage"" as ""rlTitle"" on pa.""Id""=""rlTitle"".""TemplateId"" 
                and ""rlTitle"".""TemplateType""=13 and ""rlTitle"".""Code""='PortalPageTitle' and ""rlTitle"".""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuName"" on pa.""Id""=""rlMenuName"".""TemplateId"" 
                and ""rlMenuName"".""TemplateType""=13 and ""rlMenuName"".""Code""='PortalPageMenuName' and ""rlMenuName"".""IsDeleted""=false
                
                left join public.""UserPortal"" as up on po.""Id""=up.""PortalId""
                and up.""IsDeleted""=false 
                left join public.""MenuGroup"" as mg on pa.""MenuGroupId""=mg.""Id"" and mg.""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuGroupName"" on mg.""Id""=""rlMenuGroupName"".""TemplateId"" 
                and ""rlMenuGroupName"".""TemplateType""=14 and ""rlMenuGroupName"".""Code""='MenuGroupName' and ""rlMenuGroupName"".""IsDeleted""=false
                left join public.""UserPermission"" as upe on pa.""Id""=upe.""PageId"" 
                and upe.""IsDeleted""=false 
                where  pa.""PortalId""='{portal.Id}'
                and pa.""IsDeleted""=false and po.""IsDeleted""=false
                union
                select pa.*,upe.""Id"" as UserPermissionId
               
                ,up.""Id"" as UserPortalId,upe.""Permissions"" as Permissions
                ,mg.""MenuGroupIconFileId"" as ""MenuGroupIconFileId"",pa.""IconFileId"" as ""IconFileId""
                ,mg.""MenuGroupDetails"" as ""MenuGroupDetails"",mg.""Name"" as ""MenuGroupName"",pa.""PageDetails"" as ""PageDetails""
                ,pa.""MenuGroupId"" as MenuGroupId,mg.""ParentId"" as ""ParentId"",
                mg.""SequenceOrder"" as MenuGroupSequenceOrder,mg.""IconCss"" as MenuGroupIconCss
                from public.""Page"" as pa 
                join public.""Portal"" as po on pa.""PortalId""=po.""Id""
                left join public.""ResourceLanguage"" as ""rlTitle"" on pa.""Id""=""rlTitle"".""TemplateId"" 
                and ""rlTitle"".""TemplateType""=13 and ""rlTitle"".""Code""='PortalPageTitle'  and ""rlTitle"".""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuName"" on pa.""Id""=""rlMenuName"".""TemplateId"" 
                and ""rlMenuName"".""TemplateType""=13 and ""rlMenuName"".""Code""='PortalPageMenuName'  and ""rlMenuName"".""IsDeleted""=false
                
                left join 
                ( select up.* from 
                public.""UserRolePortal"" as up 
                join public.""UserRole"" as ur on up.""UserRoleId""=ur.""Id""
                and ur.""IsDeleted""=false 
                join public.""UserRoleUser"" as uru on ur.""Id""=uru.""UserRoleId""
                and uru.""IsDeleted""=false 
                where up.""PortalId""='{portal.Id}' and up.""IsDeleted""=false 
                ) as up on po.""Id""=up.""PortalId""
                left join public.""MenuGroup"" as mg on pa.""MenuGroupId""=mg.""Id"" and mg.""IsDeleted""=false
                left join public.""ResourceLanguage"" as ""rlMenuGroupName"" on mg.""Id""=""rlMenuGroupName"".""TemplateId"" 
                and ""rlMenuGroupName"".""TemplateType""=14 and ""rlMenuGroupName"".""Code""='MenuGroupName'  and ""rlMenuGroupName"".""IsDeleted""=false
                left join 
                ( select upe.*
                from public.""UserRolePermission"" as upe 
                join public.""UserRole"" as ur2 on upe.""UserRoleId""=ur2.""Id""
                and ur2.""IsDeleted""=false 
                join public.""UserRoleUser"" as uru2 on ur2.""Id""=uru2.""UserRoleId""
                and uru2.""IsDeleted""=false 
                where  upe.""IsDeleted""=false 
                )upe on pa.""Id""=upe.""PageId""
                where  pa.""PortalId""='{portal.Id}' 
                and pa.""IsDeleted""=false and po.""IsDeleted""=false"
            , null);



            pages = pages.Where(x => x.IsAuthorized).ToList();

            var menus = pages.Where(x => x.MenuGroupName != null).GroupBy(x => x.MenuGroupId).Select(x => x.First()).OrderBy(x => x.MenuGroupSequenceOrder).Select(
                x => new MenuViewModel
                {
                    Id = x.MenuGroupId,
                    Name = x.MenuGroupName,
                    DisplayName = x.MenuGroupName,
                    ParentId = x.ParentId.Coalesce(portal.Id),
                    PortalId = portal.Id,
                    HasChild = true,
                    IconCss = x.MenuGroupIconCss.Coalesce("fa fa-user-circle"),
                    IconFileId = x.MenuGroupIconFileId,
                    MenuDetails = x.MenuGroupDetails,
                    PageType = TemplateTypeEnum.MenuGroup,
                    Type = "MenuGroup"
                }
                ).ToList();
            menus.AddRange(pages.GroupBy(x => x.Id).Select(x => x.First()).OrderBy(x => x.SequenceOrder).Select(x =>
              new MenuViewModel
              {
                  Id = x.Id,
                  Name = x.Name,
                  DisplayName = x.MenuName.Coalesce(x.Title).Coalesce(x.Name),
                  ParentId = x.MenuGroupId,
                  IconColor = x.IconColor.Coalesce("#fff"),
                  IconCss = x.IconCss.Coalesce("fa fa-user-circle"),
                  IsRoot = x.IsRootPage,
                  Url = string.Concat("/Portal/", portal.Name, "/", x.Name),
                  IsHidden = x.HideInMenu,
                  PortalId = x.PortalId,
                  HasChild = false,
                  PageType = x.PageType,
                  HideInMenu = x.HideInMenu,
                  ShowMenuWhenAuthorized = x.ShowMenuWhenAuthorized,
                  ShowMenuWhenNotAuthorized = x.ShowMenuWhenNotAuthorized,
                  AuthorizationNotRequired = x.AuthorizationNotRequired,
                  ShowOutsideMenuGroup = x.ShowOutsideMenuGroup,
                  Layout = x.Layout,
                  IconFileId = x.IconFileId,
                  MenuDetails = x.PageDetails,
                  Type = "Page"
              }
                ));


            menus.Add(new MenuViewModel
            {

                Id = portal.Id,
                Name = portal.Name,
                DisplayName = portal.Name,
                HasChild = true,
                Type = "PORTAL"
            });


            return menus;
        }

        public async Task<PortalViewModel> GetPortalByDomain(string domain)
        {
            return await _repo.GetSingle(x => x.DomainName == domain);
        }

        public async Task<List<IdNameViewModel>> GetPortalForUser(string id)
        {
            var companyPortals = await GetSingleById<CompanyViewModel, Company>(_repo.UserContext.CompanyId);
            var portalIds = string.Join("','", companyPortals.LicensedPortalIds);
            var query = $@"SELECT p.""Id"" as Id, p.""Name"" as Name FROM public.""Portal"" as p

 where p.""Id"" in ('{portalIds}')
ORDER BY p.""CreatedDate"" ASC";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            // queryData = queryData.Where(x => portalIds.Any(y => y == x.Id)).ToList();

            if (id.IsNotNullAndNotEmpty())
            {
                queryData = queryData.Where(x => x.Id == id).ToList();
            }

            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetPortals(string id)
        {
            var query = @"SELECT p.""Id"" as Id, p.""Name"" as Name FROM public.""Portal"" as p ORDER BY p.""CreatedDate"" ASC";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            if (id.IsNotNullAndNotEmpty())
            {
                queryData = queryData.Where(x => x.Id == id).ToList();
            }
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetAllowedPortals()
        {
            var companyPortals = await GetSingleById<CompanyViewModel, Company>(_repo.UserContext.CompanyId);
            var portalIds = string.Join("','", companyPortals.LicensedPortalIds);
            var query = $@"SELECT p.""Id"" as Id, p.""Name"" as Name FROM public.""Portal"" as p

 where p.""Id"" in ('{portalIds}')
ORDER BY p.""CreatedDate"" ASC";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);

            return queryData;
        }

        public async Task<List<PageViewModel>> GetMenuGroupOfPortal(string portalIds)
        {
            var query = $@"select mg.""Name"" as Name,sm.""Name"" as SubModuleName,m.""Name"" as ModuleName
,null as ParentId,mg.""Id"" as Id,'MenuGroup' as Type,mg.""Id"" as key,'' as title,true as lazy
from public.""MenuGroup"" as mg
left join public.""SubModule"" as sm on sm.""Id""= mg.""SubModuleId"" and sm.""IsDeleted""=false --and sm.""PortalId"" ='{_repo.UserContext.PortalId}'
left join public.""Module"" as m on m.""Id""= sm.""ModuleId"" and m.""IsDeleted""=false --and m.""PortalId"" ='{_repo.UserContext.PortalId}'
where mg.""IsDeleted""=false and mg.""PortalId"" in ('{portalIds}') ";

            var menugroup = await _queryRepo.ExecuteQueryList<PageViewModel>(query, null);
            var list = new List<PageViewModel>();
            foreach (var item in menugroup)
            {
                var query1 = $@"select p.""Title"" as Name,mg.""Name"" as MenuGroupName,sm.""Name"" as SubModuleName,m.""Name"" as ModuleName,
t.""DisplayName"" as TemplateName,p.""PageType"",'{item.Id}' as ParentId,p.""Id"" as Id,'Page' as Type,mg.""Id"" as key,'' as title,true as lazy
from public.""Page"" as p
join public.""MenuGroup"" as mg on mg.""Id""= p.""MenuGroupId"" and mg.""IsDeleted""=false --and mg.""PortalId"" ='{_repo.UserContext.PortalId}'
left join public.""SubModule"" as sm on sm.""Id""= mg.""SubModuleId"" and sm.""IsDeleted""=false --and sm.""PortalId"" ='{_repo.UserContext.PortalId}'
left join public.""Module"" as m on m.""Id""= sm.""ModuleId"" and m.""IsDeleted""=false --and m.""PortalId"" ='{_repo.UserContext.PortalId}'
left join public.""Template"" as t on t.""Id""= p.""TemplateId"" and t.""IsDeleted""=false --and t.""PortalId"" ='{_repo.UserContext.PortalId}'
where p.""IsDeleted""=false and mg.""Id""='{item.Id}' and p.""PortalId"" in ('{portalIds}') ";
                var page = await _queryRepo.ExecuteQueryList<PageViewModel>(query1, null);
                list.AddRange(page);
            }
            menugroup.AddRange(list);
            //var menuGroup = await _menuGroupBusiness.GetList(x => x.PortalId == _repo.UserContext.PortalId);

            //var list = new List<TreeViewViewModel>();
            //list = menuGroup.Select(x => new TreeViewViewModel { Name = x.Name, id = x.Id,ParentId=null,expanded=true }).ToList();
            //foreach(var item in menuGroup)
            //{
            //    var page = await _pageBusiness.GetList(x => x.MenuGroupId == item.Id && x.PortalId == _repo.UserContext.PortalId);
            //     list.AddRange(page.Select(x => new TreeViewViewModel { id = x.Id, Name = x.Name, ParentId = item.Id }).ToList());
            //}
            return menugroup;

        }

        public async Task<List<TreeViewViewModel>> GetNtsTemplateTreeList(string id, string[] portalIds)
        {

            var list = new List<TreeViewViewModel>();
            list.Add(new TreeViewViewModel
            {
                id = TemplateTypeEnum.Page.ToString(),
                Name = TemplateTypeEnum.Page.ToString(),
                DisplayName = TemplateTypeEnum.Page.ToString(),
                ParentId = null,
                // hasChildren = true,
                expanded = true,
                Type = "Root",
                key = TemplateTypeEnum.Page.ToString(),
                title = "",
                lazy = true
            });
            //var categoryP = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Page && x.PortalId == _repo.UserContext.PortalId && (x.ParentId == null || x.ParentId == ""));
            var categoryP = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Page && (x.ParentId == null || x.ParentId == ""));
            categoryP = categoryP.Where(x => portalIds.Contains(x.PortalId)).ToList();
            list.AddRange(categoryP.Select(x => new TreeViewViewModel
            {
                id = x.Id,
                Name = x.Name,
                DisplayName = x.Name,
                ParentId = TemplateTypeEnum.Page.ToString(),
                TemplateCode = x.Code,
                // hasChildren = true,
                expanded = true,
                Type = "TemplateCategory",
                key = x.Id,
                title = "",
                lazy = true
            }));
            foreach (var item in categoryP)
            {
                //var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id && x.PortalId == _repo.UserContext.PortalId);
                var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id);
                templates = templates.Where(x => portalIds.Contains(x.PortalId)).ToList();
                if (templates.Count > 0)
                {
                    list.AddRange(templates.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        ParentId = item.Id
                    ,
                        TemplateCode = x.Code,
                        Type = "Template",
                        DisplayName = x.DisplayName,
                    }).ToList());
                }
            }
            list.Add(new TreeViewViewModel
            {
                id = TemplateTypeEnum.Form.ToString(),
                Name = TemplateTypeEnum.Form.ToString(),
                DisplayName = TemplateTypeEnum.Form.ToString(),
                ParentId = null,
                //hasChildren = true,
                expanded = true,
                Type = "Root",
                key = TemplateTypeEnum.Form.ToString(),
                title = "",
                lazy = true
            });
            //var categoryF = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Form && x.PortalId == _repo.UserContext.PortalId && (x.ParentId == null || x.ParentId == ""));
            var categoryF = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Form && (x.ParentId == null || x.ParentId == ""));
            categoryF = categoryF.Where(x => portalIds.Contains(x.PortalId)).ToList();
            list.AddRange(categoryF.Select(x => new TreeViewViewModel
            {
                id = x.Id,
                Name = x.Name,
                DisplayName = x.Name,
                ParentId = TemplateTypeEnum.Form.ToString(),
                TemplateCode = x.Code,
                // hasChildren = true,
                expanded = true,
                Type = "TemplateCategory",
                key = x.Id,
                title = "",
                lazy = true

            }));
            foreach (var item in categoryF)
            {
                //var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id && x.PortalId == _repo.UserContext.PortalId);
                var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id);
                templates = templates.Where(x => portalIds.Contains(x.PortalId)).ToList();
                if (templates.Count > 0)
                {
                    list.AddRange(templates.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        ParentId = item.Id,
                        TemplateCode = x.Code,
                        Type = "Template",
                        key = x.Id,
                        title = "",
                        lazy = true
                    }).ToList());
                }
            }
            list.Add(new TreeViewViewModel
            {
                id = TemplateTypeEnum.Note.ToString(),
                Name = TemplateTypeEnum.Note.ToString(),
                DisplayName = TemplateTypeEnum.Note.ToString(),
                ParentId = null,
                //  hasChildren = true,
                expanded = true,
                Type = "Root",
                key = TemplateTypeEnum.Note.ToString(),
                title = "",
                lazy = true
            });
            //var categoryN = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Note && x.PortalId == _repo.UserContext.PortalId && (x.ParentId == null || x.ParentId == ""));
            var categoryN = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Note && (x.ParentId == null || x.ParentId == ""));
            categoryN = categoryN.Where(x => portalIds.Contains(x.PortalId)).ToList();
            list.AddRange(categoryN.Select(x => new TreeViewViewModel
            {
                id = x.Id,
                Name = x.Name,
                DisplayName = x.Name,
                ParentId = TemplateTypeEnum.Note.ToString(),
                TemplateCode = x.Code,
                // hasChildren = true,
                expanded = true,
                Type = "TemplateCategory",
                key = x.Id,
                title = "",
                lazy = true
            }));
            foreach (var item in categoryN)
            {
                //var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id && x.PortalId == _repo.UserContext.PortalId);
                var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id);
                templates = templates.Where(x => portalIds.Contains(x.PortalId)).ToList();
                if (templates.Count > 0)
                {
                    list.AddRange(templates.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        ParentId = item.Id,
                        TemplateCode = x.Code,
                        Type = "Template",
                        key = x.Id,
                        title = "",
                        lazy = true
                    }).ToList());
                }
            }
            list.Add(new TreeViewViewModel
            {
                id = TemplateTypeEnum.Task.ToString(),
                Name = TemplateTypeEnum.Task.ToString(),
                DisplayName = TemplateTypeEnum.Task.ToString(),
                ParentId = null,
                // hasChildren = true,
                expanded = true,
                Type = "Root",
                key = TemplateTypeEnum.Task.ToString(),
                title = "",
                lazy = true
            });
            //var categoryT = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task && x.PortalId == _repo.UserContext.PortalId && (x.ParentId == null || x.ParentId == ""));
            var categoryT = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Task && (x.ParentId == null || x.ParentId == ""));
            categoryT = categoryT.Where(x => portalIds.Contains(x.PortalId)).ToList();
            list.AddRange(categoryT.Select(x => new TreeViewViewModel
            {
                id = x.Id,
                Name = x.Name,
                DisplayName = x.Name,
                ParentId = TemplateTypeEnum.Task.ToString(),
                TemplateCode = x.Code,
                // hasChildren = true,
                expanded = true,
                Type = "TemplateCategory",
                key = x.Id,
                title = "",
                lazy = true

            }));
            foreach (var item in categoryT)
            {
                //var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id && x.PortalId == _repo.UserContext.PortalId);
                var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id);
                templates = templates.Where(x => portalIds.Contains(x.PortalId)).ToList();
                if (templates.Count > 0)
                {
                    list.AddRange(templates.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        ParentId = item.Id,
                        TemplateCode = x.Code,
                        Type = "Template",
                        key = x.Id,
                        title = "",
                        lazy = true
                    }).ToList());
                }
            }
            list.Add(new TreeViewViewModel
            {
                id = TemplateTypeEnum.Service.ToString(),
                Name = TemplateTypeEnum.Service.ToString(),
                DisplayName = TemplateTypeEnum.Service.ToString(),
                ParentId = null,
                //hasChildren = true,
                expanded = true,
                Type = "Root",
                key = TemplateTypeEnum.Service.ToString(),
                title = "",
                lazy = true
            });
            //var categoryS = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Service && x.PortalId == _repo.UserContext.PortalId && (x.ParentId == null || x.ParentId == ""));
            var categoryS = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Service && (x.ParentId == null || x.ParentId == ""));
            categoryS = categoryS.Where(x => portalIds.Contains(x.PortalId)).ToList();
            list.AddRange(categoryS.Select(x => new TreeViewViewModel
            {
                id = x.Id,
                Name = x.Name,
                DisplayName = x.Name,
                ParentId = TemplateTypeEnum.Service.ToString(),
                TemplateCode = x.Code,
                // hasChildren = true,
                expanded = true,
                Type = "TemplateCategory",
                key = x.Id,
                title = "",
                lazy = true

            }));
            foreach (var item in categoryS)
            {
                //var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id && x.PortalId == _repo.UserContext.PortalId);
                var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id);
                templates = templates.Where(x => portalIds.Contains(x.PortalId)).ToList();
                if (templates.Count > 0)
                {
                    list.AddRange(templates.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        ParentId = item.Id,
                        TemplateCode = x.Code,
                        Type = "Template",
                        key = x.Id,
                        title = "",
                        lazy = true
                    }).ToList());
                }
            }
            list.Add(new TreeViewViewModel
            {
                id = TemplateTypeEnum.Custom.ToString(),
                Name = TemplateTypeEnum.Custom.ToString(),
                DisplayName = TemplateTypeEnum.Custom.ToString(),
                ParentId = null,
                // hasChildren = true,
                expanded = true,
                Type = "Root",
                key = TemplateTypeEnum.Custom.ToString(),
                title = "",
                lazy = true

            });
            //var categoryC = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Custom && x.PortalId == _repo.UserContext.PortalId && (x.ParentId == null || x.ParentId == ""));
            var categoryC = await _categoryBusiness.GetList(x => x.TemplateType == TemplateTypeEnum.Custom && x.PortalId == _repo.UserContext.PortalId && (x.ParentId == null || x.ParentId == ""));
            categoryC = categoryC.Where(x => portalIds.Contains(x.PortalId)).ToList();
            list.AddRange(categoryC.Select(x => new TreeViewViewModel
            {
                id = x.Id,
                Name = x.Name,
                DisplayName = x.Name,
                ParentId = TemplateTypeEnum.Custom.ToString(),
                TemplateCode = x.Code,
                // hasChildren = true,
                expanded = true,
                Type = "TemplateCategory",
                key = x.Id,
                title = "",
                lazy = true

            }));
            foreach (var item in categoryC)
            {
                //var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id && x.PortalId == _repo.UserContext.PortalId);
                var templates = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id);
                templates = templates.Where(x => portalIds.Contains(x.PortalId)).ToList();
                if (templates.Count > 0)
                {
                    list.AddRange(templates.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        ParentId = item.Id,
                        TemplateCode = x.Code,
                        Type = "Template",
                        key = x.Id,
                        title = "",
                        lazy = true
                    }).ToList());
                }
            }




            return list;
        }
    }
}
