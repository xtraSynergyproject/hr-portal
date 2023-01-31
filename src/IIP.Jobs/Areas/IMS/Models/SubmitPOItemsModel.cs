using Synergy.App.ViewModel;
using System.Collections.Generic;

namespace Synergy.App.Api.Areas.IMS.Models
{
    public class SubmitPOItemsModel
    {
       public List<POItemsViewModel> itemList { get; set; }
       public string poId { get; set; }
       public string userId { get; set; }

    }
}
