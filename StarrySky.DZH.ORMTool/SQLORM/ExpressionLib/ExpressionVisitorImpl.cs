using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.SQLORM.ExpressionLib
{
    public class ExpressionVisitorImpl : ExpressionVisitor
    {
        public List<string> SelectColumnList = new List<string>();
        public List<string> WhereColumnList = new List<string>();

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
            SelectColumnList.Add(node.Member.Name);
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
            foreach (var item in node.Members)
            {
                SelectColumnList.Add(item.Name);
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
                foreach (var item in properties)
                {
                    SelectColumnList.Add(item.Name);
                }
            }
            return node;
        }

    }
}
