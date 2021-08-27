using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovitCodeTools.Common
{
    public class SqlServerDBA
    {
        /// <summary>
        /// 查询db下所有表名和表注释
        /// </summary>
        /// <param name="connStr">连接字串</param>
        /// <returns></returns>
        public static string AllTablesSql() =>
                    @"SELECT 
                        so.name as tableName,
                        Convert( VARCHAR(10), ep.[value]) AS tableComment
                    FROM 
                        sysobjects so(NOLOCK)
                        LEFT JOIN sys.extended_properties ep(NOLOCK) ON ep.major_id=so.id AND ep.minor_id=0
                    WHERE  
                        so.[type]='U' AND so.name<>'sysdiagrams' 
                    ORDER BY 
                        so.name";

        /// <summary>
        /// 查表下所有列的所有信息（参数化sql）
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string AllColumnsSql() =>
                     @"SELECT  
                           c.name AS CollName,
                           t.name AS CollType,
                           c.length AS CollLength,
                           c.isnullable AS CollIsNull,
                           (
                               SELECT COUNT(1) FROM sys.identity_columns ic(NOLOCK) WHERE ic.[object_id]=c.id AND ic.column_id=c.colid 
                           ) AS CollIsIdentity,
                           (
                               SELECT VALUE FROM   sys.extended_properties ep(NOLOCK) WHERE  ep.major_id = c.id AND ep.minor_id=c.colid
                           ) AS CollDescription,
                           CollIsPrimary=CASE WHEN EXISTS (SELECT 1 FROM sysobjects WHERE xtype='PK' and parent_obj=c.id and name IN (
                               SELECT name FROM sysindexes WHERE indid in(SELECT indid FROM sysindexkeys WHERE id = c.id AND colid=c.colid))) THEN 1 ELSE 0 END
                       FROM 
                           syscolumns c(NOLOCK)
                           INNER JOIN sys.tables ts(NOLOCK) ON ts.[object_id] = c.id
                           INNER JOIN sys.types t(NOLOCK) ON t.system_type_id=c.xtype
                           INNER JOIN systypes st(NOLOCK) ON st.name=t.name AND st.name<>'sysname'
                           INNER JOIN sysusers su(NOLOCK) ON st.uid=su.uid AND su.name='sys'
                           --INNER JOIN syscolumns s(NOLOCK) ON c.[object_id]=s.id
                       WHERE 
                           ts.name=@tableName
                       ORDER BY
                           c.id ASC";
    }
}
