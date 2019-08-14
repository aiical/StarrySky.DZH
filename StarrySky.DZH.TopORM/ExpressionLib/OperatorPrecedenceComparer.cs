using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.ExpressionLib
{
    /// <summary>
    /// 重写比较方法,用于判断表达式连接是否需要加括号
    /// 有or连接的需要用到
    /// </summary>
    public class OperatorPrecedenceComparer : Comparer<ExpressionType>
    {
        public override int Compare(ExpressionType x, ExpressionType y)
            => Precedence(x).CompareTo(Precedence(y));

        private int Precedence(ExpressionType expressionType)
        {
            if (expressionType == ExpressionType.OrElse)
                return expressionType.GetHashCode();
            return -100;
        }
    }
}
