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
    public class PageMemberBusiness : BusinessBase<PageMemberViewModel, PageMember>, IPageMemberBusiness
    {
        private readonly IPageMemberGroupBusiness _groupBusiness;
        public PageMemberBusiness(IRepositoryBase<PageMemberViewModel, PageMember> repo, IMapper autoMapper, IPageMemberGroupBusiness groupBusiness) : base(repo, autoMapper)
        {
            _groupBusiness = groupBusiness;
        }

        public async override Task<CommandResult<PageMemberViewModel>> Create(PageMemberViewModel model)
        {
            var existlist = await GetList(x => x.PortalId == model.PortalId && x.PageId == model.PageId);
            try
            {
                if (!model.MemberIds.IsNullOrEmpty())
                {
                    var str = model.MemberIds.TrimEnd(',').Split(',');
                    foreach (var item in str)
                    {
                        var exist = await GetSingle(x => x.PortalId == model.PortalId && x.PageId == model.PageId && x.MemberId == item);
                        if (exist == null)
                        {
                            model.Id = null;
                            var data = _autoMapper.Map<PageMemberViewModel>(model);
                            data.MemberId = item;
                            await base.Create(data);
                        }
                    }
                    existlist = existlist.Where(x=>!str.Contains(x.MemberId)).ToList();

                }

                foreach (var item in existlist)
                {
                    await Delete(item.Id);
                }
            }
            catch(Exception ex)
            {

            }

            return CommandResult<PageMemberViewModel>.Instance(model);
        }

        public async override Task<CommandResult<PageMemberViewModel>> Edit(PageMemberViewModel model)
        {

            var data = _autoMapper.Map<PageMemberViewModel>(model);
          
            var result = await base.Edit(data);

            return CommandResult<PageMemberViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<TreeViewViewModel>> GetPagePermission(string id,string portalId)
        {
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                list.Add(new TreeViewViewModel
                {
                    id = "member",
                    Name = "Member",
                    DisplayName = "Member",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = ContentTypeEnum.Root.ToString()

                });
                          }
            if(id == "member")
            {
                var member = await _repo.GetList<MemberViewModel, Member>(x => x.MemberPortals.Contains(portalId));
                if (member != null && member.Any())
                {
                    list.AddRange(member.Select(x => new TreeViewViewModel
                    {
                        id = x.Id,
                        Name = x.UserName,
                        DisplayName = x.UserName,
                        ParentId = "member",
                        hasChildren = false,
                    }));
                }
                var memberlist = await GetList();
                list.ForEach(x => x.Checked = memberlist.Any(y => y.MemberId == x.id));
            }
           
            return list;
        }
    }
}
