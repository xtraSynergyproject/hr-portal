using AutoMapper;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
  public  class OrganizationDocumentBusiness : BusinessBase<OrganizationDocumentsViewModel, OrganizationDocument>, IOrganizationDocumentBusiness
    {
        private readonly IRepositoryQueryBase<OrganizationDocumentsViewModel> _queryOrgDocument;
        public OrganizationDocumentBusiness(IRepositoryBase<OrganizationDocumentsViewModel, OrganizationDocument> repo
            , IMapper autoMapper,
            IRepositoryQueryBase<OrganizationDocumentsViewModel> queryOrgDocument) : base(repo, autoMapper)
        {

            _queryOrgDocument=queryOrgDocument;

        }



        public async Task<List<OrganizationDocumentsViewModel>> GetOrgListById(string OrgId)
        {
            var query = $@"SELECT O.""Id"", ""OrganizationId"", ""AttachmentId"", ""Version"",F.""FileName""

    FROM rec.""OrganizationDocument"" O left join  public.""File"" F on  F.""Id""=O.""AttachmentId""
	
	where ""OrganizationId""='{OrgId}' order by O.""Version"" desc";
            var result = await _queryOrgDocument.ExecuteQueryList<OrganizationDocumentsViewModel>(query, null);
            return result;
        }

    }
}
