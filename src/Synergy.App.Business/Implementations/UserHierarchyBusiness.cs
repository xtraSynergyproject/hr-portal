using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class UserHierarchyBusiness : BusinessBase<UserHierarchyViewModel, UserHierarchy>, IUserHierarchyBusiness
    {
        private readonly IRepositoryQueryBase<UserHierarchyViewModel> _queryRepo;
        private readonly IHierarchyMasterBusiness _hierarchyMasterBusiness;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;


        public UserHierarchyBusiness(IRepositoryBase<UserHierarchyViewModel, UserHierarchy> repo, IMapper autoMapper, IRepositoryQueryBase<UserHierarchyViewModel> queryRepo, ICmsQueryBusiness cmsQueryBusiness,
          IHierarchyMasterBusiness hierarchyMasterBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<UserHierarchyViewModel>> Create(UserHierarchyViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model,autoCommit);

            return CommandResult<UserHierarchyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserHierarchyViewModel>> Edit(UserHierarchyViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);

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

            var list = await _cmsQueryBusiness.GetHierarchyList(HierarchyId, userId);
            return list;
        }
        public async Task<IList<UserHierarchyViewModel>> GetHierarchyListForAllPortals(string HierarchyId)
        {

            var list = await _cmsQueryBusiness.GetHierarchyListForAllPortals(HierarchyId);
            return list;
        }
        public async Task<IList<UserHierarchyViewModel>> GetHierarchyListForPortal(string HierarchyId, string userId)
        {
            var list = await _cmsQueryBusiness.GetHierarchyListForPortal(HierarchyId, userId);
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

            var list = await _cmsQueryBusiness.GetHierarchyUser(HierarchyId, userId);
            return list;
        }

        public async Task<List<UserViewModel>> GetHierarchyUsers(string hierarchyCode, string parentUserId,int level,int option)
        {


            var list = await _cmsQueryBusiness.GetHierarchyUsers(hierarchyCode, parentUserId, level, option);
            return list;
        }

        public async Task<List<UserHierarchyViewModel>> GetPerformanceHierarchyUsers(string parentUserId)
        {


            var list = await _cmsQueryBusiness.GetPerformanceHierarchyUsers(parentUserId);
            return list;
        }
        public async Task<List<UserHierarchyViewModel>> GetUserHierarchyByCode(string code,string parentUserId)
        {

            var list = await _cmsQueryBusiness.GetUserHierarchyByCode(code, parentUserId);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetNonExistingUser(string hierarchyId,string userId)
        {
            var list = await _cmsQueryBusiness.GetNonExistingUser(hierarchyId, userId);
            return list;
        }
    }
}
