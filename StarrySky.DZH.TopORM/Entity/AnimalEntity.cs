using StarrySky.DZH.TopORM.Common;
using StarrySky.DZH.TopORM.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.Entity
{
    /// <summary>
    /// 不存表的字段加Ignore（不存or表没有） 否则报错MySql.Data.MySqlClient.MySqlException:“Unknown column 'xxx' in 'field list'”
    /// </summary>
    [TableInfo("Animal", DBTypeEnum.MySQL)]
    public class AnimalEntity
    {
        [PrimaryKey]
        public long AId { get; set; }

        public string AName { get; set; }

        public int ASex { get; set; }

        public int ARowStatus { get; set; }

        public DateTime ACreateTime { get; set; }

        public string ACreateUser { get; set; }

        public DateTime AUpdateTime { get; set; }

        public string AUpdateUser { get; set; }

        [IgnoreField]
        public string AAddress { get; set; }
    }
}
