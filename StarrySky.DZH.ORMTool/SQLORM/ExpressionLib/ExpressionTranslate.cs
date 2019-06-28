using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.SQLORM.ExpressionLib
{
    public class ExpressionTranslate
    {
        public static List<string> GetSelectColumn(Expression expr)
        {
            //var expression = GetRealExpression(expr);
            var vistor = new ExpressionVisitorImpl();
            vistor.Visit(expr);
            var result = vistor.SelectColumnList;

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
