using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.ExpressionLib
{
    public static class ExpressionExtends
    {
        /// <summary>
        /// 将表达式运算符转为sql运算符
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToSqlOperator(this ExpressionType type)
        {
            switch (type)
            {
                case (ExpressionType.AndAlso):
                case (ExpressionType.And):
                    return " AND ";
                case (ExpressionType.OrElse):
                case (ExpressionType.Or):
                    return " OR ";
                case (ExpressionType.Not):
                    return " NOT ";
                case (ExpressionType.NotEqual):
                    return "<>";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case (ExpressionType.Equal):
                    return "=";
                default:
                    throw new Exception("不支持该方法");
            }
        }
    }
}
