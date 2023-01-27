﻿using Synergy.App.DataModel;
using Synergy.App.ViewModel;
//using Syncfusion.EJ2.Diagrams;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IBusinessRuleNodeBusiness : IBusinessBase<BusinessRuleNodeViewModel, BusinessRuleNode>
    {
        //public Task CreateNode(Node node, string BussinessRuleId);
        //public Task CreateNode(BusinessRuleNodeViewModel model);
        //public Task EditNode(BusinessRuleNodeViewModel model);
        Task RemoveNodeAndItsChild(string ComponentId);
        //  public Task<List<DiagramNode>> GetNode(string BussinessRuleId);
        Task RemoveBusinessRuleAndItsNode(string ruleId);
        Task<List<dynamic>> CopyBusinessRuleNodes(List<dynamic> BRIds, List<BusinessRuleNodeViewModel> oldNodesList);
    }
}