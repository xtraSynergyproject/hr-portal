using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class SynergyIdNameViewModel
    {
        public long Id { get; set; }
        public long? RootId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

    }
}
