using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class BusinessDiagramViewModel : TaskTemplateViewModel
    {
        public string diagramJson { get; set; }
        public IList<IdNameViewModel> nodeIds { get; set; }
        public string diagramTemplateId { get; set; }
    }

    public class BusinessDiagramNodeViewModel
    {
        public string ReferenceId { get; set; }
        public List<String> Predeccessor { get; set; }
        public string Id { get; set; }
        public List<String> ParentIdList
        {
            get
            {
                return ParentIds?.Split(',')?.ToList();
            }
        }
        public string ParentIds { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public TemplateTypeEnum TemplateType { get; set; }
        public NodeShapeEnum NodeShape { get; set; }
        public bool HasChild { get; set; }
    }
}
