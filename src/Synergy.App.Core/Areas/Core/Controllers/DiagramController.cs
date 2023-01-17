using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
//using Syncfusion.EJ2.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Controllers.Diagram
{
    public class DiagramController : Controller
    {
        //public IActionResult ComplexHierarchicalLayout()
        //{
        //    List<Syncfusion.EJ2.Diagrams.DiagramNode> flowShapes = new List<Syncfusion.EJ2.Diagrams.DiagramNode>();
        //    flowShapes.Add(new DiagramNode() { Id = "Terminator", Shape = new { type = "Flow", shape = "Terminator" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Process", Shape = new { type = "Flow", shape = "Process" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Decision", Shape = new { type = "Flow", shape = "Decision" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Document", Shape = new { type = "Flow", shape = "Document" } });
        //    flowShapes.Add(new DiagramNode() { Id = "PreDefinedProcess", Shape = new { type = "Flow", shape = "PreDefinedProcess" } });
        //    flowShapes.Add(new DiagramNode() { Id = "PaperTap", Shape = new { type = "Flow", shape = "PaperTap" } });
        //    flowShapes.Add(new DiagramNode() { Id = "DirectData", Shape = new { type = "Flow", shape = "DirectData" } });
        //    flowShapes.Add(new DiagramNode() { Id = "SequentialData", Shape = new { type = "Flow", shape = "SequentialData" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Sort", Shape = new { type = "Flow", shape = "Sort" } });
        //    flowShapes.Add(new DiagramNode() { Id = "MultiDocument", Shape = new { type = "Flow", shape = "MultiDocument" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Collate", Shape = new { type = "Flow", shape = "Collate" } });
        //    flowShapes.Add(new DiagramNode() { Id = "SummingJunction", Shape = new { type = "Flow", shape = "SummingJunction" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Or", Shape = new { type = "Flow", shape = "Or" } });
        //    flowShapes.Add(new DiagramNode() { Id = "InternalStorage", Shape = new { type = "Flow", shape = "InternalStorage" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Extract", Shape = new { type = "Flow", shape = "Extract" } });
        //    flowShapes.Add(new DiagramNode() { Id = "ManualOperation", Shape = new { type = "Flow", shape = "ManualOperation" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Merge", Shape = new { type = "Flow", shape = "Merge" } });
        //    flowShapes.Add(new DiagramNode() { Id = "OffPageReference", Shape = new { type = "Flow", shape = "OffPageReference" } });
        //    flowShapes.Add(new DiagramNode() { Id = "SequentialAccessStorage", Shape = new { type = "Flow", shape = "SequentialAccessStorage" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Annotation", Shape = new { type = "Flow", shape = "Annotation" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Annotation2", Shape = new { type = "Flow", shape = "Annotation2" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Data", Shape = new { type = "Flow", shape = "Data" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Card", Shape = new { type = "Flow", shape = "Card" } });
        //    flowShapes.Add(new DiagramNode() { Id = "Delay", Shape = new { type = "Flow", shape = "Delay" } });


        //    List<DiagramConnector> SymbolPaletteConnectors = new List<DiagramConnector>();
        //    SymbolPaletteConnectors.Add(new DiagramConnector()
        //    {
        //        Id = "Link1",
        //        Type = Segments.Orthogonal,
        //        SourcePoint = new DiagramPoint() { X = 0, Y = 0 },
        //        TargetPoint = new DiagramPoint() { X = 40, Y = 40 },
        //        TargetDecorator = new ConnectorTargetDecoratorConnectors() { Shape = DecoratorShapes.Arrow, Style = new DiagramShapeStyle() { StrokeColor = "#757575", Fill = "#757575" } },
        //        Style = new DiagramStrokeStyle() { StrokeWidth = 2, StrokeColor = "#757575" }
        //    });
        //    SymbolPaletteConnectors.Add(new DiagramConnector()
        //    {
        //        Id = "Link2",
        //        Type = Segments.Orthogonal,
        //        SourcePoint = new DiagramPoint() { X = 0, Y = 0 },
        //        TargetPoint = new DiagramPoint() { X = 40, Y = 40 },
        //        TargetDecorator = new ConnectorTargetDecoratorConnectors() { Shape = DecoratorShapes.None },
        //        Style = new DiagramStrokeStyle() { StrokeWidth = 2, StrokeColor = "#757575" }
        //    });
        //    SymbolPaletteConnectors.Add(new DiagramConnector()
        //    {
        //        Id = "Link3",
        //        Type = Segments.Straight,
        //        SourcePoint = new DiagramPoint() { X = 0, Y = 0 },
        //        TargetPoint = new DiagramPoint() { X = 40, Y = 40 },
        //        TargetDecorator = new ConnectorTargetDecoratorConnectors() { Shape = DecoratorShapes.Arrow, Style = new DiagramShapeStyle() { StrokeColor = "#757575", Fill = "#757575" } },
        //        Style = new DiagramStrokeStyle() { StrokeWidth = 2, StrokeColor = "#757575" }
        //    });
        //    SymbolPaletteConnectors.Add(new DiagramConnector()
        //    {
        //        Id = "Link4",
        //        Type = Segments.Straight,
        //        SourcePoint = new DiagramPoint() { X = 0, Y = 0 },
        //        TargetPoint = new DiagramPoint() { X = 40, Y = 40 },
        //        TargetDecorator = new ConnectorTargetDecoratorConnectors() { Shape = DecoratorShapes.None },
        //        Style = new DiagramStrokeStyle() { StrokeWidth = 2, StrokeColor = "#757575" }
        //    });
        //    SymbolPaletteConnectors.Add(new DiagramConnector()
        //    {
        //        Id = "Link5",
        //        Type = Segments.Bezier,
        //        SourcePoint = new DiagramPoint() { X = 0, Y = 0 },
        //        TargetPoint = new DiagramPoint() { X = 40, Y = 40 },
        //        TargetDecorator = new ConnectorTargetDecoratorConnectors() { Shape = DecoratorShapes.None },
        //        Style = new DiagramStrokeStyle() { StrokeWidth = 2, StrokeColor = "#757575" }
        //    });

        //    List<SymbolPalettePalette> Palette = new List<SymbolPalettePalette>();
        //    Palette.Add(new SymbolPalettePalette() { Id = "flow", Expanded = true, Symbols = flowShapes, IconCss = "shapes", Title = "Flow Shapes" });
        //    Palette.Add(new SymbolPalettePalette() { Id = "connectors", Expanded = true, Symbols = SymbolPaletteConnectors, IconCss = "shapes", Title = "Connectors" });

        //    ViewBag.Palette = Palette;
        //    ViewBag.Nodes = ComplexHierarchicalDataDetails.GetAllRecords();
        //    ViewBag.getNodeDefaults = "nodeDefaults";
        //    ViewBag.getConnectorDefaults = "connectorDefaults";
        //    DiagramMargin margin = new DiagramMargin() { Left = 10, Top = 50 };
        //    ViewBag.marginValue = margin;
        //    return View();
        //}
    }
}
