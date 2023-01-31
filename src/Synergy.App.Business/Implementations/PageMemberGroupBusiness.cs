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
    public class PageMemberGroupBusiness : BusinessBase<PageMemberGroupViewModel, PageMemberGroup>, IPageMemberGroupBusiness
    {
        public PageMemberGroupBusiness(IRepositoryBase<PageMemberGroupViewModel, PageMemberGroup> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<PageMemberGroupViewModel>> Create(PageMemberGroupViewModel model, bool autoCommit = true)
        {
            var existlist = await GetList(x => x.PortalId == model.PortalId && x.PageId == model.PageId);
            try
            {
                if (!model.MemberGroupIds.IsNullOrEmpty())
                {
                    var str = model.MemberGroupIds.TrimEnd(',').Split(',');
                    foreach (var item in str)
                    {
                        var exist = await GetSingle(x => x.PortalId == model.PortalId && x.PageId == model.PageId && x.MemberGroupId == item);
                        if (exist == null)
                        {
                            model.Id = null;
                            var data = _autoMapper.Map<PageMemberGroupViewModel>(model);
                            data.MemberGroupId = item;
                            await base.Create(data, autoCommit);
                        }
                    }
                    existlist = existlist.Where(x => !str.Contains(x.MemberGroupId)).ToList();

                }

                foreach (var item in existlist)
                {
                    await Delete(item.Id);
                }
            }
            catch (Exception ex)
            {

            }
            return CommandResult<PageMemberGroupViewModel>.Instance(model);
        }

        public async override Task<CommandResult<PageMemberGroupViewModel>> Edit(PageMemberGroupViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<PageMemberGroupViewModel>(model);           
            var result = await base.Edit(data,autoCommit);

            return CommandResult<PageMemberGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<List<TreeViewViewModel>> GetPageMemberGroupPermission(string id, string portalId)
        {
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {               
                list.Add(new TreeViewViewModel
                {
                    id = "membergroup",
                    Name = "Member Group",
                    DisplayName = "Member Group",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = ContentTypeEnum.Root.ToString()

                });

            }
            
            if (id == "membergroup")
            {
                var group = await _repo.GetList<MemberGroupViewModel, MemberGroup>(x => x.GroupPortals.Contains(portalId));
                if (group != null && group.Any())
                {
                    list.AddRange(group.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.Name,
                        DisplayName = x.Name,
                        ParentId = "membergroup",
                        hasChildren = false,
                    }));
                }
                var memberlist = await GetList();
                list.ForEach(x => x.Checked = memberlist.Any(y => y.MemberGroupId == x.id));
            }
            return list;
        }
    }
}
