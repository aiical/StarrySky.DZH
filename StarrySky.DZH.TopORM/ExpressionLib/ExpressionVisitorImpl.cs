using Dapper;
using StarrySky.DZH.TopORM.Common;
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.ExpressionLib
{
    public class ExpressionVisitorImpl : ExpressionVisitor
    {
        private ExpressionVisitorImpl() { }

        public ExpressionVisitorImpl(TranslatorEnum etranslator)
        {
            _eTranslator = etranslator;
        }
        private TranslatorEnum _eTranslator = TranslatorEnum.Default;
        public List<string> SelectColumnList = new List<string>();
        public List<string> UpdateColumnList = new List<string>();
        public List<string> WhereColumnList = new List<string>();
        //声明动态参数
        public DynamicParameters Parameters = new DynamicParameters();
        public int AutoIncrementId = 0;

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.Expression`1。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 类型参数:
        //   T:
        //     该委托的类型。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Visit(node.Body);
        }

        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.BinaryExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Left is BinaryExpression)
            {
                Expression left = this.Visit(node.Left);
                Expression right = this.Visit(node.Right);
            }
            else
            {
                string name = (node.Left as MemberExpression).Member.Name;
                object value = string.Empty;
                if (node.Right is ConstantExpression)
                {
                    value = (node.Right as ConstantExpression).Value.ToString();
                }
                else if (node.Right is MethodCallExpression)
                {
                    value = Expression.Lambda(node.Right as MethodCallExpression).Compile().DynamicInvoke();//Compile有性能消耗
                }
                else if (node.Right is MemberExpression)
                {
                    value = Expression.Lambda(node.Right as MemberExpression).Compile().DynamicInvoke();
                }

                string paramsName = $"{name}{AutoIncrementId.ToString()}";
                if (_eTranslator == TranslatorEnum.Update)
                {
                    UpdateColumnList.Add($"{name}{node.NodeType.ToSqlOperator()}@{paramsName}");
                }
                else if (_eTranslator == TranslatorEnum.Where)
                {
                    WhereColumnList.Add($"{name}{node.NodeType.ToSqlOperator()}@{paramsName}");
                }
                Parameters.Add($"{paramsName}", value);
                AutoIncrementId++;
            }
            return node;
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.BlockExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitBlock(BlockExpression node)
        {
            return node;
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.ConditionalExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            return node;
        }

        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.MemberExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitMember(MemberExpression node)
        {
            if (_eTranslator == TranslatorEnum.Select)
            {
                SelectColumnList.Add(node.Member.Name);
            }
            return node;
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.MemberInitExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            if (node.Bindings.Count <= 0)
            {
                return node;
            }
            foreach (var item in node.Bindings)
            {
                string paramsName = $"{item.Member.Name}{AutoIncrementId.ToString()}";
                if (_eTranslator == TranslatorEnum.Update)
                {
                    UpdateColumnList.Add($"{item.Member.Name}=@{paramsName}");
                }
                else if (_eTranslator == TranslatorEnum.Where)
                {
                    WhereColumnList.Add($"{item.Member.Name}=@{paramsName}");
                }
                Parameters.Add($"{paramsName}", item.ToString().Split('=')[1].Trim());
                AutoIncrementId++;
            }
            return node;
        }

        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.NewExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitNew(NewExpression node)
        {
            if (node.Members != null)
            {
                if (_eTranslator == TranslatorEnum.Select)
                {
                    foreach (var item in node.Members)
                    {
                        SelectColumnList.Add(item.Name);
                    }
                }
            }
            return node;
        }

        //
        // 摘要:
        //     访问 System.Linq.Expressions.ParameterExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitParameter(ParameterExpression node)
        {
            var properties = node.Type.GetProperties();
            if (!properties.IsNullOrEmptyCollection())
            {
                if (_eTranslator == TranslatorEnum.Select)
                {
                    foreach (var item in properties)
                    {
                        SelectColumnList.Add(item.Name);
                    }
                }
            }
            return node;
        }

    }
}
