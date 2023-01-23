using Synergy.App.Common;
using System;
using System.Collections.Generic;

namespace Synergy.App.DataModel
{
    public class Style : DataModelBase
    {
        public string SourceId { get; set; }
        public string PageId { get; set; }
        public PageContentTypeEnum? SourceType{ get; set; }
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }
        public string Color { get; set; }
        public string FontSize { get; set; }
        public string MinWidth { get; set; }
       


        public string Width { get; set; }
        public string MaxWidth { get; set; }
     

        public string MinHeight { get; set; }
   
        public string Height { get; set; }
    
        public string MaxHeight { get; set; }
   


        public string PaddingDefault { get; set; }
    
        public string PaddingLeft { get; set; }
    
        public string PaddingTop { get; set; }
      
        public string PaddingRight { get; set; }
     
        public string PaddingBottom { get; set; }
     

        public string MarginDefault { get; set; }
      
        public string MarginLeft { get; set; }
        
        public string MarginTop { get; set; }
    
        public string MarginRight { get; set; }
 
        public string MarginBottom { get; set; }
        

        public string BorderWidthDefault { get; set; }
       
        public string BorderWidthLeft { get; set; }
     
        public string BorderWidthTop { get; set; }
    
        public string BorderWidthRight { get; set; }
 
        public string BorderWidthBottom { get; set; }
       


        public string BorderColorDefault { get; set; }
        public string BorderColorLeft { get; set; }
        public string BorderColorTop { get; set; }
        public string BorderColorRight { get; set; }
        public string BorderColorBottom { get; set; }


        public string BorderStyleDefault { get; set; }
        public string BorderStyleLeft { get; set; }
        public string BorderStyleTop { get; set; }
        public string BorderStyleRight { get; set; }
        public string BorderStyleBottom { get; set; }


        public string BorderRadiusDefault { get; set; }
        public string BorderRadiusLeft { get; set; }
        public string BorderRadiusTop { get; set; }
        public string BorderRadiusRight { get; set; }
        public string BorderRadiusBottom { get; set; }
    }

    public class StylePublished : Page
    {
        public string StyleId { get; set; }
        public string PublishedBy { get; set; }
        public string PublishedDate { get; set; }
        public int VersionNo { get; set; }
        public bool IsLatest { get; set; }
    }
}
