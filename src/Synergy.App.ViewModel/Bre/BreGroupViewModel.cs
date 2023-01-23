﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class BreGroupViewModel : BusinessRuleGroup
    {
        public BusinessRuleTreeNodeTypeEnum BusinessRuleTreeNodeType { get; set; }
        public DataActionEnum DataAction { get; set; }
    }
}