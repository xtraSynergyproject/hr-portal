using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class SynergyIdNameViewModel
    {
        public long Id { get; set; }
        public long? RootId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

    }
}
