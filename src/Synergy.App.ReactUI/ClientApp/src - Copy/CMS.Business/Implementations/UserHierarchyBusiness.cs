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
    public class UserHierarchyBusiness : BusinessBase<UserHierarchyViewModel, UserHierarchy>, IUserHierarchyBusiness
    {
        private readonly IRepositoryQueryBase<UserHierarchyViewModel> _queryRepo;
        private readonly IHierarchyMasterBusiness _hierarchyMasterBusiness;
       

        public UserHierarchyBusiness(IRepositoryBase<UserHierarchyViewModel, UserHierarchy> repo, IMapper autoMapper, IRepositoryQueryBase<UserHierarchyViewModel> queryRepo,
          IHierarchyMasterBusiness hierarchyMasterBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
        }

        public async override Task<CommandResult<UserHierarchyViewModel>> Create(UserHierarchyViewModel model)
        {
            var result = await base.Create(model);

            return CommandResult<UserHierarchyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserHierarchyViewModel>> Edit(UserHierarchyViewModel model)
        {
            var result = await base.Edit(model);

            return CommandResult<UserHierarchyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<IList<UserHierarchyViewModel>> GetLeaveApprovalHierarchyDetailsOfUser(string userId)
        {
           
            var hierarchyId = await _hierarchyMasterBusiness.GetSingle(x=>x.Code=="Leave");
          
            var userHierarchy = await GetHierarchyList(hierarchyId.Id, userId);
            //if (userHierarchy != null)
            //{
            //    var levels = GetLevelsByHierarchy(hierarchyId);
            //    if (levels != null)
            //    {
            //        foreach (var le in levels)
            //        {
            //            if (le.Id == 1)
            //            {
            //                userHierarchy.Level1Name = le.Name;
            //            }
            //            else if (le.Id == 2)
            //            {
            //                userHierarchy.Level2Name = le.Name;
            //            }
            //            else if (le.Id == 3)
            //            {
            //                userHierarchy.Level3Name = le.Name;
            //            }
            //            else if (le.Id == 4)
            //            {
            //                userHierarchy.Level4Name = le.Name;
            //            }
            //            else if (le.Id == 5)
            //            {
            //                userHierarchy.Level5Name = le.Name;
            //            }
            //        }
            //    }
            //}
            
            return userHierarchy;
        }

        public async Task<IList<UserHierarchyViewModel>> GetHierarchyList(string HierarchyId,string userId) 
        {
            var query = @$"select u.""Id"" as UserIds,u.""Name"" as UserName,ur11.""ParentUserId"" as Level1ApproverOption1UserId,ur11.""username"" as Level1ApproverOption1UserName,ur12.""ParentUserId"" as Level1ApproverOption2UserId,ur12.""username"" as Level1ApproverOption2UserName,ur13.""ParentUserId"" as Level1ApproverOption3UserId,ur13.""username"" as Level1ApproverOption3UserName,
ur21.""ParentUserId"" as Level2ApproverOption1UserId,ur21.""username"" as Level2ApproverOption1UserName,ur22.""ParentUserId"" as Level2ApproverOption2UserId,ur22.""username"" as Level2ApproverOption2UserName,ur23.""ParentUserId"" as Level2ApproverOption3UserId,ur23.""username"" as Level2ApproverOption3UserName,
ur31.""ParentUserId"" as Level3ApproverOption1UserId,ur31.""username"" as Level3ApproverOption1UserName,ur32.""ParentUserId"" as Level3ApproverOption2UserId,ur32.""username"" as Level3ApproverOption2UserName,ur33.""ParentUserId"" as Level3ApproverOption3UserId,ur33.""username"" as Level3ApproverOption3UserName,
ur41.""ParentUserId"" as Level4ApproverOption1UserId,ur41.""username"" as Level4ApproverOption1UserName,ur42.""ParentUserId"" as Level4ApproverOption2UserId,ur42.""username"" as Level4ApproverOption2UserName,ur43.""ParentUserId"" as Level4ApproverOption3UserId,ur43.""username"" as Level4ApproverOption3UserName,
ur51.""ParentUserId"" as Level5ApproverOption1UserId,ur51.""username"" as Level5ApproverOption1UserName,ur52.""ParentUserId"" as Level5ApproverOption2UserId,ur52.""username"" as Level5ApproverOption2UserName,ur53.""ParentUserId"" as Level5ApproverOption3UserId,ur53.""username"" as Level5ApproverOption3UserName
from public.""User"" as u
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	  u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur11   on ur11.""UserId"" = u.""Id""
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur12   on ur12.""UserId"" = u.""Id""
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur13   on ur13.""UserId"" = u.""Id""	

		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur21   on ur21.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur22   on ur22.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur23   on ur23.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur31   on ur31.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur32   on ur32.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur33   on ur33.""UserId"" = u.""Id""		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur41   on ur41.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur42   on ur42.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur43   on ur43.""UserId"" = u.""Id""	
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur51   on ur51.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur52   on ur52.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur53   on ur53.""UserId"" = u.""Id""		

        #Where#
                                ";

            var where = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                where = @$"  where u.""Id""='{userId}' ";
            }
            query = query.Replace("#Where#", where);
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }
        public async Task<IList<UserHierarchyViewModel>> GetHierarchyListForPortal(string HierarchyId, string userId)
        {
            var query = @$"select u.""Id"" as UserIds,u.""Name"" as UserName,ur11.""ParentUserId"" as Level1ApproverOption1UserId,ur11.""username"" as Level1ApproverOption1UserName,ur12.""ParentUserId"" as Level1ApproverOption2UserId,ur12.""username"" as Level1ApproverOption2UserName,ur13.""ParentUserId"" as Level1ApproverOption3UserId,ur13.""username"" as Level1ApproverOption3UserName,
ur21.""ParentUserId"" as Level2ApproverOption1UserId,ur21.""username"" as Level2ApproverOption1UserName,ur22.""ParentUserId"" as Level2ApproverOption2UserId,ur22.""username"" as Level2ApproverOption2UserName,ur23.""ParentUserId"" as Level2ApproverOption3UserId,ur23.""username"" as Level2ApproverOption3UserName,
ur31.""ParentUserId"" as Level3ApproverOption1UserId,ur31.""username"" as Level3ApproverOption1UserName,ur32.""ParentUserId"" as Level3ApproverOption2UserId,ur32.""username"" as Level3ApproverOption2UserName,ur33.""ParentUserId"" as Level3ApproverOption3UserId,ur33.""username"" as Level3ApproverOption3UserName,
ur41.""ParentUserId"" as Level4ApproverOption1UserId,ur41.""username"" as Level4ApproverOption1UserName,ur42.""ParentUserId"" as Level4ApproverOption2UserId,ur42.""username"" as Level4ApproverOption2UserName,ur43.""ParentUserId"" as Level4ApproverOption3UserId,ur43.""username"" as Level4ApproverOption3UserName,
ur51.""ParentUserId"" as Level5ApproverOption1UserId,ur51.""username"" as Level5ApproverOption1UserName,ur52.""ParentUserId"" as Level5ApproverOption2UserId,ur52.""username"" as Level5ApproverOption2UserName,ur53.""ParentUserId"" as Level5ApproverOption3UserId,ur53.""username"" as Level5ApproverOption3UserName
from public.""User"" as u
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	  u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and ur.""CompanyId""='{_repo.UserContext.CompanyId}'
	where ur.""LevelNo""=1 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}' 
        ) as ur11   on ur11.""UserId"" = u.""Id"" and u.""CompanyId""='{_repo.UserContext.CompanyId}'
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur12   on ur12.""UserId"" = u.""Id""
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false 
	where ur.""LevelNo""=1 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur13   on ur13.""UserId"" = u.""Id""	

		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur21   on ur21.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur22   on ur22.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur23   on ur23.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur31   on ur31.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur32   on ur32.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur33   on ur33.""UserId"" = u.""Id""		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur 
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur41   on ur41.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur42   on ur42.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur43   on ur43.""UserId"" = u.""Id""	
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""   and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur51   on ur51.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur52   on ur52.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId""  and ur.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false 
	where ur.""LevelNo""=5 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur53   on ur53.""UserId"" = u.""Id""		
where  u.""LegalEntityId""='{_repo.UserContext.LegalEntityId}' and u.""PortalId""='{_repo.UserContext.PortalId}'
        #Where#
                                ";

            var where = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                where = @$"  and u.""Id""='{userId}' ";
            }
            query = query.Replace("#Where#", where);
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }
        public async Task<CommandResult<UserHierarchyViewModel>> CreateUserHierarchy(UserHierarchyViewModel viewModel)
        {
            if (!viewModel.UserIds.IsNullOrEmpty())
            {
                var Str = viewModel.UserIds.Trim(',');
                var ids = Str.Split(',');
                foreach (var userId in ids)
                {
                   // var userId = id.();

                   // UpdateUserHierarchyAdmin(viewModel.HierarchyId, userId, viewModel.AdminUserId);

                   await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level1ApproverOption1UserId, 1, 1);
                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level1ApproverOption2UserId, 1, 2);
                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level1ApproverOption3UserId, 1, 3);

                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level2ApproverOption1UserId, 2, 1);
                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level2ApproverOption2UserId, 2, 2);
                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level2ApproverOption3UserId, 2, 3);

                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level3ApproverOption1UserId, 3, 1);
                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level3ApproverOption2UserId, 3, 2);
                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level3ApproverOption3UserId, 3, 3);

                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level4ApproverOption1UserId, 4, 1);
                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level4ApproverOption2UserId, 4, 2);
                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level4ApproverOption3UserId, 4, 3);

                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level5ApproverOption1UserId, 5, 1);
                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level5ApproverOption2UserId, 5, 2);
                    await UpdateHierarchyLevel(viewModel.HierarchyId, userId, viewModel.Level5ApproverOption3UserId, 5, 3);
                }
            }

           
            return CommandResult<UserHierarchyViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<UserHierarchyViewModel>> UpdateHierarchyLevel(string hierarchyId, string userId, string levelUserId, int levelNo, int optionNo)
        {
            if (levelUserId.IsNotNullAndNotEmpty())
            {
                var res = await GetSingle(x => x.HierarchyMasterId == hierarchyId && x.UserId == userId && x.LevelNo == levelNo && x.OptionNo == optionNo);
                if (res.IsNotNull())
                {
                    res.HierarchyMasterId = hierarchyId;
                    res.ParentUserId = levelUserId;
                    res.LevelNo = levelNo;
                    res.OptionNo = optionNo;
                    res.UserId = userId;
                    await Edit(res);
                }

                else
                {


                    var data = new UserHierarchyViewModel
                    {

                        HierarchyMasterId = hierarchyId,
                        ParentUserId = levelUserId,
                        LevelNo = levelNo,
                        OptionNo = optionNo,
                        UserId = userId

                    };
                    await Create(data);
                }
                    
            }
            return CommandResult<UserHierarchyViewModel>.Instance();
        }


        public async Task<CommandResult<UserHierarchyViewModel>> CreateUserHierarchyForPortal(UserHierarchyViewModel viewModel)
        {
            if (!viewModel.UserIds.IsNullOrEmpty())
            {
                var Str = viewModel.UserIds.Trim(',');
                var ids = Str.Split(',');
                foreach (var userId in ids)
                {
                    // var userId = id.();

                    // UpdateUserHierarchyAdmin(viewModel.HierarchyId, userId, viewModel.AdminUserId);

                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level1ApproverOption1UserId, 1, 1);
                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level1ApproverOption2UserId, 1, 2);
                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level1ApproverOption3UserId, 1, 3);

                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level2ApproverOption1UserId, 2, 1);
                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level2ApproverOption2UserId, 2, 2);
                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level2ApproverOption3UserId, 2, 3);

                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level3ApproverOption1UserId, 3, 1);
                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level3ApproverOption2UserId, 3, 2);
                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level3ApproverOption3UserId, 3, 3);

                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level4ApproverOption1UserId, 4, 1);
                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level4ApproverOption2UserId, 4, 2);
                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level4ApproverOption3UserId, 4, 3);

                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level5ApproverOption1UserId, 5, 1);
                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level5ApproverOption2UserId, 5, 2);
                    await UpdateHierarchyLevelForPortal(viewModel.HierarchyId, userId, viewModel.Level5ApproverOption3UserId, 5, 3);
                }
            }


            return CommandResult<UserHierarchyViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<UserHierarchyViewModel>> UpdateHierarchyLevelForPortal(string hierarchyId, string userId, string levelUserId, int levelNo, int optionNo)
        {
            if (levelUserId.IsNotNullAndNotEmpty())
            {
                var res = await GetSingle(x => x.HierarchyMasterId == hierarchyId && x.UserId == userId && x.LevelNo == levelNo && x.OptionNo == optionNo);
                if (res.IsNotNull())
                {
                    res.HierarchyMasterId = hierarchyId;
                    res.ParentUserId = levelUserId;
                    res.LevelNo = levelNo;
                    res.OptionNo = optionNo;
                    res.UserId = userId;
                    await Edit(res);
                }

                else
                {


                    var data = new UserHierarchyViewModel
                    {

                        HierarchyMasterId = hierarchyId,
                        ParentUserId = levelUserId,
                        LevelNo = levelNo,
                        OptionNo = optionNo,
                        UserId = userId,
                        LegalEntityId=_repo.UserContext.LegalEntityId,
                        PortalId = _repo.UserContext.PortalId,
                    };
                    await Create(data);
                }

            }
            return CommandResult<UserHierarchyViewModel>.Instance();
        }


        public async Task<UserHierarchyViewModel> GetLeaveApprovalHierarchyUser(string userId, string hierarchyId)
        {

            //var hierarchyId = await _hierarchyMasterBusiness.GetSingle(x => x.Code == "Leave");
            var hierarchy = await _hierarchyMasterBusiness.GetSingleById(hierarchyId);

            var userHierarchy = await GetHierarchyUser(hierarchy.Id, userId);
            userHierarchy.Level1Name = hierarchy.Level1Name;
            userHierarchy.Level2Name = hierarchy.Level2Name;
            userHierarchy.Level3Name = hierarchy.Level3Name;
            userHierarchy.Level4Name = hierarchy.Level4Name;
            userHierarchy.Level5Name = hierarchy.Level5Name;

            //if (userHierarchy != null)
            //{
            //    var levels = GetLevelsByHierarchy(hierarchyId);
            //    if (levels != null)
            //    {
            //        foreach (var le in levels)
            //        {
            //            if (le.Id == 1)
            //            {
            //                userHierarchy.Level1Name = le.Name;
            //            }
            //            else if (le.Id == 2)
            //            {
            //                userHierarchy.Level2Name = le.Name;
            //            }
            //            else if (le.Id == 3)
            //            {
            //                userHierarchy.Level3Name = le.Name;
            //            }
            //            else if (le.Id == 4)
            //            {
            //                userHierarchy.Level4Name = le.Name;
            //            }
            //            else if (le.Id == 5)
            //            {
            //                userHierarchy.Level5Name = le.Name;
            //            }
            //        }
            //    }
            //}

            return userHierarchy;
        }

        public async Task<UserHierarchyViewModel> GetHierarchyUser(string HierarchyId, string userId)
        {
            var query = @$"select u.""Id"" as UserIds,u.""Name"" as UserName,ur11.""ParentUserId"" as Level1ApproverOption1UserId,ur11.""username"" as Level1ApproverOption1UserName,ur12.""ParentUserId"" as Level1ApproverOption2UserId,ur12.""username"" as Level1ApproverOption2UserName,ur13.""ParentUserId"" as Level1ApproverOption3UserId,ur13.""username"" as Level1ApproverOption3UserName,
ur21.""ParentUserId"" as Level2ApproverOption1UserId,ur21.""username"" as Level2ApproverOption1UserName,ur22.""ParentUserId"" as Level2ApproverOption2UserId,ur22.""username"" as Level2ApproverOption2UserName,ur23.""ParentUserId"" as Level2ApproverOption3UserId,ur23.""username"" as Level2ApproverOption3UserName,
ur31.""ParentUserId"" as Level3ApproverOption1UserId,ur31.""username"" as Level3ApproverOption1UserName,ur32.""ParentUserId"" as Level3ApproverOption2UserId,ur32.""username"" as Level3ApproverOption2UserName,ur33.""ParentUserId"" as Level3ApproverOption3UserId,ur33.""username"" as Level3ApproverOption3UserName,
ur41.""ParentUserId"" as Level4ApproverOption1UserId,ur41.""username"" as Level4ApproverOption1UserName,ur42.""ParentUserId"" as Level4ApproverOption2UserId,ur42.""username"" as Level4ApproverOption2UserName,ur43.""ParentUserId"" as Level4ApproverOption3UserId,ur43.""username"" as Level4ApproverOption3UserName,
ur51.""ParentUserId"" as Level5ApproverOption1UserId,ur51.""username"" as Level5ApproverOption1UserName,ur52.""ParentUserId"" as Level5ApproverOption2UserId,ur52.""username"" as Level5ApproverOption2UserName,ur53.""ParentUserId"" as Level5ApproverOption3UserId,ur53.""username"" as Level5ApproverOption3UserName
from public.""User"" as u
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	  u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur11   on ur11.""UserId"" = u.""Id""
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur12   on ur12.""UserId"" = u.""Id""
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=1 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur13   on ur13.""UserId"" = u.""Id""	

		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur21   on ur21.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur22   on ur22.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=2 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur23   on ur23.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur31   on ur31.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur32   on ur32.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=3 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur33   on ur33.""UserId"" = u.""Id""		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur41   on ur41.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur42   on ur42.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=4 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur43   on ur43.""UserId"" = u.""Id""	
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=1 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur51   on ur51.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=2 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur52   on ur52.""UserId"" = u.""Id""	
		
		
left join (
       SELECT ur.""Id"", ur.""HierarchyMasterId"", ur.""UserId"", ur.""ParentUserId"", ur.""LevelNo"", ur.""OptionNo"",ur.""IsDeleted"",
	u.""Name"" as username
	FROM public.""UserHierarchy"" as ur
	left join public.""User"" as u on u.""Id""=ur.""ParentUserId"" and u.""IsDeleted""=false
	where ur.""LevelNo""=5 and  ur.""OptionNo""=3 and ur.""IsDeleted""=false and ur.""HierarchyMasterId""='{HierarchyId}'
        ) as ur53   on ur53.""UserId"" = u.""Id""		

        #Where#
                                ";

            var where = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                where = @$"  where u.""Id""='{userId}' ";
            }
            query = query.Replace("#Where#", where);
            var list = await _queryRepo.ExecuteQuerySingle(query, null);
            return list;
        }

        public async Task<List<UserViewModel>> GetHierarchyUsers(string hierarchyCode, string parentUserId,int level,int option)
        {
            var query = @$" select u.*
from public.""HierarchyMaster"" as hm
join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false
join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false
where hm.""Code""='{hierarchyCode}' and uh.""ParentUserId""='{parentUserId}'
and uh.""LevelNo""={level} and uh.""OptionNo""={option}   and hm.""IsDeleted""=false ";

           
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }

        public async Task<List<UserHierarchyViewModel>> GetPerformanceHierarchyUsers(string parentUserId)
        {
            var query = @$" WITH RECURSIVE pos AS(
                                select uh.*,'DIRECT' as Type
from public.""HierarchyMaster"" as hm
join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false
join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false
where hm.""Code""='PERFORMANCE_HIERARCHY' and uh.""ParentUserId""='{parentUserId}'
and uh.""LevelNo""=1 and uh.""OptionNo""=1   and hm.""IsDeleted""=false


                              union all

                                  select uh.*,'INDIRECT' as Type
from public.""HierarchyMaster"" as hm
join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false	 
join pos as po on po.""UserId""=uh.""ParentUserId""	 
join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false
where hm.""Code""='PERFORMANCE_HIERARCHY' 
and uh.""LevelNo""=1 and uh.""OptionNo""=1   and hm.""IsDeleted""=false
                             )
                            select* from pos ";


            var list = await _queryRepo.ExecuteQueryList<UserHierarchyViewModel>(query, null);
            return list;
        }
        public async Task<List<UserHierarchyViewModel>> GetUserHierarchyByCode(string code,string parentUserId)
        {
            var query = @$" WITH RECURSIVE pos AS(
                                select uh.*,'DIRECT' as Type
                from public.""HierarchyMaster"" as hm
                join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false
                join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false
                where hm.""Code""='{code}' and uh.""ParentUserId""='{parentUserId}'
                and uh.""LevelNo""=1 and uh.""OptionNo""=1   and hm.""IsDeleted""=false


                                              union all

                                                  select uh.*,'INDIRECT' as Type
                from public.""HierarchyMaster"" as hm
                join public.""UserHierarchy"" as uh on uh.""HierarchyMasterId""=hm.""Id"" and uh.""IsDeleted""=false	 
                join pos as po on po.""UserId""=uh.""ParentUserId""	 
                join public.""User"" as u on u.""Id""=uh.""UserId"" and u.""IsDeleted""=false
                where hm.""Code""='{code}' 
                and uh.""LevelNo""=1 and uh.""OptionNo""=1   and hm.""IsDeleted""=false
                                             )
                            select* from pos ";


            var list = await _queryRepo.ExecuteQueryList<UserHierarchyViewModel>(query, null);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetNonExistingUser(string hierarchyId,string userId)
        {
            string query = $@" select u.""Id"" as Id ,u.""Name"" as Name
                            from public.""User"" as u
join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
where ((u.""IsDeleted""=false and  u.""CompanyId""='{_repo.UserContext.CompanyId}') and u.""Id"" not in(SELECT ""RootNodeId"" FROM public.""HierarchyMaster"" 
where ""Id""='{hierarchyId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}') and u.""Id"" not in 
	  (SELECT ""UserId"" FROM public.""UserHierarchy"" 
where ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}') 
and u.""Id"" not in 	  (SELECT ""ParentUserId"" FROM public.""UserHierarchy"" 
where ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}' and ""IsDeleted""=false and ""ParentUserId"" is not null))  
and up.""PortalId""='{_repo.UserContext.PortalId}'";
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;
        }
    }
}
