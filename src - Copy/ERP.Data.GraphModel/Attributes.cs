using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
  
    public class NotMapped : Attribute
    {
        
    }
    //public class CompanyNotRequired : Attribute
    //{

    //}
    public class PrimaryKey : Attribute
    {

    }
    public class CreateOnly : Attribute
    {

    }
}
