using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class DashboardItemMasterViewModel : NoteTemplateViewModel
    {            
        public string chartTypeId { get; set; }        
        public string chartMetadata { get; set; }        
        public string boilerplateCode { get; set; }
        public string measuresField { get; set; }
        public string dimensionsField { get; set; }
        public string segmentsField { get; set; }
        public string filterField { get; set; }
        public string timeDimensionsField { get; set; }
        public string[] measuresArray { get; set; }
        public string[] dimensionsArray { get; set; }
        public string[] segmentsArray { get; set; }        
        public List<MeasuresViewModel> measures { get; set; }
        public List<DimensionsViewModel> dimensions { get; set; }
        public string dimensionsJson { get; set; }
        public List<SegmentsViewModel> segments { get; set; }
        public string Layout { get; set; }
        public string ChartKey { get; set; }
        public string height { get; set; }
        public string width { get; set; }        
        public string onChartClickFunction { get; set; }
        public string mapUrl { get; set; }
        public string mapLayer { get; set; }
        public string Help { get; set; }
        public string ThemeMode { get; set; }
        public string Palette { get; set; }
        public string MonocromeColor { get; set; }
        public bool DynamicMetadata { get; set; }
        public string Xaxis { get; set; }
        public string Yaxis { get; set; }
        public string Count { get; set; }
        public bool isLibrary { get; set; }
    }    

    public class DashboardItemFilterViewModel
    {        
        public string FilterField { get; set; }
        public string FilterOperator { get; set; }
        public string FilterText { get; set; }
        public string DefaultValue { get; set; }
    }
    public class DashboardItemTimeDimensionViewModel
    {
        public string TimeDimensionField { get; set; }
        public string RangeType { get; set; }
        public string TimeParams { get; set; }
        public string DefaultValue { get; set; }
        public string RangeBy { get; set; }
    }
    public class CubeJsViewModel
    {
        public string name { get; set; }
        public string title { get; set; }
        public string sql { get; set; }
        public string datasource { get; set; }
        public List<MeasuresViewModel> measures { get; set; }
        public List<DimensionsViewModel> dimensions { get; set; }
        public List<SegmentsViewModel> segments { get; set; }
    }
    public class MeasuresViewModel
    {
        public string name { get; set; }
        public string title { get; set; }
        public string dataType { get; set; }
    }
    public class DimensionsViewModel
    {
        public string name { get; set; }
        public string title { get; set; }
        public string dataType { get; set; }
    }
    public class SegmentsViewModel
    {
        public string name { get; set; }
        public string title { get; set; }

    }
    public class MapLayerItemViewModel : NoteTemplateViewModel
    {
        public string MapUrl { get; set; }
        public string MapLayer { get; set; }
        public string MapTransparency { get; set; }
        public string MapFormat { get; set; }
        public string MapOpacity { get; set; }
        public bool IsBaseMap { get; set; }
    }
}
