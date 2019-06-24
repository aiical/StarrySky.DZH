using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.SQLORM.ExpressionLib
{
    public class ExpressionProvider
    {
        private static object GetExpression(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "不能为null");
            }

            if (expression is BinaryExpression)
            {
               
            }
            if (expression is BlockExpression)
            {
                throw new NotImplementedException("未实现的BlockExpression2Sql");
            }
            if (expression is ConditionalExpression)
            {
                throw new NotImplementedException("未实现的ConditionalExpression2Sql");
            }
            if (expression is ConstantExpression)
            {
                //return new ConstantExpression2Sql();
            }
            if (expression is DebugInfoExpression)
            {
                throw new NotImplementedException("未实现的DebugInfoExpression2Sql");
            }
            if (expression is DefaultExpression)
            {
                throw new NotImplementedException("未实现的DefaultExpression2Sql");
            }
            if (expression is DynamicExpression)
            {
                throw new NotImplementedException("未实现的DynamicExpression2Sql");
            }
            if (expression is GotoExpression)
            {
                throw new NotImplementedException("未实现的GotoExpression2Sql");
            }
            if (expression is IndexExpression)
            {
                throw new NotImplementedException("未实现的IndexExpression2Sql");
            }
            if (expression is InvocationExpression)
            {
                throw new NotImplementedException("未实现的InvocationExpression2Sql");
            }
            if (expression is LabelExpression)
            {
                throw new NotImplementedException("未实现的LabelExpression2Sql");
            }
            if (expression is LambdaExpression)
            {
                throw new NotImplementedException("未实现的LambdaExpression2Sql");
            }
            if (expression is ListInitExpression)
            {
                throw new NotImplementedException("未实现的ListInitExpression2Sql");
            }
            if (expression is LoopExpression)
            {
                throw new NotImplementedException("未实现的LoopExpression2Sql");
            }
            if (expression is MemberExpression)
            {
                //return new MemberExpression2Sql();
            }
            if (expression is MemberInitExpression)
            {
                throw new NotImplementedException("未实现的MemberInitExpression2Sql");
            }
            if (expression is MethodCallExpression)
            {
               // return new MethodCallExpression2Sql();
            }
            if (expression is NewArrayExpression)
            {
                //return new NewArrayExpression2Sql();
            }
            if (expression is NewExpression)
            {
                //return new NewExpression2Sql();
            }
            if (expression is ParameterExpression)
            {
                throw new NotImplementedException("未实现的ParameterExpression2Sql");
            }
            if (expression is RuntimeVariablesExpression)
            {
                throw new NotImplementedException("未实现的RuntimeVariablesExpression2Sql");
            }
            if (expression is SwitchExpression)
            {
                throw new NotImplementedException("未实现的SwitchExpression2Sql");
            }
            if (expression is TryExpression)
            {
                throw new NotImplementedException("未实现的TryExpression2Sql");
            }
            if (expression is TypeBinaryExpression)
            {
                throw new NotImplementedException("未实现的TypeBinaryExpression2Sql");
            }
            if (expression is UnaryExpression)
            {
                //return new UnaryExpression2Sql();
            }
            throw new NotImplementedException("未实现的Expression2Sql");
        }
    }
}
