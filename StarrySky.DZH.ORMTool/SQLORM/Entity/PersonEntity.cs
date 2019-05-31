using StarrySky.DZH.ORMTool.SQLORM.Common;
using StarrySky.DZH.ORMTool.SQLORM.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.SQLORM.Entity
{
    /// <summary>
    /// 不用的字段加Ignore 否则报错MySql.Data.MySqlClient.MySqlException:“Unknown column 'xxx' in 'field list'”
    /// </summary>
    [TableInfo("Demo", DBTypeEnum.MySQL)]
    public class DemoEntity
    {
        [PrimaryKey]
        public long DId { get; set; }

        public string DName { get; set; }

        public int DSex { get; set; }

        public int DRowStauts { get; set; }

        public DateTime DCreateTime { get; set; }

        public string DCreateUser { get; set; }

        public DateTime DUpdateTime { get; set; }

        public string DUpdateUser { get; set; }

        [IgnoreField]
        public string DAddress { get; set; }
    }
}
