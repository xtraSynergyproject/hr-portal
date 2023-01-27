using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Models
{
    public class ComplexHierarchicalDataDetails
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string[] ReportingPersons { get; set; }
        public string Border { get; set; }
        public string Shape { get; set; }

        public ComplexHierarchicalDataDetails(string name, string color, string[] reportingPersons, string border, string shape)
        {
            this.Name = name;
            this.Color = color;
            this.ReportingPersons = reportingPersons;
            this.Border = border;
            this.Shape = shape;
        }

        public static List<ComplexHierarchicalDataDetails> GetAllRecords()
        {
            List<ComplexHierarchicalDataDetails> multiParents = new List<ComplexHierarchicalDataDetails>();

            multiParents.Add(new ComplexHierarchicalDataDetails("node11", "#e7704c", null, "#c15433", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));

            multiParents.Add(new ComplexHierarchicalDataDetails("node12", "#efd46e", new string[] { "node114" }, "#d6b123", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
            multiParents.Add(new ComplexHierarchicalDataDetails("node13", "#58b087", new string[] { "node12" }, "#16955e", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
            multiParents.Add(new ComplexHierarchicalDataDetails("node14", "#58b087", new string[] { "node12" }, "#16955e", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
           
            multiParents.Add(new ComplexHierarchicalDataDetails("node1001", "#58b087", null, "#16955e", ""));
            
            multiParents.Add(new ComplexHierarchicalDataDetails("node15", "#58b087", new string[] { "node12" }, "#16955e", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
            multiParents.Add(new ComplexHierarchicalDataDetails("node116", "#58b087", new string[] { "node22", "node12" }, "#d3722e", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
            multiParents.Add(new ComplexHierarchicalDataDetails("node16", "#14ad85", new string[] { }, "", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));

            multiParents.Add(new ComplexHierarchicalDataDetails("node17", "#659be5", new string[] { "node13", "node14", "node15" }, "#3a6eb5", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
            multiParents.Add(new ComplexHierarchicalDataDetails("node170", "#659be5", new string[] { "node13", "node14", "node15" }, "#3a6eb5", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
            multiParents.Add(new ComplexHierarchicalDataDetails("node18", "#14ad85", new string[] { }, "", ""));
            multiParents.Add(new ComplexHierarchicalDataDetails("node19", "#8dbe6c", new string[] { "node16", "node17", "node18" }, "#489911", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));

            multiParents.Add(new ComplexHierarchicalDataDetails("node110", "#8dbe6c", new string[] { "node16", "node17", "node18" }, "#489911", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
            multiParents.Add(new ComplexHierarchicalDataDetails("node111", "#8dbe6c", new string[] { "node16", "node17", "node18", "node116" }, "#489911", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));

            multiParents.Add(new ComplexHierarchicalDataDetails("node21", "#e7704c", null, "#c15433", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));

            multiParents.Add(new ComplexHierarchicalDataDetails("node22", "#efd46e", new string[] { "node114" }, "#d6b123", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
            multiParents.Add(new ComplexHierarchicalDataDetails("node23", "#58b087", new string[] { "node22" }, "#16955e", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
            multiParents.Add(new ComplexHierarchicalDataDetails("node24", "#58b087", new string[] { "node22" }, "#16955e", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));
            multiParents.Add(new ComplexHierarchicalDataDetails("node25", "#58b087", new string[] { "node22" }, "#16955e", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));

            multiParents.Add(new ComplexHierarchicalDataDetails("node26", "#14ad85", new string[] { }, "#14ad85", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));

            multiParents.Add(new ComplexHierarchicalDataDetails("node27", "#659be5", new string[] { "node23", "node24", "node25" }, "#3a6eb5", "{ type = 'HTML', content= ' < div style = 'background:#6BA5D7;height:100%;width:100%;' >< button type = 'button' style = 'width:100px' > Button </ button ></ div > ' }"));

            multiParents.Add(new ComplexHierarchicalDataDetails("node28", "#14ad85", new string[] { }, "",""));

            multiParents.Add(new ComplexHierarchicalDataDetails("node29", "#8dbe6c", new string[] { "node26", "node27", "node28", "node116" }, "#489911",""));
            multiParents.Add(new ComplexHierarchicalDataDetails("node210", "#8dbe6c", new string[] { "node26", "node27", "node28" }, "#489911",""));
            multiParents.Add(new ComplexHierarchicalDataDetails("node211", "#8dbe6c", new string[] { "node26", "node27", "node28" }, "#489911",""));

            multiParents.Add(new ComplexHierarchicalDataDetails("node31", "#e7704c", null, "#c15433",""));

            multiParents.Add(new ComplexHierarchicalDataDetails("node114", "#f3904a", new string[] { "node11", "node21", "node31" }, "#d3722e",""));

            return multiParents;
        }
    }
}
