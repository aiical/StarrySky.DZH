using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StarrySky.DZH.ORMTool.ORMFramework.ORMEnum;

namespace StarrySky.DZH.ORMTool.ORMFramework
{
    /// <summary>
    /// 查询参数实体
    /// </summary>
    public class ParamValue
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public ParamValue()
        {
            this.isSql = false;
            this.SqlStr = string.Empty;
            this.fieldName = string.Empty;
            this.value = null;
            this.cdt = ConditionEnum.Equal;
            this.opr = OperatorEnum.And;
        }
        /// <summary>
        /// 是否SQL查询 *Update里用于标识是否在其它更新有变更时才更新
        /// </summary>
        public bool isSql { get; set; }
        /// <summary>
        /// 查询SQL
        /// </summary>
        public string SqlStr { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string fieldName { get; set; }
        /// <summary>
        /// 查询值
        /// </summary>
        public object value { get; set; }
        /// <summary>
        /// 查询操作关系
        /// </summary>
        public ConditionEnum cdt { get; set; }
        /// <summary>
        /// 查询操作类型
        /// </summary>
        public OperatorEnum opr { get; set; }
    }

    /// <summary>
    /// 联表参数实体
    /// </summary>
    class JoinParam
    {
        /// <summary>
        /// 关联类型
        /// </summary>
        public JoinEnum joinType { get; set; }
        /// <summary>
        /// 主表关联字段
        /// </summary>
        public Enum mainTableField { get; set; }
        /// <summary>
        /// 联表关联字段
        /// </summary>
        public Enum joinTableField { get; set; }
        /// <summary>
        /// 主表别名
        /// </summary>
        public string mainTableAlias { get; set; }
        /// <summary>
        /// 联表别名
        /// </summary>
        public string joinTableAlias { get; set; }
        /// <summary>
        /// 联表关联字段
        /// </summary>
        public string joinStr { get; set; }
    }

    /// <summary>
    /// 排序参数实体
    /// </summary>
    public class OrderParam
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string orderField { get; set; }
        /// <summary>
        /// 排序枚举
        /// </summary>
        public OrderEnum orderEnum { get; set; }
    }

    /// <summary>
    /// 变更值
    /// </summary>
    public class ChangedValues
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string fieldName { get; set; }
        /// <summary>
        /// 字段备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 原值
        /// </summary>
        public object oldValue { get; set; }
        /// <summary>
        /// 新值
        /// </summary>
        public object newValue { get; set; }
    }

    /// <summary>
    /// 表配置实体
    /// </summary>
    public class TableConfigModel
    {
        /// <summary>
        /// 分片key
        /// </summary>
        public List<string> ShardKeys { get; set; } = new List<string>();

        /// <summary>
        /// 默认不生成字段
        /// </summary>
        public List<string> NGFields { get; set; } = new List<string>();

        /// <summary>
        /// 只读字段
        /// </summary>
        public List<string> ReadOnlyFields { get; set; } = new List<string>();

        /// <summary>
        /// 逻辑主键
        /// </summary>
        public string LIKey { get; set; } = string.Empty;
    }
}