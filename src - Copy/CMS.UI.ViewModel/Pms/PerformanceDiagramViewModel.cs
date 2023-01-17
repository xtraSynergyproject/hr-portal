using CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class PerformanceDiagramViewModel
    {
        public string ReferenceId { get; set; }
        public string Id { get; set; }
        //public List<String> ParentIdList
        //{
        //    get
        //    {
        //        return ParentIds?.Split(',')?.ToList();
        //    }
        //}
        public string ParentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public TemplateTypeEnum TemplateType { get; set; }
        public NodeShapeEnum NodeShape { get; set; }
        public bool HasChild { get; set; }
    }
}
