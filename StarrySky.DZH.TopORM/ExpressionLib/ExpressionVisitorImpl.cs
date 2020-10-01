using Dapper;
using StarrySky.DZH.TopORM.Common;
using StarrySky.DZH.TopORM.CustomAttribute;
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
        private readonly IComparer<ExpressionType> m_comparer = new OperatorPrecedenceComparer();

        private ExpressionVisitorImpl() { }

        public ExpressionVisitorImpl(TranslatorEnum etranslator)
        {
            _eTranslator = etranslator;
        }
        private TranslatorEnum _eTranslator = TranslatorEnum.Default;
        public List<string> SelectColumnList = new List<string>();
        public List<string> UpdateColumnList = new List<string>();
        public StringBuilder WhereStrBuilder = new StringBuilder();
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
        /// <summary>
        /// 检查是否需要加括号, true需要括号
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        protected bool CheckBinary(BinaryExpression node, ExpressionType parentType)
        {
            if (node == null)
            {
                return false;
            }
            if (m_comparer.Compare(parentType, node.NodeType) != 0)
            {
                return true;
            }
            return false;
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
            StringBuilder sbstr = new StringBuilder();
            // t=>!(t.DRowStatus == 3) 非BinaryExpression
            if (node.Left is BinaryExpression || node.Right is BinaryExpression)
            {
                var ifLeft = CheckBinary(node.Left as BinaryExpression, node.NodeType);
                var ifRight = CheckBinary(node.Right as BinaryExpression, node.NodeType);
                if (ifLeft)
                {
                    WhereStrBuilder.Append(" ( ");
                }
                Expression left = this.Visit(node.Left);
                if (ifLeft)
                {
                    WhereStrBuilder.Append(" ) ");
                }
                WhereStrBuilder.Append(node.NodeType.ToSqlOperator());
                if (ifRight)
                {
                    WhereStrBuilder.Append(" ( ");
                }
                Expression right = this.Visit(node.Right);
                if (ifRight)
                {
                    WhereStrBuilder.Append(" ) ");
                }
            }
            else
            {
                MemberExpression leftExpress = node.Left as MemberExpression;
                if (leftExpress == null)
                {
                    throw new NotImplementedException($"TopORM不支持当前类型{node.ToString()}");
                }
                object[] PrimaryKey = leftExpress.Member.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                object[] Ignore = leftExpress.Member.GetCustomAttributes(typeof(IgnoreFieldAttribute), false);
                if (!PrimaryKey.CollectionIsNullOrEmpty() && _eTranslator == TranslatorEnum.Update)
                {
                    throw new Exception("主键字段不支持更新");
                }
                if (!Ignore.CollectionIsNullOrEmpty())
                {
                    return node;
                }
                else
                {
                    string name = leftExpress.Member.Name;
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

                    string paramsName = "";
                    if (_eTranslator == TranslatorEnum.Update)
                    {
                        paramsName = $"{name}{AutoIncrementId.ToString()}";
                        UpdateColumnList.Add($"{name}{node.NodeType.ToSqlOperator()}@{paramsName}");
                    }
                    else if (_eTranslator == TranslatorEnum.Where)
                    {
                        paramsName = $"{name}_W{AutoIncrementId.ToString()}";
                        WhereStrBuilder.Append($"{name}{node.NodeType.ToSqlOperator()}@{paramsName}");
                    }
                    Parameters.Add($"{paramsName}", value);
                    AutoIncrementId++;
                }
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
                object[] PrimaryKey = item.Member.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                object[] Ignore = item.Member.GetCustomAttributes(typeof(IgnoreFieldAttribute), false);
                if (!PrimaryKey.CollectionIsNullOrEmpty() && _eTranslator == TranslatorEnum.Update)
                {
                    throw new Exception("主键字段不支持更新");
                }
                if (!Ignore.CollectionIsNullOrEmpty())
                {
                    return node;
                }
                string paramsName = "";
                if (_eTranslator == TranslatorEnum.Update)
                {
                    paramsName = $"{item.Member.Name}{AutoIncrementId.ToString()}";
                    UpdateColumnList.Add($"{item.Member.Name}=@{paramsName}");
                }
                else if (_eTranslator == TranslatorEnum.Where)
                {
                    paramsName = $"{item.Member.Name}_W{AutoIncrementId.ToString()}";
                    WhereStrBuilder.Append($"{item.Member.Name}=@{paramsName}");
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
            if (!properties.CollectionIsNullOrEmpty())
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


        protected override Expression VisitUnary(UnaryExpression node)
        {
            throw new NotImplementedException($"TOP ORM 不支持一元表达式{node.ToString()}");
        }
    }
}
