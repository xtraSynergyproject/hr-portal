/// <copyright>
/// Copyright (c) 2013, Vargas Isaias Patag
/// All rights reserved.
/// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
/// Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
/// Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
/// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
/// </copyright>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common.Extensions;
using ProtoBuf;

namespace Synergy.App.Common.Rules
{
    [ProtoContract, ProtoInclude(6, typeof(MDASRule)), ProtoInclude(7, typeof(EqualityRule)), ProtoInclude(8, typeof(BitWiseRule))]
    [Serializable]
    public abstract class RuleBase
    {
        private readonly static ReadOnlyDictionary<char, Func<Expression, Expression, Expression>> operations =
            new ReadOnlyDictionary<char,Func<Expression,Expression,Expression>>(new Dictionary<char, Func<Expression, Expression, Expression>>
                {
                    { '+', (current, next) => 
                        {
                            if(current is Expression<Func<decimal>>)
                            {
                                current = current.LambdaExpressionToDecimalBinaryExpression();
                            }
                            if(next is Expression<Func<decimal>>)
                            {
                                next = next.LambdaExpressionToDecimalBinaryExpression();
                            }

                            return Expression.Add(current, next);
                        }},
                    { '-', (current, next) =>
                        {
                            if(current is Expression<Func<decimal>>)
                            {
                                current = current.LambdaExpressionToDecimalBinaryExpression();
                            }
                            if(next is Expression<Func<decimal>>)
                            {
                                next = next.LambdaExpressionToDecimalBinaryExpression();
                            }

                            return Expression.Subtract(current, next);
                        } },
                    { '*', (current, next) => 
                        {
                            if(current is Expression<Func<decimal>>)
                            {
                                current = current.LambdaExpressionToDecimalBinaryExpression();
                            }
                            if(next is Expression<Func<decimal>>)
                            {
                                next = next.LambdaExpressionToDecimalBinaryExpression();
                            }

                            return Expression.Multiply(current, next);
                        }},
                    { '/', (current, next) => 
                        {
                            if(current is Expression<Func<decimal>>)
                            {
                                current = current.LambdaExpressionToDecimalBinaryExpression();
                            }
                            if(next is Expression<Func<decimal>>)
                            {
                                next = next.LambdaExpressionToDecimalBinaryExpression();
                            }

                            return Expression.Divide(current, next);
                        }}
                });

        protected RuleBase()
        {

        }

        public RuleBase(string left, string op, string right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
        public RuleBase(RuleBase primaryRule, string op, string right)
        {
            PrimaryRule = primaryRule;
            Operator = op;
            Right = right;
        }
        public RuleBase(RuleBase primaryRule, string op, RuleBase secondaryRule)
        {
            PrimaryRule = primaryRule;
            Operator = op;
            SecondaryRule = secondaryRule;
        }

        [ProtoMember(1)]
        protected string Left { get; private set; }
        [ProtoMember(2)]
        protected string Operator { get; private set; }
        [ProtoMember(3)]
        protected string Right { get; private set; }
        [ProtoMember(4)]
        protected RuleBase PrimaryRule { get; private set; }
        [ProtoMember(5)]
        protected RuleBase SecondaryRule { get; private set; }


        public Func<T, bool> CompileRule<T>()
        {
            var paramUser = Expression.Parameter(typeof(T));
            Expression expr = RuleBase.buildExpr<T>(this, paramUser);
            // build a lambda function T->bool and compile it
            var compiledRule = Expression.Lambda<Func<T, bool>>(expr, paramUser);
            return compiledRule.Compile();
        }

        public Func<T, bool> CompileRule<T>(out string compiledRuleTextualRepresentation)
        {
            var paramUser = Expression.Parameter(typeof(T));
            Expression expr = RuleBase.buildExpr<T>(this, paramUser);
            // build a lambda function T->bool and compile it
            var compiledRule = Expression.Lambda<Func<T, bool>>(expr, paramUser);
            compiledRuleTextualRepresentation = compiledRule.Body.ToString();
            return compiledRule.Compile();
        }

        public static Func<decimal> CompileDecimalEvaluator(string expression)
        {
            if (!expression.Any(x => operations.Keys.Contains(x)))
            {
                throw new ArgumentException("String should contain +, -, *, or / operators");
            }

            if (expression.Contains('(') || expression.Contains(')'))
            {
                throw new ArgumentException("String cannot contain parentheses");
            }

            var param = Expression.Parameter(typeof(decimal));
            var expr = (Expression<Func<decimal>>)RuleBase.evaluate(expression, param);

            return expr.Compile();
        }

        public static Func<T, decimal> CompileDecimalEvaluatorWithParameter<T>(string expression)
        {
            if (!expression.Any(x => operations.Keys.Contains(x)))
            {
                throw new ArgumentException("String should contain +, -, *, or / operators");
            }

            if (expression.Contains('(') || expression.Contains(')'))
            {
                throw new ArgumentException("String cannot contain parentheses");
            }

            var param = Expression.Parameter(typeof(T));
            var expr = (Expression<Func<decimal>>)RuleBase.evaluate(expression, param);

            return Expression.Lambda<Func<T, decimal>>(expr.Body, param).Compile();
        }

        private static Expression evaluate(string expression, ParameterExpression paramExpression)
        {
            foreach (var operation in operations)
            {
                if (expression.Contains(operation.Key))
                {
                    var parts = expression.Split(operation.Key);
                    Expression result;
                    try
                    {
                        result = RuleBase.evaluate(parts[0], paramExpression);
                    }
                    catch
                    {
                        result = MemberExpression.Property(paramExpression, parts[0]);
                    }

                    try
                    {
                        result = parts.Skip(1).Aggregate(result,
                                                         (current, next) =>
                                                         operation.Value(current, RuleBase.evaluate(next, paramExpression)));
                    }
                    catch
                    {
                        result = parts.Skip(1).Aggregate(result,
                                                         (current, next) =>
                                                         operation.Value(current, RuleBase.evaluate(next, paramExpression)));
                    }

                    var lambda = Expression.Lambda<Func<decimal>>(result);
                    return lambda;
                }
            }

            Expression expressionForPropsOrConstant;
            try
            {
                expressionForPropsOrConstant = Expression.Constant(Convert.ChangeType(expression, typeof(decimal)));
            }
            catch
            {
                expressionForPropsOrConstant = MemberExpression.Property(paramExpression, expression);
            }

            return expressionForPropsOrConstant;
        }

        private static Expression buildExpr<T>(RuleBase r, ParameterExpression param)
        {
            Expression left;
            Expression right;
            Type tProp;
            if (r.PrimaryRule != null)
            {
                left = RuleBase.buildExpr<T>(r.PrimaryRule, param);
                tProp = left.Type;
            }
            else if (r.Left.Any(x => operations.Keys.Contains(x)))
            {
                left = RuleBase.evaluate(r.Left, param).LambdaExpressionToDecimalBinaryExpression();
                tProp = typeof(decimal);
            }
            else
            {
                left = MemberExpression.Property(param, r.Left);
                tProp = typeof(T).GetProperty(r.Left).PropertyType;
            }
            ExpressionType tBinary;
            // make sure to use a know .NET operator or have a dictionary for it
            if (ExpressionType.TryParse(r.Operator, out tBinary))
            {
                if (r.SecondaryRule != null)
                {
                    right = RuleBase.buildExpr<T>(r.SecondaryRule, param);
                }
                else if (r.Right.Any(x => operations.Keys.Contains(x)))
                {
                    right = RuleBase.evaluate(r.Right, param).LambdaExpressionToDecimalBinaryExpression();
                    tProp = typeof(decimal);
                }
                else
                {
                    try
                    {
                        right = Expression.Constant(Convert.ChangeType(r.Right, tProp));
                    }
                    catch (FormatException)
                    {
                        right = MemberExpression.Property(param, r.Right);
                    }
                }
                // use a binary operation, e.g. 'Equal' -> 'u.Age == 15'
                return Expression.MakeBinary(tBinary, left, right);
            }

            // this is almost impossible to reach
            throw new ArgumentException("Operator used is not a valid .NET operator");
        }
    }
}
