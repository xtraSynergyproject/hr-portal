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
   public class PageDetailsBusiness : BusinessBase<PageDetailsViewModel, PageDetails>, IPageDetailsBusiness
    {
        public PageDetailsBusiness(IRepositoryBase<PageDetailsViewModel, PageDetails> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
    }
}
