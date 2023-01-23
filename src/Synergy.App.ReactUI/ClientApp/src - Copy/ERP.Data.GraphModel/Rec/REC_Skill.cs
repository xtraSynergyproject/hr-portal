
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_Skill : NodeBase
    {
        public string Name { get; set; }
        public SkillTypeEnum SkillType { get; set; } 
    }
}
