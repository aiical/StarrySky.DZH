using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovitCodeTools.Common
{
    public class TableDetailsInfo
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string CollName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string CollType { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        public string CollLength { get; set; }


        /// <summary>
        /// 是否可null
        /// </summary>
        public int CollIsNull { get; set; }

        /// <summary>
        /// 是否自增主键
        /// </summary>
        public int CollIsIdentity { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string CollDescription { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public int CollIsPrimary { get; set; }
    }
}
