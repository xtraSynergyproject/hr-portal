using AutoMapper;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class DocumentBusiness : BusinessBase<DocumentViewModel, Document>, IDocumentBusiness
    {
        public DocumentBusiness(IRepositoryBase<DocumentViewModel, Document> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
    }
}
