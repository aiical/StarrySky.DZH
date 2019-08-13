using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.ExpressionLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpressionTranslate
    {
        /*
         * BinaryExpression 表示包含二元运算符的表达式。 可以理解为一个子表达式,如 b.Number>10
         * MemberExpression 表示访问字段或属性。 如 b.Number
         * NewArrayExpression 表示创建新数组并可能初始化该新数组的元素
         * MethodCallExpression 表示对静态方法或实例方法的调用 如 b.Name.Contains("123")
         * ConstantExpression 表示具有常量值的表达式 如 b.Name="hubro" 
         * UnaryExpression 表示包含一元运算符的表达式
         */

        public static string ToSqlOperator(ExpressionType type)
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


        public static List<string> GetSelectColumn(Expression expr)
        {
            //var expression = GetRealExpression(expr);
            var vistor = new ExpressionVisitorImpl();
            vistor.Visit(expr);
            var result = vistor.SelectColumnList;

            return result;
        }

        public static List<string> GetUpdateColumn(Expression expr)
        {
            //var expression = GetRealExpression(expr);
            var vistor = new ExpressionVisitorImpl();
            vistor.Visit(expr);
            var result = vistor.UpdateColumnList;

            return result;
        }

        public static List<string> GetWhereColumn(Expression expr)
        {
            var vistor = new ExpressionVisitorImpl();
            vistor.Visit(expr);
            var result = vistor.WhereColumnList;

            return result;
        }

        /// <summary>
        /// 获取有效的表达式部件
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Expression GetRealExpression(Expression expr)
        {
            if (expr == null)
            {
                return null;
                //return "";
                //throw new ArgumentNullException("expression", "不能为null");
            }
            if (!(expr is LambdaExpression))
            {
                return null;
                //return "";
            }
            LambdaExpression expression = expr as LambdaExpression;
            //List<string> SelectColumnList = new List<string>();
            if (expression.Body is MemberExpression)
            {
                //SelectColumnList.Add(express.Member.Name);
                //return string.Join(",", SelectColumnList);
                return expression.Body;
            }
            if (expression.Body is ParameterExpression)
            {
                //SelectColumnList.Add(express.Member.Name);
                //return string.Join(",", SelectColumnList);
                return expression.Body;
            }
            return null;
        }
    }
}
