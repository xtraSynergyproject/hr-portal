using AutoMapper;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class DocumentBusiness : BusinessBase<DocumentViewModel, Document>, IDocumentBusiness
    {
        public DocumentBusiness(IRepositoryBase<DocumentViewModel, Document> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
    }
}
