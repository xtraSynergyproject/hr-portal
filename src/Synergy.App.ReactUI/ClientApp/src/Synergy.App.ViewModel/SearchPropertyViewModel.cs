using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;
using Synergy.App.DataModel;

namespace Synergy.App.ViewModel
{
    public class SearchPropertyViewModel
    {
        public string CityId { get; set; }
        public string Id { get; set; }
        public string PropertyId { get; set; }

        public string OldPropertyId { get; set; }
        public int Mobile { get; set; }
        public string WardNoId { get; set; }
        public string HouseNo { get; set; }

        public string Street{ get; set; }
        public string PostalCode { get; set; }

        public string Email { get; set; }
        public DataActionEnum DataAction { get; set; }
        public string OwnerName { get; set; }

    }
}
