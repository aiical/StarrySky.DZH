using StarrySky.DZH.TopORM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableInfoAttribute : Attribute
    {
        public string TableName { get; set; }
        public DBTypeEnum DBType { get; set; }
        public TableInfoAttribute(string tableName, DBTypeEnum dbType)
        {
            TableName = tableName;
            DBType = dbType;
        }
    }

    /// <summary>
    /// Specifies that this field is a primary key in the database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }


    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreFieldAttribute : Attribute
    {
    }

    
}
