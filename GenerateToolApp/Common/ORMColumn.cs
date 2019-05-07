using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenerateToolApp.Common.ORMEnum;

namespace GenerateToolApp.Common
{
    /// <summary>
    /// 字段信息
    /// </summary>
    public class ORMColumn
    {
        /// <summary>
        /// 字段序号
        /// </summary>
        public int CIndex { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 字段数据库类型  int  varchar  等
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 字段代码类型  int  string 等
        /// </summary>
        public string CodeType { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Defalut { get; set; } = "";

        /// <summary>
        /// 是否非空
        /// </summary>
        public bool IsNull { get; set; } = false;

        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimaryKey { get; set; } = false;

        /// <summary>
        /// 是否是当前行状态字段
        /// </summary>
        public bool IsRowFlag { get; set; } = false;

        /// <summary>
        /// 对应页面上的控件类型
        /// </summary>
        public UIControlsType UIControls { get; set; } = UIControlsType.input;
    }
}