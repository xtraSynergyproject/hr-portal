using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;

namespace ERP.Data.Model
{
    public partial class vw_Actions : AdminDatedBase
    {       
        public virtual string Name  {  get; set; }
    
        public virtual string Controller  {  get; set; }
    
        public virtual string Action  {  get; set; }
    
        public virtual string Control  {  get; set; }
    
        public virtual string Description  {  get; set; }
    
        public virtual Nullable<int> SequenceNo  {  get; set; }
        public virtual int BlockId  {  get; set; }
    
        public virtual string BlockName  {  get; set; }
    
        public virtual int TabId  {  get; set; }
    
        public virtual string TabName  {  get; set; }
    
        public virtual int ScreenId  {  get; set; }
    
        public virtual string ScreenName  {  get; set; }
    
        public virtual int SubModuleId  {  get; set; }
    
        public virtual string SubModuleName  {  get; set; }
    
        public virtual int ModuleId  {  get; set; }
    
        public virtual string ModuleName  {  get; set; }
    }
}
