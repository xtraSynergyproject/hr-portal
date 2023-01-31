using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class AgencyViewModel : Agency
    {
       public string CountryName { get; set; }
    }
}
