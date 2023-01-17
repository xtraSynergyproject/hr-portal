using AutoMapper;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
//using Syncfusion.EJ2.Diagrams;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class BusinessRuleConnectorBusiness : BusinessBase<BusinessRuleConnectorViewModel, BusinessRuleConnector>, IBusinessRuleConnectorBusiness
    {
        public BusinessRuleConnectorBusiness(IRepositoryBase<BusinessRuleConnectorViewModel, BusinessRuleConnector> repo, IMapper autoMapper) : base(repo, autoMapper)
        {
            _repo = repo;
            _autoMapper = autoMapper;
        }
        //public async Task CreateConnector(DiagramConnector connector, string BussinessRuleId)
        //{
        //    var model = _autoMapper.Map<BusinessRuleConnectorViewModel>(connector);
        //    model.BusinessRuleId = BussinessRuleId;
        //    await _repo.Create(model);
        //}
        //public async Task<List<DiagramConnector>> GetConnector(string BussinessRuleId)
        //{
        //    var data = await GetList(x => x.BusinessRuleId == BussinessRuleId);
        //    var model = _autoMapper.Map<List<DiagramConnector>>(data);
        //    return model;
        //}

        public async Task<bool> CopyBusinessRuleConnector(List<dynamic> BRIds, List<dynamic> nodeIds, List<BusinessRuleConnectorViewModel> oldList)
        {
            foreach (var item in oldList)
            {
                var BRIdx = BRIds.FindIndex(x => x.OldId == item.BusinessRuleId);
                string newBRId = BRIds[BRIdx].NewId.ToString();
                var srcIdx = nodeIds.FindIndex(x => x.OldId == item.SourceId);
                string newSrcId = nodeIds[srcIdx].NewId.ToString();
                var targetIdx = nodeIds.FindIndex(x => x.OldId == item.TargetId);
                string newTargetId = nodeIds[targetIdx].NewId.ToString();

                var model = _autoMapper.Map<BusinessRuleConnectorViewModel>(item);
                model.Id = null;
                model.BusinessRuleId = newBRId;
                model.SourceId = newSrcId;
                model.TargetId = newTargetId;

                var res = await base.Create(model);

                if (!res.IsSuccess)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
