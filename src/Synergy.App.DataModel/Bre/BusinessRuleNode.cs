using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using Syncfusion.EJ2.Diagrams;
using System.ComponentModel.DataAnnotations.Schema;
using Synergy.App.Common;

namespace Synergy.App.DataModel
{
    public class BusinessRuleNode : DataModelBase
    {
        public string Name { get; set; }
        public string BusinessRuleId { get; set; }
        public FlowShapes Type { get; set; }
        public bool IsStarter { get; set; }
        public BusinessRuleLogicTypeEnum? BusinessRuleLogicType { get; set; }
        public string Script { get; set; }
        public string OperationValue { get; set; }

    }
    //public class Node
    //{

    //    public string Id { get; set; }
    //    public double MaxHeight { get; set; }
    //    public double MaxWidth { get; set; }


    //    public double MinHeight { get; set; }


    //    public double MinWidth { get; set; }


    //    public double OffsetX { get; set; }


    //    public double OffsetY { get; set; }


    //    public DiagramPoint Pivot { get; set; }


    //    public List<DiagramPort> Ports { get; set; }


    //    public object PreviewSize { get; set; }


    //    public double RotateAngle { get; set; }


    //    public double RowIndex { get; set; }


    //    public double RowSpan { get; set; }


    //    public object Rows { get; set; }


    //    public DiagramShadow Shadow { get; set; }



    //    public DiagramShape Shape { get; set; }


    //    public NodeShapeStyle Style { get; set; }


    //    //public object SymbolInfo { get; set; }


    //    public DiagramDiagramTooltip Tooltip { get; set; }



    //    public VerticalAlignment VerticalAlignment { get; set; }


    //    public bool Visible { get; set; }


    //    public double Width { get; set; }


    //    public DiagramMargin Margin { get; set; }


    //    //public object LayoutInfo { get; set; }


    //    public bool IsExpanded { get; set; }



    //    //public object AddInfo { get; set; }


    //    public List<DiagramNodeAnnotation> Annotations { get; set; }


    //    public string BackgroundColor { get; set; }


    //    public string BorderColor { get; set; }


    //    public double BorderWidth { get; set; }


    //    public BranchTypes Branch { get; set; }


    //    public string[] Children { get; set; }


    //    public DiagramIconShape CollapseIcon { get; set; }


    //    public double ColumnIndex { get; set; }



    //   // public object Wrapper { get; set; }


    //    public double ColumnSpan { get; set; }



    //    public NodeConstraints Constraints { get; set; }


    //   // public object Container { get; set; }


    //   // public object Data { get; set; }


    //   // public object DragSize { get; set; }



    //    public bool ExcludeFromLayout { get; set; }



    //    public DiagramIconShape ExpandIcon { get; set; }


    //    public List<DiagramNodeFixedUserHandle> FixedUserHandles { get; set; }



    //    public FlipDirection Flip { get; set; }


    //    public double Height { get; set; }




    //    public HorizontalAlignment HorizontalAlignment { get; set; }


    //   // public object Columns { get; set; }


    //    public double ZIndex { get; set; }

    //}
    //public class DiagramShape
    //{
    //    public string type { get; set; }
    //    public FlowShapes shape { get; set; }
    //    public string data { get; set; }
    //}
    //public class NodeShapeStyle
    //{
    //    public string strokeColor { get; set; }
    //    public string fill { get; set; }

    //}
}
