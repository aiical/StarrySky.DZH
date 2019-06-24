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

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
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
            return base.VisitBinary(node);
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
            return base.VisitBlock(node);
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
            return base.VisitConditional(node);
        }
        //
        // 摘要:
        //     访问 System.Linq.Expressions.ConstantExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitConstant(ConstantExpression node)
        {
            return base.VisitConstant(node);
        }
        //
        // 摘要:
        //     访问 System.Linq.Expressions.DebugInfoExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            return base.VisitDebugInfo(node);
        }
        //
        // 摘要:
        //     访问 System.Linq.Expressions.DefaultExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitDefault(DefaultExpression node)
        {
            return base.VisitDefault(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.DynamicExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitDynamic(DynamicExpression node)
        {
            return base.VisitDynamic(node);
        }
        //
        // 摘要:
        //     访问扩展的表达式的子级。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitExtension(Expression node)
        {
            return base.VisitExtension(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.GotoExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitGoto(GotoExpression node)
        {
            return base.VisitGoto(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.IndexExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitIndex(IndexExpression node)
        {
            return base.VisitIndex(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.InvocationExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            return base.VisitInvocation(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.LabelExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitLabel(LabelExpression node)
        {
            return base.VisitLabel(node);
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
        //     访问的子级 System.Linq.Expressions.ListInitExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitListInit(ListInitExpression node)
        {
            return base.VisitListInit(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.LoopExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitLoop(LoopExpression node)
        {
            return base.VisitLoop(node);
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
            return Visit(node);
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
            return base.VisitMemberInit(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.MethodCallExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return base.VisitMethodCall(node);
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
            return base.VisitNew(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.NewArrayExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            return base.VisitNewArray(node);
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
            return base.VisitParameter(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.RuntimeVariablesExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            return base.VisitRuntimeVariables(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.SwitchExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitSwitch(SwitchExpression node)
        {
            return base.VisitSwitch(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.TryExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitTry(TryExpression node)
        {
            return base.VisitTry(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.TypeBinaryExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            return base.VisitTypeBinary(node);
        }
        //
        // 摘要:
        //     访问的子级 System.Linq.Expressions.UnaryExpression。
        //
        // 参数:
        //   node:
        //     要访问的表达式。
        //
        // 返回结果:
        //     修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。
        protected override Expression VisitUnary(UnaryExpression node)
        {
            return base.VisitUnary(node);
        }
    }
}
