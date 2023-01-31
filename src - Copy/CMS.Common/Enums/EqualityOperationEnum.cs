
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Common
{
    public enum EqualityOperationEnum
    {
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
    }
    public enum LogicalEnum
    {
        And,
        Or,
    }
    public enum BusinessRuleTypeEnum
    {
        Logical,
        Operational,
    }

    public enum TrueOrFalseEnum
    {
        False = 0,
        True = 1

    }
    public enum LogicalOperationTypeEnum
    {
        And = 1,
        Or = 2,
    }
    public enum BusinessRuleSourceEnum
    {
        MasterData,
        BusinessRule,
    }

}
