using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovitCodeTools.Common
{
    /// <summary>
    /// 数据访问公共方法枚举
    /// </summary>
    public class ORMEnum
    {
        /// <summary>
        /// 查询操作关系
        /// </summary>
        public enum ConditionEnum
        {
            /// <summary>等于</summary>
            [Description("=")]
            Equal,

            /// <summary>不等于</summary>
            [Description("!=")]
            NotEqual,

            /// <summary>大于</summary>
            [Description(">")]
            GreaterThan,

            /// <summary>小于</summary>
            [Description("<")]
            LessThan,

            /// <summary>大于等于</summary>
            [Description(">=")]
            GreaterThanAndEqual,

            /// <summary>小于等于</summary>
            [Description("<=")]
            LessThanAndEqual,

            /// <summary>in 操作</summary>
            [Description("in")]
            InOpertion,

            /// <summary>like 操作</summary>
            [Description("like")]
            Like,

            /// <summary>%like 操作</summary>
            [Description("like")]
            LeftLike,

            /// <summary>like% 操作</summary>
            [Description("like")]
            RightLike,

            /// <summary>%like% 操作</summary>
            [Description("like")]
            FuzzyLike
        }

        /// <summary>
        /// 查询操作类型
        /// </summary>
        public enum OperatorEnum
        {
            /// <summary>and 关系</summary>
            And,

            /// <summary>or 关系</summary>
            Or
        }

        /// <summary>
        /// 排序枚举
        /// </summary>
        public enum OrderEnum
        {
            /// <summary>升序</summary>
            Asc,
            /// <summary>降序</summary>
            Desc
        }

        /// <summary>
        /// 联表枚举
        /// </summary>
        public enum JoinEnum
        {
            /// <summary>
            /// 连接
            /// </summary>
            [Description("Join")]
            Join,
            /// <summary>
            /// 左连接
            /// </summary>
            [Description("Left Join")]
            LeftJoin,
            /// <summary>
            /// 右连接
            /// </summary>
            [Description("Right Join")]
            RightJoin,
            /// <summary>
            /// 内连接
            /// </summary>
            [Description("Inner Join")]
            InnerJoin,
            /// <summary>
            /// 全连接
            /// </summary>
            [Description("Full Join")]
            FullJoin
        }

        /// <summary>
        /// 数据库枚举
        /// </summary>
        public enum DataBaseType
        {
            /// <summary>
            ///  微软 sql server数据库
            /// </summary>
            [Description("SQLServer")]
            SQLServer,
            /// <summary>
            ///  SUM MySQL数据库
            /// </summary>
            [Description("MySQL")]
            MySQL
        }


        /// <summary>
        /// elementUI控件类型
        /// </summary>
        public enum UIControlsType
        {
            input,
            select,
            checkbox,
            radio,
            date,

        }
    }
}