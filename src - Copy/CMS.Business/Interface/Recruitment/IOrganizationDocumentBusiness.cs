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
    public interface IOrganizationDocumentBusiness : IBusinessBase<OrganizationDocumentsViewModel, OrganizationDocument>
    {
        Task<List<OrganizationDocumentsViewModel>> GetOrgListById(string OrgId);
    }
}
