using System;

namespace CMS.Data.Model
{
    public class SettingsBase : DataModelBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string IconCss { get; set; }
        public string ParentId { get; set; }
    }
}
