using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class DocumentTypeBusiness : BusinessBase<DocumentTypeViewModel, DocumentType>, IDocumentTypeBusiness
    {
        public DocumentTypeBusiness(IRepositoryBase<DocumentTypeViewModel, DocumentType> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

    }
}
