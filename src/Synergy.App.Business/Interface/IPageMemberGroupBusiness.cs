using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IPageMemberBusiness : IBusinessBase<PageMemberViewModel,PageMember>
    {
        Task<List<TreeViewViewModel>> GetPagePermission(string id, string portalId);
    }
}
