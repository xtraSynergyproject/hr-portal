﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.ComponentModel;

namespace ERP.UI.ViewModel
{
    public class PositionHierarchyHistoryViewModel  
    {

        public int? HierarchyNameId { get; set; }
        public int? PositionId { get; set; }
        public GridSelectOption SelectOption { get; set; }
        public bool IsSuccess { get; set; }

    }
}
