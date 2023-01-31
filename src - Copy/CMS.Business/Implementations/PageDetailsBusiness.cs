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
   public class PageDetailsBusiness : BusinessBase<PageDetailsViewModel, PageDetails>, IPageDetailsBusiness
    {
        public PageDetailsBusiness(IRepositoryBase<PageDetailsViewModel, PageDetails> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
    }
}
