using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web
{
    public class ChartViewModel
    {

        public string Id
        {
            get;
            set;
        }
        public string ParentId
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string NodeType
        {
            get;
            set;
        }
        public int DirectChildCount
        {
            get;
            set;
        }
        public int Level
        {
            get;
            set;
        }
        public string CssClass
        {
            get;
            set;
        }

    }
}
