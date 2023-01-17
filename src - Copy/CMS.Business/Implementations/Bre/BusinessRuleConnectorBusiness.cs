using AutoMapper;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Syncfusion.EJ2.Diagrams;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class BusinessRuleConnectorBusiness : BusinessBase<BusinessRuleConnectorViewModel, BusinessRuleConnector>, IBusinessRuleConnectorBusiness
    {
        public BusinessRuleConnectorBusiness(IRepositoryBase<BusinessRuleConnectorViewModel, BusinessRuleConnector> repo, IMapper autoMapper) : base(repo, autoMapper)
        {
            _repo = repo;
            _autoMapper = autoMapper;
        }
        public async Task CreateConnector(DiagramConnector connector, string BussinessRuleId)
        {
            var model = _autoMapper.Map<BusinessRuleConnectorViewModel>(connector);
            model.BusinessRuleId = BussinessRuleId;
            await _repo.Create(model);
        }
        public async Task<List<DiagramConnector>> GetConnector(string BussinessRuleId)
        {
            var data = await GetList(x => x.BusinessRuleId == BussinessRuleId);
            var model = _autoMapper.Map<List<DiagramConnector>>(data);
            return model;
        }

    }
}
