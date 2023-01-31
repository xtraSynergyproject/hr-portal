using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class DeletedRelViewModel
    {
        public string StartNodeName { get; set; }
        public string StartId { get; set; }
        public string RelType { get; set; }
        public string EndId { get; set; }
        public string Query
        {
            get
            {
                return "match(n: " + StartNodeName + "{ Id: " + StartId + "})-[r: " + RelType + "]->(m{ Id: " + EndId + "}) delete r";
            }
        }
    }
}


