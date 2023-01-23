using CMS.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Common.Rules
{
    public static class TestRule
    {
        public static bool ArithmeticConditionalRule(int emp,string val)
        {
            Pricing p = new Pricing()
            {
                Employee = emp
            };


            var r = new EqualityRule("Employee", EqualityOperationEnum.GreaterThan, val);
            string ruleText;
            var compiledRule = r.CompileRule<Pricing>(out ruleText);
            var result = compiledRule(p);
            //Assert.IsTrue(result);
            return result;
        }
    }
    public class Pricing
    {
        public decimal Cost { get; set; }
        public decimal Employee { get; set; }
        public string Name { get; set; }
    }
}
