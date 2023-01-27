using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class BusinessDataBusiness : BusinessBase<BusinessDataViewModel, BusinessData>, IBusinessDataBusiness
    {
        public BusinessDataBusiness(IRepositoryBase<BusinessDataViewModel, BusinessData> _repo, IMapper _autoMapper) : base(_repo, _autoMapper)
        {

        }


        public async Task<List<BusinessDataTreeViewModel>> GetBusinessDataTreeList(string companyId)
        {

            var list = new List<BusinessDataTreeViewModel>();

            var baList = new List<BusinessAreaViewModel>();
            var bsList = new List<BusinessSectionViewModel>();
            var breGroupList = new List<BreGroupViewModel>();
            var brList = new List<BusinessDataViewModel>();

            await Task.WhenAll(
                Task.Run(() =>
                {
                    baList = GetList<BusinessAreaViewModel, BusinessArea>(x => x.ParentId != null).Result.ToList();
                }),
                Task.Run(() =>
                {
                    bsList = GetList<BusinessSectionViewModel, BusinessArea>(x => x.ParentId != null).Result.ToList();
                }),
                Task.Run(() =>
                {
                    breGroupList = GetList<BreGroupViewModel, BusinessArea>(x => x.ParentId != null).Result.ToList();
                }),
                Task.Run(() =>
                {
                    brList = GetList<BusinessDataViewModel, BusinessArea>(x => x.ParentId != null).Result.ToList();
                })
            );


            list.AddRange(baList.Select(x => new BusinessDataTreeViewModel { Id = x.Id, CompanyId = x.CompanyId, Name = x.Name, ParentId = x.ParentId, BusinessDataTreeNodeType = BusinessDataTreeNodeTypeEnum.BusinessArea }));
            list.AddRange(bsList.Select(x => new BusinessDataTreeViewModel { Id = x.Id, CompanyId = x.CompanyId, Name = x.Name, ParentId = x.ParentId, BusinessDataTreeNodeType = BusinessDataTreeNodeTypeEnum.BusinessSection }));
            list.AddRange(breGroupList.Select(x => new BusinessDataTreeViewModel { Id = x.Id, CompanyId = x.CompanyId, Name = x.Name, ParentId = x.ParentId, BusinessDataTreeNodeType = BusinessDataTreeNodeTypeEnum.BusinessDataGroup }));
            list.AddRange(brList.Select(x => new BusinessDataTreeViewModel { Id = x.Id, CompanyId = x.CompanyId, Name = x.Name, ParentId = x.ParentId, BusinessDataTreeNodeType = BusinessDataTreeNodeTypeEnum.BusinessData }));
            list.ForEach(x => x.HasSubFolders = list.Any(y => y.ParentId == x.Id));
            list.Add(new BusinessDataTreeViewModel { Id = _repo.UserContext.CompanyId, Name = _repo.UserContext.CompanyName, ParentId = null, CompanyId = _repo.UserContext.CompanyId, BusinessDataTreeNodeType = BusinessDataTreeNodeTypeEnum.Root, HasSubFolders = list.Any() });

            //list.Add(new BusinessDataTreeViewModel { Id = companyId, Name = "Company Name", ParentId = null, CompanyId = companyId, BusinessDataTreeNodeType = BusinessDataTreeNodeTypeEnum.Root, HasSubFolders = list.Any() });
            return list;
        }


    }
}
