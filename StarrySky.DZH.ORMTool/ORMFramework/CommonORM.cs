using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using StarrySky.DZH.Util.Common;
using StarrySky.DZH.Util.DataConvert;
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static StarrySky.DZH.ORMTool.ORMFramework.ORMEnum;

namespace StarrySky.DZH.ORMTool.ORMFramework
{
    /// <summary>
    /// 公共数据访问方法
    /// <para>using Dapper</para>
    /// <para>using Dapper.Contrib</para>
    /// </summary>
    public class CommonORM
    {
        #region 初始化
        /// <summary>
        /// 初始化数据库类型
        /// </summary>
        static CommonORM()
        {
            SqlMapperExtensions.GetDatabaseType = DBConfig.GetDatabaseType;
            //dbConnectionFactory = ServiceLocator.Instance.GetRequiredService<IDbConnectionFactory>();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="isExport">是否导出</param>
        public CommonORM(bool isExport = false)
        {
            this.useTableList = new List<string>();
            this.shows = new List<string>();
            this.wheres = new List<ParamValue>();
            this.joins = new List<JoinParam>();
            this.orders = new List<OrderParam>();
            this.updates = new List<ParamValue>();
            this.totalCount = 0;
            this.pageIndex = 1;
            this.pageSize = 0;
            this.dataBaseName = string.Empty;
            this.tableName = string.Empty;
            this.primaryKey = string.Empty;
            this.Params = new Dictionary<string, object>();
            this.IsExport = isExport;
            //this.DataBaseType = dataBaseType;
            this.UnOrder = false;
            this.ChangedValues = new List<ChangedValues>();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tableInfo">需要查询的 数据库.dbo.表名</param>
        /// <param name="isExport">是否导出</param>
        public CommonORM(string tableInfo, bool isExport = false)
        {
            this.useTableList = new List<string>();
            this.shows = new List<string>();
            this.wheres = new List<ParamValue>();
            this.joins = new List<JoinParam>();
            this.orders = new List<OrderParam>();
            this.updates = new List<ParamValue>();
            this.totalCount = 0;
            this.pageIndex = 1;
            this.pageSize = 0;
            //this.DataBaseType = dataBaseType;
            if (!string.IsNullOrWhiteSpace(tableInfo))
            {
                string[] ti = tableInfo.Split('.');
                this.dataBaseName = ti[0].Trim().TrimStart('[').TrimEnd(']');
                this.tableName = ti[ti.Length - 1].Trim().TrimStart('[').TrimEnd(']');
            }
            this.primaryKey = string.Empty;
            this.Params = new Dictionary<string, object>();
            this.IsExport = isExport;
            this.UnOrder = false;
            this.ChangedValues = new List<ChangedValues>();
        }
        #endregion

        #region 公共变量
        /// <summary>总数</summary>
        private int totalCount = 0;
        /// <summary>获取数据总条数 *只读</summary>
        public int TotalCount
        {
            get { return totalCount; }
        }
        /// <summary>sql语句</summary>
        StringBuilder sql = new StringBuilder();
        /// <summary>获取生成的SQL语句 *只读</summary>
        public string SqlStr
        {
            get { return sql.ToString(); }
        }
        /// <summary>update影响条数</summary>
        private int updateRows = 0;
        /// <summary>Update更新所影响的数据条数 *只读</summary>
        public int UpdateRows
        {
            get { return updateRows; }
        }
        /// <summary>当前AddUpdate的数量 *只读</summary>
        public int UpdatesCount
        {
            get { return updates == null ? 0 : updates.Count; }
        }
        /// <summary>当前页数</summary>
        public int pageIndex { get; set; }

        /// <summary>每页显示</summary>
        public int pageSize { get; set; }

        /// <summary>每页显示默认值</summary>
        public const int defaultSize = 10;

        /// <summary>数据库名</summary>
        public string dataBaseName { get; set; }

        /// <summary>查询表名</summary>
        public string tableName { get; set; }

        /// <summary>主键</summary>
        public string primaryKey { get; set; }

        /// <summary>表别名</summary>
        public string alias { get; set; }

        /// <summary>是否导出</summary>
        public bool IsExport { get; set; }

        /// <summary>无需自动排序</summary>
        public bool UnOrder { get; set; }

        /// <summary>字段枚举</summary>
        public Type fieldsEmum { get; set; }

        /// <summary>变更值列表</summary>
        public List<ChangedValues> ChangedValues { get; set; }

        private bool hasReadDataBase = false;
        private DataBaseType dataBaseTypeInit = DataBaseType.SQLServer;
        /// <summary>数据库种类（不同的sql语句）</summary>
        public DataBaseType DataBaseType { get { return dataBaseTypeInit; } set { dataBaseTypeInit = value; } }
        /// <summary>查询列</summary>
        private List<string> shows { get; set; }

        /// <summary>使用到的表</summary>
        private List<string> useTableList { get; set; }

        /// <summary>查询条件</summary>
        private List<ParamValue> wheres { get; set; }

        /// <summary>排序列</summary>
        private List<OrderParam> orders { get; set; }

        /// <summary>关联参数</summary>
        private List<JoinParam> joins { get; set; }

        /// <summary>更新参数</summary>
        private List<ParamValue> updates { get; set; }

        /// <summary>查询参数</summary>
        private Dictionary<string, object> Params { get; set; }

        /// <summary>SQL注释</summary>
        private string notes { get; set; }

        //private static IDbConnectionFactory dbConnectionFactory;
        #endregion

        #region 公共调用

        /// <summary>
        /// 添加SQL注释
        /// </summary>
        /// <param name="author">作者</param>
        /// <param name="forFun">作用</param>
        /// <param name="file">文件路径</param>
        /// <param name="fun">方法</param>
        /// <returns>string</returns>
        public void AddNotes(string author, string forFun, string file = "", string fun = "")
        {
            notes = string.Format("/* Flat:ORM/Author:{0}/For:{1}{2}{3} */", author, forFun, string.IsNullOrWhiteSpace(file) ? string.Empty : "/File:" + file, string.IsNullOrWhiteSpace(fun) ? string.Empty : "/Fun:" + fun);
        }

        /// <summary>用于存放SqlClient实例</summary>
        //public static ConcurrentDictionary<string, ConcurrentBag<DbConnection>> sqlCD = new ConcurrentDictionary<string, ConcurrentBag<DbConnection>>();

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <param name="dataBase">数据库名称</param>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <returns></returns>
        public static DbConnection GetSqlConnection(string dataBase, bool useWriteDataBase = false)
        {
            var conn = DBConfig.GetDbConnection(DBConfig.GetDbDataSource(dataBase));
            if (conn?.State == ConnectionState.Closed) try { conn.Open(); } finally { }
            return (DbConnection)conn;
            //DbConnection conn = null;
            //ConcurrentBag<DbConnection> connPool = new ConcurrentBag<DbConnection>();
            //string key = string.Format("{0}_{1}", dataBase, useWriteDataBase ? "write" : "read");
            //if (sqlCD.TryGetValue(key, out connPool))
            //{
            //    if (connPool == null || !connPool.Any())//链接池里没链接
            //    {
            //        conn = DataSource.GetConnection(dataBase, !useWriteDataBase);
            //        connPool.Add(conn);
            //        if (conn?.State == ConnectionState.Closed) try { conn.Open(); } finally { }
            //        return conn;
            //    }
            //    var canUseConn = connPool.Where(t => t.State == ConnectionState.Closed || t.State == ConnectionState.Broken);
            //    if (canUseConn.Any())//存在可复用链接
            //    {
            //        conn = canUseConn.FirstOrDefault();
            //        if (conn?.State == ConnectionState.Broken) conn.Close();
            //        if (conn?.State == ConnectionState.Closed) try { conn.Open(); } finally { }
            //        return conn;
            //    }
            //    //无可复用链接则新开链接
            //    conn = DataSource.GetConnection(dataBase, !useWriteDataBase);
            //    connPool.Add(conn);
            //    if (conn?.State == ConnectionState.Closed) try { conn.Open(); } finally { }
            //    return conn;
            //}
            //conn = DataSource.GetConnection(dataBase, !useWriteDataBase);
            //if (!sqlCD.ContainsKey(key)) sqlCD.TryAdd(key, new ConcurrentBag<DbConnection> { conn });
            //if (conn?.State == ConnectionState.Closed) try { conn.Open(); } finally { }
            //return conn;
        }


        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">要获取数据库的表实体</param>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <returns></returns>
        public static DbConnection GetSqlConnection<T>(T entity, bool useWriteDataBase = false) where T : class
        {
            return GetSqlConnection(DALAttrHelper.GetDataBase(entity), useWriteDataBase);
        }

        /// <summary>
        /// 根据字段名获取字段备注
        /// </summary>
        /// <param name="fieldname">字段名</param>
        /// <returns></returns>
        string GetCNameFromFieldName(string fieldname)
        {
            string enumname = string.Empty;
            FieldInfo[] fields = fieldsEmum.GetFields();
            //检索所有字段
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum == true && field.Name == fieldname)
                {
                    DescriptionAttribute da = null;
                    object[] arrobj = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (arrobj.Length > 0)
                    {
                        da = arrobj[0] as DescriptionAttribute;
                    }
                    if (da != null)
                    {
                        //枚举中文描述
                        enumname = da.Description;
                    }
                }
            }
            return enumname;
        }

        /// <summary>
        /// 获取WHERE条件SQL
        /// </summary>
        /// <returns></returns>
        string GetWhereStr()
        {
            StringBuilder whereStr = new StringBuilder();
            switch (this.DataBaseType)
            {
                case DataBaseType.MySQL: whereStr.Append(new TranslatorForMYSQL().FuseGetWhereStr(wheres, Params)); break;
                case DataBaseType.SQLServer:
                default:
                    for (int i = 0, len = wheres.Count; i < len; i++)
                    {
                        if (wheres[i].isSql)//使用SQL查询
                        {
                            if (string.IsNullOrWhiteSpace(wheres[i].SqlStr)) continue;
                            string Operator = string.Empty;
                            string wsql = wheres[i].SqlStr.Trim().ToLower();
                            if (wsql.StartsWith("and")) Operator = string.Empty;
                            else if (wsql.StartsWith("or")) Operator = string.Empty;
                            else Operator = " and ";
                            whereStr.AppendFormat(" {0} {1} ", Operator, wheres[i].SqlStr);
                        }
                        else//使用参数化
                        {
                            if (wheres[i].cdt == ConditionEnum.FuzzyLike)
                            {
                                whereStr.AppendFormat(" {0} [{1}] {2} '%'+@{1}{3}+'%'", wheres[i].opr.PackString(), wheres[i].fieldName, "like", i);
                            }
                            else if (wheres[i].cdt == ConditionEnum.LeftLike)
                            {
                                whereStr.AppendFormat(" {0} [{1}] {2} '%'+@{1}{3}", wheres[i].opr.PackString(), wheres[i].fieldName, "like", i);
                            }
                            else if (wheres[i].cdt == ConditionEnum.RightLike)
                            {
                                whereStr.AppendFormat(" {0} [{1}] {2} @{1}{3}+'%'", wheres[i].opr.PackString(), wheres[i].fieldName, "like", i);
                            }
                            else if (wheres[i].cdt == ConditionEnum.InOpertion)
                            {
                                string inStr = wheres[i].value.PackString().Trim().TrimStart('(').TrimEnd(')').Trim(',');
                                var enumerable = TranslatorForMYSQL.GetMultiExec(wheres[i].value);
                                if (enumerable != null)
                                {
                                    StringBuilder inStrb = new StringBuilder();
                                    foreach (var item in enumerable)
                                    {
                                        if (item is string) inStrb.AppendFormat("'{0}',", item);
                                        else inStrb.AppendFormat("{0},", item);
                                    }
                                    inStr = inStrb.ToString().Trim(',');
                                }
                                whereStr.AppendFormat(" {0} [{1}] {2} ({3})", wheres[i].opr.PackString(), wheres[i].fieldName, "in", inStr);
                            }
                            else whereStr.AppendFormat(" {0} [{1}] {2} @{1}{3}", wheres[i].opr.PackString(), wheres[i].fieldName, wheres[i].cdt.PackEnumDescription(), i);
                            if (wheres[i].cdt != ConditionEnum.InOpertion) Params.Add(string.Format("{0}{1}", wheres[i].fieldName, i), wheres[i].value);
                        }
                    }
                    break;
            }
            return whereStr.ToString();
        }

        /// <summary>
        /// 获取联表SQL
        /// </summary>
        /// <returns></returns>
        string GetJoinStr()
        {
            if (joins == null || joins.Count < 1) return string.Empty;
            StringBuilder joinStr = new StringBuilder();
            foreach (var join in joins)
            {
                if (!string.IsNullOrWhiteSpace(join.joinStr))
                {
                    joinStr.AppendFormat(join.joinStr);
                    continue;
                }
                if (join.mainTableField == null || join.joinTableField == null) continue;
                string tableName = DALAttrHelper.GetTableNameByFields(join.joinTableField).TableFullName;
                if (string.IsNullOrWhiteSpace(tableName)) throw new Exception(string.Format("表名获取失败 *请用最新的生成工具生成字段【{0}】所在的表", join.joinTableField));
                joinStr.AppendFormat(string.Format(" {0} {1} {2} ON {3}{4}={5}{6} ",
                    join.joinType.PackEnumDescription(), tableName, join.joinTableAlias,
                    string.IsNullOrWhiteSpace(join.mainTableAlias) ? string.Empty : join.mainTableAlias + ".", join.mainTableField,
                    string.IsNullOrWhiteSpace(join.joinTableAlias) ? string.Empty : join.joinTableAlias + ".", join.joinTableField));
            }
            return joinStr.ToString();
        }

        /// <summary>
        /// 获取联表MySQL
        /// </summary>
        /// <returns></returns>
        string GetMySqlJoinStr()
        {
            if (joins == null || joins.Count < 1) return string.Empty;
            StringBuilder joinStr = new StringBuilder();
            foreach (var join in joins)
            {
                if (!string.IsNullOrWhiteSpace(join.joinStr))
                {
                    joinStr.AppendFormat(join.joinStr);
                    continue;
                }
                if (join.mainTableField == null || join.joinTableField == null) continue;
                var tableInfo = DALAttrHelper.GetTableNameByFields(join.joinTableField);
                string tableName = tableInfo.DataBaseName + "." + tableInfo.TableName;
                if (string.IsNullOrWhiteSpace(tableName)) throw new Exception(string.Format("表名获取失败 *请用最新的生成工具生成字段【{0}】所在的表", join.joinTableField));
                joinStr.AppendFormat(string.Format(" {0} {1} {2} ON {3}{4}={5}{6} ",
                    join.joinType.PackEnumDescription(), tableName, join.joinTableAlias,
                    string.IsNullOrWhiteSpace(join.mainTableAlias) ? string.Empty : join.mainTableAlias + ".", join.mainTableField,
                    string.IsNullOrWhiteSpace(join.joinTableAlias) ? string.Empty : join.joinTableAlias + ".", join.joinTableField));
            }
            return joinStr.ToString();
        }
        #endregion

        #region 添加查询列
        /// <summary>
        /// 添加查询列
        /// </summary>
        /// <param name="fieldName">列枚举</param>
        public void AddShow(Enum fieldName)
        {
            shows.Add(fieldName.PackString());
            dynamic tableInfo = DALAttrHelper.GetTableNameByFields(fieldName);
            GetDataBaseTypeByFields(fieldName);
            if (!useTableList.Contains(tableInfo.TableName)) useTableList.Add(tableInfo.TableName);
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = tableInfo.DataBaseName;
            if (string.IsNullOrWhiteSpace(tableName)) tableName = tableInfo.TableName;
        }
        /// <summary>
        /// 添加查询列
        /// </summary>
        /// <param name="fieldNames">列枚举</param>
        public void AddShow(params Enum[] fieldNames)
        {
            if (fieldNames == null || fieldNames.Length < 1) return;
            dynamic tableInfo = DALAttrHelper.GetTableNameByFields(fieldNames[0]);
            GetDataBaseTypeByFields(fieldNames[0]);
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = tableInfo.DataBaseName;
            if (string.IsNullOrWhiteSpace(tableName)) tableName = tableInfo.TableName;
            for (int i = 0, len = fieldNames.Length; i < len; i++)
            {
                shows.Add(fieldNames[i].PackString());
                if (!useTableList.Contains(tableInfo.TableName)) useTableList.Add(tableInfo.TableName);
            }
        }
        /// <summary>
        /// 添加查询列
        /// </summary>
        /// <param name="fieldName">列名</param>
        public void AddShow(string fieldName)
        {
            shows.Add(fieldName);
        }
        #endregion

        #region 添加查询条件
        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="fieldName">列名</param>
        /// <param name="value">查询值</param>
        /// <param name="cdt">查询操作关系</param>
        /// <param name="opr">查询操作类型</param>
        public void AddWhere(Enum fieldName, object value, ConditionEnum cdt = ConditionEnum.Equal, OperatorEnum opr = OperatorEnum.And)
        {
            AddWhere(fieldName.PackString(), value, cdt, opr);
            dynamic tableInfo = DALAttrHelper.GetTableNameByFields(fieldName);
            GetDataBaseTypeByFields(fieldName);
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = tableInfo.DataBaseName;
            if (string.IsNullOrWhiteSpace(tableName)) tableName = tableInfo.TableName;
        }
        /// <summary>
        /// 转义SQL查询字符串中的特殊字符
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>转义后字符串</returns>
        private string PackSqlSearchMysqlStr(string value)
        {
            return value.Replace("[", "\\[")
                .Replace("%", "\\%")
                .Replace("_", "\\_");
        }

        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="fieldName">列名</param>
        /// <param name="value">查询值</param>
        /// <param name="cdt">查询操作关系</param>
        /// <param name="opr">查询操作类型</param>
        public void AddWhere(string fieldName, object value, ConditionEnum cdt = ConditionEnum.Equal, OperatorEnum opr = OperatorEnum.And)
        {
            List<ConditionEnum> needUseSearchStr = new List<ConditionEnum> { ConditionEnum.Like, ConditionEnum.LeftLike, ConditionEnum.RightLike, ConditionEnum.FuzzyLike };
            if (fieldName.IsNullOrEmpty()) return;
            if (value == null || value == DBNull.Value) return;
            if (needUseSearchStr.Contains(cdt) && typeof(string) == value.GetType())
            {
                if ((string)value == string.Empty) return;
                value = this.DataBaseType == DataBaseType.SQLServer ? ((string)value).PackSqlSearchStr() : PackSqlSearchMysqlStr((string)value);
            }
            if (typeof(int) == value.GetType() && (int)value == int.MinValue) return;
            if (typeof(DateTime) == value.GetType() && (DateTime)value <= new DateTime(2000, 1, 1)) return;
            if (typeof(decimal) == value.GetType() && (decimal)value == 0m) return;
            if (typeof(float) == value.GetType() && (float)value == float.MinValue) return;
            if (typeof(long) == value.GetType() && (long)value == long.MinValue) return;
            if (typeof(char) == value.GetType() && (char)value == char.MaxValue) return;
            if (typeof(Single) == value.GetType() && (Single)value == Single.MinValue) return;
            if (typeof(byte) == value.GetType() && (byte)value == byte.MaxValue) return;
            if (typeof(Guid) == value.GetType() && (Guid)value == Guid.Empty) return;
            if (typeof(TimeSpan) == value.GetType() && (TimeSpan)value == TimeSpan.Zero) return;
            wheres.Add(new ParamValue
            {
                fieldName = fieldName,
                value = value,
                cdt = cdt,
                opr = opr
            });
        }
        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="SqlStr">查询SQL *非int型必须用参数化查询防注入</param>
        public void AddWhere(string SqlStr)
        {
            AddWhere(SqlStr, null);
        }
        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="SqlStr">查询SQL *非int型必须用参数化查询防注入</param>
        /// <param name="Parameter">查询参数</param>
        public void AddWhere(string SqlStr, KeyValuePair<string, object> Parameter)
        {
            wheres.Add(new ParamValue
            {
                isSql = true,
                SqlStr = SqlStr
            });
            if (!Params.ContainsKey(Parameter.Key)) Params.Add(Parameter.Key, Parameter.Value);
        }
        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="SqlStr">查询SQL *非int型必须用参数化查询防注入</param>
        /// <param name="Parameters">查询参数</param>
        public void AddWhere(string SqlStr, Dictionary<string, object> Parameters)
        {
            wheres.Add(new ParamValue
            {
                isSql = true,
                SqlStr = SqlStr
            });
            if (Parameters != null)
            {
                foreach (var item in Parameters)
                {
                    if (!Params.ContainsKey(item.Key)) Params.Add(item.Key, item.Value);
                }
            }
        }
        #endregion

        #region 添加表关联
        /// <summary>
        /// 添加表关联 *Update不支持Join
        /// </summary>
        /// <param name="joinString">关联字符串</param>
        public void AddJoin(string joinString)
        {
            if (string.IsNullOrWhiteSpace(joinString)) return;
            joins.Add(new JoinParam { joinStr = joinString });
        }
        /// <summary>
        /// 添加表关联 *Update不支持Join
        /// </summary>
        /// <param name="joinType">关联类型</param>
        /// <param name="mainTableField">主表关联字段</param>
        /// <param name="mainTableAlias">主表别名</param>
        /// <param name="joinTableField">联表关联字段 *该字段所属的表将被用作关联表</param>
        /// <param name="joinTableAlias">联表别名</param>
        public void AddJoin(JoinEnum joinType, Enum mainTableField, string mainTableAlias, Enum joinTableField, string joinTableAlias = "")
        {
            if (mainTableField == null || joinTableField == null) return;
            alias = mainTableAlias;
            joins.Add(new JoinParam
            {
                joinType = joinType,
                mainTableField = mainTableField,
                mainTableAlias = mainTableAlias,
                joinTableField = joinTableField,
                joinTableAlias = joinTableAlias
            });
        }
        /// <summary>
        /// 添加表关联 *Update不支持Join
        /// </summary>
        /// <param name="joinType">关联类型</param>
        /// <param name="mainTableField">主表关联字段</param>
        /// <param name="joinTableField">联表关联字段 *该字段所属的表将被用作关联表</param>
        /// <param name="joinTableAlias">联表别名</param>
        public void AddJoin(JoinEnum joinType, Enum mainTableField, Enum joinTableField, string joinTableAlias = "")
        {
            if (mainTableField == null || joinTableField == null) return;
            joins.Add(new JoinParam
            {
                joinType = joinType,
                mainTableField = mainTableField,
                joinTableField = joinTableField,
                joinTableAlias = joinTableAlias
            });
        }
        #endregion

        #region 添加排序
        /// <summary>
        /// 添加排序
        /// </summary>
        /// <param name="fieldName">排序字段</param>
        /// <param name="orderEnum">排序枚举</param>
        public void AddOrder(Enum fieldName, OrderEnum orderEnum = OrderEnum.Asc)
        {
            AddOrder(fieldName.PackString(), orderEnum);
            dynamic tableInfo = DALAttrHelper.GetTableNameByFields(fieldName);
            GetDataBaseTypeByFields(fieldName);
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = tableInfo.DataBaseName;
            if (string.IsNullOrWhiteSpace(tableName)) tableName = tableInfo.TableName;
        }
        /// <summary>
        /// 添加排序
        /// </summary>
        /// <param name="fieldName">排序字段</param>
        /// <param name="orderEnum">排序枚举</param>
        public void AddOrder(string fieldName, OrderEnum orderEnum = OrderEnum.Asc)
        {
            orders.Add(new OrderParam
            {
                orderField = fieldName,
                orderEnum = orderEnum
            });
        }
        #endregion

        #region 添加更新参数
        /// <summary>
        /// 添加更新参数
        /// </summary>
        /// <param name="fieldName">列名</param>
        /// <param name="value">查询值</param>
        /// <param name="isChangeSave">是否在其它更新有变更时才更新</param>
        public void AddUpdate(Enum fieldName, object value, bool isChangeSave = false)
        {
            var obj = fieldName.GetType().GetCustomAttributes(typeof(ReadOnlyAttribute), false)?.FirstOrDefault();
            if (obj != null && (obj as ReadOnlyAttribute).IsReadOnly) return;
            AddUpdate(fieldName.PackString(), value, isChangeSave);
            fieldsEmum = fieldName.GetType();
            dynamic tableInfo = DALAttrHelper.GetTableNameByFields(fieldName);
            GetDataBaseTypeByFields(fieldName);
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = tableInfo.DataBaseName;
            if (string.IsNullOrWhiteSpace(tableName)) tableName = tableInfo.TableName;
        }
        /// <summary>
        /// 添加更新参数
        /// </summary>
        /// <param name="fieldName">列名</param>
        /// <param name="value">查询值</param>
        /// <param name="isChangeSave">是否在其它更新有变更时才更新</param>
        public void AddUpdate(string fieldName, object value, bool isChangeSave = false)
        {
            if (fieldName.IsNullOrEmpty()) return;
            if (value == null || value == DBNull.Value) return;
            updates.Add(new ParamValue
            {
                isSql = isChangeSave,
                fieldName = fieldName,
                value = value
            });
        }
        #endregion

       

        #region 查
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="id">主键</param>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <returns>实体</returns>
        public static T GetEntityById<T>(dynamic id, bool useWriteDataBase = false, string dataBaseName = "") where T : class
        {
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
            var conn = GetSqlConnection(dataBaseName, useWriteDataBase);
            var val = default(T);
            try
            {
                val = SqlMapperExtensions.Get<T>(conn, id);
            }
            finally { conn?.Dispose(); }
            return val;
        }

        /// <summary>
        /// 根据主键获取实体 异步
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="id">主键</param>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <returns>实体</returns>
        public static async Task<T> GetEntityByIdAsync<T>(dynamic id, bool useWriteDataBase = false, string dataBaseName = "") where T : class
        {
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
            var conn = GetSqlConnection(dataBaseName, useWriteDataBase);
            var val = default(T);
            try
            {
                val = await SqlMapperExtensions.GetAsync<T>(conn, id);
            }
            finally { conn?.Dispose(); }
            return val;
        }

        /// <summary>
        /// 根据分片键获取实体列表
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">包含分片键的实体</param>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <returns>实体</returns>
        public static IEnumerable<T> GetEntityByShardKeys<T>(T entity, bool useWriteDataBase = false, string dataBaseName = "") where T : class
        {
            CommonORM cdal = new CommonORM();
            if (string.IsNullOrWhiteSpace(dataBaseName)) cdal.dataBaseName = DALAttrHelper.GetDataBase(entity);
            else cdal.dataBaseName = dataBaseName;
            cdal.tableName = DALAttrHelper.GetTableName(entity);
            foreach (PropertyInfo field in typeof(T).GetProperties())
            {
                cdal.AddShow(field.Name);
                ShardKeyAttribute da = null;
                ExplicitKeyAttribute ke = null;
                object[] arrobj = field.GetCustomAttributes(typeof(ShardKeyAttribute), true);
                if (arrobj.Length > 0) da = arrobj[0] as ShardKeyAttribute;
                object[] keyobj = field.GetCustomAttributes(typeof(ExplicitKeyAttribute), true);
                if (keyobj.Length > 0) ke = keyobj[0] as ExplicitKeyAttribute;
                if (da != null) cdal.AddWhere(field.Name, field.GetValue(entity, null));
                else if (ke != null)
                {
                    var fieldValue = field.GetValue(entity, null);
                    if (fieldValue != null && fieldValue != (object)0 && fieldValue != (object)"") cdal.AddWhere(field.Name, field.GetValue(entity, null));
                }
            }
            return cdal.GetList<T>(useWriteDataBase);
        }

        /// <summary>
        /// 根据分片键获取实体列表 异步
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">包含分片键的实体</param>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <returns>实体</returns>
        public static async Task<IEnumerable<T>> GetEntityByShardKeysAsync<T>(T entity, bool useWriteDataBase = false, string dataBaseName = "") where T : class
        {
            CommonORM cdal = new CommonORM();
            if (string.IsNullOrWhiteSpace(dataBaseName)) cdal.dataBaseName = DALAttrHelper.GetDataBase(entity);
            else cdal.dataBaseName = dataBaseName;
            cdal.tableName = DALAttrHelper.GetTableName(entity);
            foreach (PropertyInfo field in typeof(T).GetProperties())
            {
                cdal.AddShow(field.Name);
                ShardKeyAttribute da = null;
                ExplicitKeyAttribute ke = null;
                object[] arrobj = field.GetCustomAttributes(typeof(ShardKeyAttribute), true);
                if (arrobj.Length > 0) da = arrobj[0] as ShardKeyAttribute;
                object[] keyobj = field.GetCustomAttributes(typeof(ExplicitKeyAttribute), true);
                if (keyobj.Length > 0) ke = keyobj[0] as ExplicitKeyAttribute;
                if (da != null) cdal.AddWhere(field.Name, field.GetValue(entity, null));
                else if (ke != null)
                {
                    var fieldValue = field.GetValue(entity, null);
                    if (fieldValue != null && fieldValue != (object)0 && fieldValue != (object)"") cdal.AddWhere(field.Name, field.GetValue(entity, null));
                }
            }
            return await cdal.GetListAsync<T>(useWriteDataBase);
        }

        /// <summary>
        /// 获取查询结果
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <returns>数据列表</returns>
        public IEnumerable<T> GetList<T>(bool useWriteDataBase) where T : class
        {
            return GetList<T>(0, useWriteDataBase);
        }

        /// <summary>
        /// 获取查询结果 异步
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <returns>数据列表</returns>
        public async Task<IEnumerable<T>> GetListAsync<T>(bool useWriteDataBase) where T : class
        {
            return await GetListAsync<T>(0, useWriteDataBase);
        }

        /// <summary>
        /// 获取查询结果
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="top">Top条数 *默认0查全部</param>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <returns>数据列表</returns>
        public IEnumerable<T> GetList<T>(int top = 0, bool useWriteDataBase = false) where T : class
        {
            #region 验证参数
            if (string.IsNullOrWhiteSpace(dataBaseName) || string.IsNullOrWhiteSpace(tableName))
            {
                if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
                var obj = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
                if (obj != null)
                {
                    var ma = (TableAttribute)obj;
                    tableName = ma.Name;
                }
                var shareKeys = DALAttrHelper.GetShardKeys(typeof(T));
                if (shareKeys != null && shareKeys.Any())
                {
                    foreach (string keyStr in shareKeys)
                    {
                        if (!wheres.Any(t => t.fieldName == keyStr || t.fieldName?.Substring((t.fieldName?.IndexOf('.') ?? 0) < 0 ? 0 : (t.fieldName?.IndexOf('.') ?? 0)) == keyStr))
                        {
                            throw new Exception($"请传入分片键{keyStr}的查询条件 *AddWhere");
                        }
                    }
                }
            }
            GetDataBaseType<T>();
            if (string.IsNullOrWhiteSpace(primaryKey)) primaryKey = DALAttrHelper.GetKey(typeof(T));
            if (string.IsNullOrWhiteSpace(notes)) new Exception("必须添加注释 *AddNotes");
            if (shows == null || shows.Count < 1) throw new Exception("必须输入查询列表 *AddShow");
            if (string.IsNullOrWhiteSpace(dataBaseName) || string.IsNullOrWhiteSpace(tableName)) throw new Exception("必须传入库名与表名 *dataBaseName *tableName");
            if (pageSize > 0 && orders.Count < 1 && string.IsNullOrWhiteSpace(primaryKey) && !UnOrder) throw new Exception("分页必须传入排序字段或主键 *AddOrder");
            if (joins.Count < 1 && useTableList.Count > 0)
            {
                string tbName = tableName.ToLower();
                foreach (string item in useTableList)
                {
                    if (!string.IsNullOrWhiteSpace(item) && item.ToLower() != tbName) throw new Exception("传入的查询字段与使用的数据访问类不匹配 *AddShow");
                }
            }
            if (!hasReadDataBase) throw new Exception("请传入数据库类型 *DataBaseType");
            #endregion

            StringBuilder showStr = new StringBuilder();
            StringBuilder orderStr = new StringBuilder();

            #region 查询列SQL拼接
            for (int i = 0, len = shows.Count; i < len; i++)
            {
                if (string.IsNullOrWhiteSpace(shows[i]) || shows[i].Trim() == string.Empty) continue;
                showStr.AppendFormat("{0},", shows[i].Trim(','));
            }
            #endregion

            #region 排序SQL拼接
            if (orders != null && orders.Count > 0)
            {
                orderStr.Append(" order by ");
                for (int i = 0, len = orders.Count; i < len; i++)
                {
                    orderStr.AppendFormat(" {0} {1}, ", orders[i].orderField, orders[i].orderEnum.PackString());
                }
            }
            else if (!UnOrder && !string.IsNullOrWhiteSpace(primaryKey)) orderStr.AppendFormat(" order by {0} ", primaryKey);
            #endregion

            using (DbConnection conn = GetSqlConnection(dataBaseName, useWriteDataBase))
            {
                sql.Clear();
                sql.Append(notes);
                if (!IsExport && pageSize > 0)//分页
                {
                    string whereStr = GetWhereStr();
                    switch (this.DataBaseType)
                    {
                        case DataBaseType.MySQL:
                            sql.Append(new TranslatorForMYSQL().FuseSearch(1, "COUNT(1) ", tableName, alias, GetMySqlJoinStr(), whereStr, string.Empty));
                            totalCount = conn.ExecuteScalar<int>(sql.ToString(), Params).PackInt();
                            sql.Clear();
                            sql.Append(notes);
                            sql.Append(new TranslatorForMYSQL().FusePagination(showStr.ToString().Trim().Trim(','), tableName, alias, GetMySqlJoinStr(), whereStr
                            , (pageIndex - 1) * pageSize, pageSize, orderStr.ToString().Trim().Trim(',')));
                            break;
                        case DataBaseType.SQLServer:
                        default:
                            sql.AppendFormat(@"SELECT COUNT(1) FROM [{0}] {1} WITH(NOLOCK) {2} WHERE 1=1 {3};", tableName, alias, GetJoinStr(), whereStr);
                            totalCount = conn.ExecuteScalar<int>(sql.ToString(), Params).PackInt();
                            sql.Clear();
                            sql.Append(notes);
                            sql.AppendFormat(@"SELECT * FROM (SELECT {0},ROW_NUMBER() OVER ({1}) AS rowNumber FROM [{2}] {3} WITH(NOLOCK) {4} WHERE 1=1 {5})t WHERE t.rowNumber BETWEEN {6} AND {7}"
                            , showStr.ToString().Trim().Trim(','), orderStr.ToString().Trim().Trim(','), tableName, alias, GetJoinStr(), whereStr
                            , (pageIndex - 1) * pageSize + 1, pageIndex * pageSize, alias, GetJoinStr());
                            break;
                    }
                }
                else//不分页、TOP查询
                {
                    switch (this.DataBaseType)
                    {
                        case DataBaseType.MySQL:
                            sql.Append(new TranslatorForMYSQL().FuseSearch(top, showStr.ToString().Trim().Trim(',')
                            , tableName, alias, GetMySqlJoinStr(), GetWhereStr(), orderStr.ToString().Trim().Trim(','))); break;
                        case DataBaseType.SQLServer:
                        default:
                            sql.AppendFormat("select {0}{1} from [{2}] {3} with(nolock) {4} where 1=1 {5} {6}"
                            , top > 0 ? string.Format("top {0} ", top) : string.Empty, showStr.ToString().Trim().Trim(',')
                            , tableName, alias, GetJoinStr(), GetWhereStr(), orderStr.ToString().Trim().Trim(','));
                            break;
                    }
                }
                IEnumerable<T> val = new List<T>();
                try { val = conn.Query<T>(sql.ToString(), Params); }
                catch (Exception e)
                {
                    sql.AppendLine(e.Message);
                    sql.AppendLine(e.StackTrace);
                }
                finally { conn?.Dispose(); }
                return val;
            }
        }

        /// <summary>
        /// 获取查询结果 异步
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="top">Top条数 *默认0查全部</param>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <returns>数据列表</returns>
        public async Task<IEnumerable<T>> GetListAsync<T>(int top = 0, bool useWriteDataBase = false) where T : class
        {
            #region 验证参数
            if (string.IsNullOrWhiteSpace(dataBaseName) || string.IsNullOrWhiteSpace(tableName))
            {
                if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
                var obj = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
                if (obj != null)
                {
                    var ma = (TableAttribute)obj;
                    tableName = ma.Name;
                }
                var shareKeys = DALAttrHelper.GetShardKeys(typeof(T));
                if (shareKeys != null && shareKeys.Any())
                {
                    foreach (string keyStr in shareKeys)
                    {
                        if (!wheres.Any(t => t.fieldName == keyStr || t.fieldName?.Substring((t.fieldName?.IndexOf('.') ?? 0) < 0 ? 0 : (t.fieldName?.IndexOf('.') ?? 0)) == keyStr)) throw new Exception($"请传入分片键{keyStr}的查询条件 *AddWhere");
                    }
                }
            }
            GetDataBaseType<T>();
            if (string.IsNullOrWhiteSpace(primaryKey)) primaryKey = DALAttrHelper.GetKey(typeof(T));
            if (string.IsNullOrWhiteSpace(notes)) new Exception("必须添加注释 *AddNotes");
            if (shows == null || shows.Count < 1) throw new Exception("必须输入查询列表 *AddShow");
            if (string.IsNullOrWhiteSpace(dataBaseName) || string.IsNullOrWhiteSpace(tableName)) throw new Exception("必须传入库名与表名 *dataBaseName *tableName");
            if (pageSize > 0 && orders.Count < 1 && string.IsNullOrWhiteSpace(primaryKey) && !UnOrder) throw new Exception("分页必须传入排序字段或主键 *AddOrder");
            if (joins.Count < 1 && useTableList.Count > 0)
            {
                string tbName = tableName.ToLower();
                foreach (string item in useTableList)
                {
                    if (!string.IsNullOrWhiteSpace(item) && item.ToLower() != tbName) throw new Exception("传入的查询字段与使用的数据访问类不匹配 *AddShow");
                }
            }
            if (!hasReadDataBase) throw new Exception("请传入数据库类型 *DataBaseType");
            #endregion

            StringBuilder showStr = new StringBuilder();
            StringBuilder orderStr = new StringBuilder();

            #region 查询列SQL拼接
            for (int i = 0, len = shows.Count; i < len; i++)
            {
                if (string.IsNullOrWhiteSpace(shows[i]) || shows[i].Trim() == string.Empty) continue;
                showStr.AppendFormat("{0},", shows[i].Trim(','));
            }
            #endregion

            #region 排序SQL拼接
            if (orders != null && orders.Count > 0)
            {
                orderStr.Append(" order by ");
                for (int i = 0, len = orders.Count; i < len; i++)
                {
                    orderStr.AppendFormat(" {0} {1}, ", orders[i].orderField, orders[i].orderEnum.PackString());
                }
            }
            else if (!UnOrder && !string.IsNullOrWhiteSpace(primaryKey)) orderStr.AppendFormat(" order by {0} ", primaryKey);
            #endregion

            using (DbConnection conn = GetSqlConnection(dataBaseName, useWriteDataBase))
            {
                sql.Clear();
                sql.Append(notes);
                if (!IsExport && pageSize > 0)//分页
                {
                    string whereStr = GetWhereStr();
                    switch (this.DataBaseType)
                    {
                        case DataBaseType.MySQL:
                            sql.Append(new TranslatorForMYSQL().FuseSearch(0, "COUNT(1) ", tableName, alias, GetMySqlJoinStr(), whereStr, string.Empty));
                            totalCount = await conn.ExecuteScalarAsync<int>(sql.ToString(), Params);
                            sql.Clear();
                            sql.Append(notes);
                            sql.Append(new TranslatorForMYSQL().FusePagination(showStr.ToString().Trim().Trim(','), tableName, alias, GetMySqlJoinStr(), whereStr
                            , (pageIndex - 1) * pageSize, pageSize, orderStr.ToString().Trim().Trim(',')));
                            break;
                        case DataBaseType.SQLServer:
                        default:
                            sql.AppendFormat(@"SELECT COUNT(1) FROM [{0}] {1} WITH(NOLOCK) {2} WHERE 1=1 {3};", tableName, alias, GetJoinStr(), whereStr);
                            totalCount = await conn.ExecuteScalarAsync<int>(sql.ToString(), Params);
                            sql.Clear();
                            sql.Append(notes);
                            sql.AppendFormat(@"SELECT * FROM (SELECT {0},ROW_NUMBER() OVER ({1}) AS rowNumber FROM [{2}] {3} WITH(NOLOCK) {4} WHERE 1=1 {5})t WHERE t.rowNumber BETWEEN {6} AND {7}"
                            , showStr.ToString().Trim().Trim(','), orderStr.ToString().Trim().Trim(','), tableName, alias, GetJoinStr(), whereStr
                            , (pageIndex - 1) * pageSize + 1, pageIndex * pageSize, alias, GetJoinStr());
                            break;
                    }
                }
                else//不分页、TOP查询
                {
                    switch (this.DataBaseType)
                    {
                        case DataBaseType.MySQL:
                            sql.Append(new TranslatorForMYSQL().FuseSearch(top, showStr.ToString().Trim().Trim(',')
                            , tableName, alias, GetMySqlJoinStr(), GetWhereStr(), orderStr.ToString().Trim().Trim(','))); break;
                        case DataBaseType.SQLServer:
                        default:
                            sql.AppendFormat("select {0}{1} from [{2}] {3} with(nolock) {4} where 1=1 {5} {6}"
                            , top > 0 ? string.Format("top {0} ", top) : string.Empty, showStr.ToString().Trim().Trim(',')
                            , tableName, alias, GetJoinStr(), GetWhereStr(), orderStr.ToString().Trim().Trim(','));
                            break;
                    }
                }
                IEnumerable<T> val = new List<T>();
                try { val = await conn.QueryAsync<T>(sql.ToString(), Params); }
                catch (Exception e)
                {
                    sql.AppendLine(e.Message);
                    sql.AppendLine(e.StackTrace);
                }
                finally { conn?.Dispose(); }

                return val;
            }

        }

        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <returns>true:有数据  false:无数据</returns>
        public bool IsExists(bool useWriteDataBase = false)
        {
            #region 验证参数
            if (string.IsNullOrWhiteSpace(notes)) new Exception("必须添加注释 *AddNotes");
            if (string.IsNullOrWhiteSpace(dataBaseName) || string.IsNullOrWhiteSpace(tableName)) throw new Exception("必须传入库名与表名 *dataBaseName *tableName");
            if (wheres == null || wheres.Count < 1) throw new Exception("必须传入至少一个查询条件 *AddWhere");
            #endregion

            sql.Clear();
            sql.Append(notes);
            switch (this.DataBaseType)
            {
                case DataBaseType.MySQL:
                    sql.Append(new TranslatorForMYSQL().FuseSearch(1, "1 ", tableName, alias, GetMySqlJoinStr(), GetWhereStr(), string.Empty)); break;
                case DataBaseType.SQLServer:
                default: sql.AppendFormat("SELECT TOP 1 1 FROM [{0}] {1} {2} WHERE 1=1 {3}", tableName, alias, GetJoinStr(), GetWhereStr()); break;
            }
            DbConnection conn = GetSqlConnection(dataBaseName, useWriteDataBase);
            bool val = true;
            try { val = conn.ExecuteScalar<int>(sql.ToString(), Params).PackInt() > 0; }
            catch (Exception e)
            {
                val = false;
                sql.AppendLine(e.Message);
                sql.AppendLine(e.StackTrace);
            }
            finally { conn?.Dispose(); }
            return val;
        }

        /// <summary>
        /// 是否存在数据 异步
        /// </summary>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <returns>true:有数据  false:无数据</returns>
        public async Task<bool> IsExistsAsync(bool useWriteDataBase = false)
        {
            #region 验证参数
            if (string.IsNullOrWhiteSpace(notes)) new Exception("必须添加注释 *AddNotes");
            if (string.IsNullOrWhiteSpace(dataBaseName) || string.IsNullOrWhiteSpace(tableName)) throw new Exception("必须传入库名与表名 *dataBaseName *tableName");
            if (wheres == null || wheres.Count < 1) throw new Exception("必须传入至少一个查询条件 *AddWhere");
            #endregion

            sql.Clear();
            sql.Append(notes);
            switch (this.DataBaseType)
            {
                case DataBaseType.MySQL:
                    sql.Append(new TranslatorForMYSQL().FuseSearch(1, "1 ", tableName, alias, GetMySqlJoinStr(), GetWhereStr(), string.Empty)); break;
                case DataBaseType.SQLServer:
                default: sql.AppendFormat("SELECT TOP 1 1 FROM [{0}] {1} {2} WHERE 1=1 {3}", tableName, alias, GetJoinStr(), GetWhereStr()); break;
            }
            DbConnection conn = GetSqlConnection(dataBaseName, useWriteDataBase);
            bool val = true;
            try { val = await conn.ExecuteScalarAsync<int>(sql.ToString(), Params) > 0; }
            catch (Exception e)
            {
                val = false;
                sql.AppendLine(e.Message);
                sql.AppendLine(e.StackTrace);
            }
            finally { conn?.Dispose(); }
            return val;
        }

        /// <summary>
        /// 获取数据条数
        /// </summary>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <returns>数据条数</returns>
        public int GetCount(bool useWriteDataBase = false)
        {

            #region 验证参数
            if (string.IsNullOrWhiteSpace(notes)) new Exception("必须添加注释 *AddNotes");
            if (string.IsNullOrWhiteSpace(dataBaseName) || string.IsNullOrWhiteSpace(tableName)) throw new Exception("必须传入库名与表名 *dataBaseName *tableName");
            if (wheres == null || wheres.Count < 1) throw new Exception("必须传入至少一个查询条件 *AddWhere");
            #endregion

            sql.Clear();
            sql.Append(notes);
            switch (this.DataBaseType)
            {
                case DataBaseType.MySQL:
                    sql.Append(new TranslatorForMYSQL().FuseSearch(1, "COUNT(1) ", tableName, alias, GetMySqlJoinStr(), GetWhereStr(), string.Empty)); break;
                case DataBaseType.SQLServer:
                default: sql.AppendFormat("SELECT COUNT(1) FROM [{0}] {1} {2} WHERE 1=1 {3}", tableName, alias, GetJoinStr(), GetWhereStr()); break;
            }
            DbConnection conn = GetSqlConnection(dataBaseName, useWriteDataBase);
            int val = 0;
            try { val = conn.ExecuteScalar<int>(sql.ToString(), Params); }
            catch (Exception e)
            {
                sql.AppendLine(e.Message);
                sql.AppendLine(e.StackTrace);
            }
            finally { conn?.Dispose(); }
            return val;
        }

        /// <summary>
        /// 获取数据条数 异步
        /// </summary>
        /// <param name="useWriteDataBase">是否查写库 *默认查读库</param>
        /// <returns>数据条数</returns>
        public async Task<int> GetCountAsync(bool useWriteDataBase = false)
        {

            #region 验证参数
            if (string.IsNullOrWhiteSpace(notes)) new Exception("必须添加注释 *AddNotes");
            if (string.IsNullOrWhiteSpace(dataBaseName) || string.IsNullOrWhiteSpace(tableName)) throw new Exception("必须传入库名与表名 *dataBaseName *tableName");
            if (wheres == null || wheres.Count < 1) throw new Exception("必须传入至少一个查询条件 *AddWhere");
            #endregion

            sql.Clear();
            sql.Append(notes);
            switch (this.DataBaseType)
            {
                case DataBaseType.MySQL:
                    sql.Append(new TranslatorForMYSQL().FuseSearch(1, "COUNT(1) ", tableName, alias, GetMySqlJoinStr(), GetWhereStr(), string.Empty)); break;
                case DataBaseType.SQLServer:
                default: sql.AppendFormat("SELECT COUNT(1) FROM [{0}] {1} {2} WHERE 1=1 {3}", tableName, alias, GetJoinStr(), GetWhereStr()); break;
            }
            DbConnection conn = GetSqlConnection(dataBaseName, useWriteDataBase);
            int val = 0;
            try { val = await conn.ExecuteScalarAsync<int>(sql.ToString(), Params); }
            catch (Exception e)
            {
                sql.AppendLine(e.Message);
                sql.AppendLine(e.StackTrace);
            }
            finally { conn?.Dispose(); }
            return val;
        }
        #endregion

        #region 改
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="top">更新条数</param>
        /// <returns>是否更新成功</returns>
        public bool Update(int top = 0)
        {
            return Update(false, top);
        }

        /// <summary>
        /// 更新数据 异步
        /// </summary>
        /// <param name="top">更新条数</param>
        /// <returns>是否更新成功</returns>
        public async Task<bool> UpdateAsync(int top = 0)
        {
            return await UpdateAsync(false, top);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="needChanged">是否需要获取更新前后的值</param>
        /// <param name="top">更新条数</param>
        /// <returns>变更值列表</returns>
        public bool Update(bool needChanged, int top = 0)
        {
            #region 验证参数
            if (string.IsNullOrWhiteSpace(notes)) new Exception("必须添加注释 *AddNotes");
            if (string.IsNullOrWhiteSpace(dataBaseName) || string.IsNullOrWhiteSpace(tableName)) throw new Exception("必须传入库名与表名 *dataBaseName *tableName");
            if (updates == null || updates.Count < 1) throw new Exception("必须传入至少一个更新参数 *AddUpdate");
            if (wheres == null || wheres.Count < 1) throw new Exception("必须传入至少一个查询条件 *AddWhere");
            if (needChanged && updates.Count(t => !t.isSql) < 1) throw new Exception("至少一个参数先判断更新 *isChangeSave");
            if (needChanged && fieldsEmum == null) throw new Exception("必须传入字段枚举类型 *FieldsEmum");
            #endregion

            sql.Clear();
            StringBuilder showStr = new StringBuilder();
            StringBuilder updateStr = new StringBuilder();
            string whereStr = GetWhereStr();
            using (DbConnection conn = GetSqlConnection(dataBaseName, true))
            {
                #region 获取原字段值
                if (needChanged)
                {
                    var tmpShows = from t in updates where !t.isSql select t;
                    foreach (var tmp in tmpShows) showStr.AppendFormat("{0},", tmp.fieldName);
                    sql.Append(notes);
                    switch (this.DataBaseType)
                    {
                        case DataBaseType.MySQL:
                            sql.Append(new TranslatorForMYSQL().FuseSearch(1, showStr.ToString().Trim().Trim(','), tableName, alias, string.Empty, whereStr, string.Empty)); break;
                        case DataBaseType.SQLServer:
                        default:
                            sql.AppendFormat("SELECT TOP 1 {0} FROM [{1}] {2} WHERE 1=1 {3}", showStr.ToString().Trim().Trim(','), tableName, alias, whereStr);
                            break;
                    }
                    try
                    {
                        List<dynamic> list = conn.Query<dynamic>(sql.ToString(), Params).ToList();

                        if (list == null || list.Count < 1) return false;
                        foreach (var tmp in tmpShows)
                        {
                            IDictionary<string, object> valueDic = list[0];
                            object fieldValue = valueDic[tmp.fieldName];
                            bool isNull = tmp.value != null && fieldValue == DBNull.Value;
                            bool isNotEqual = false;
                            Type t = fieldValue.GetType();
                            if (t == typeof(string) || t == typeof(char)) isNotEqual = !string.Equals(fieldValue.PackString(), tmp.value.PackString());
                            else if (t == typeof(int)) isNotEqual = !int.Equals(fieldValue, tmp.value.PackInt());
                            else if (t == typeof(long)) isNotEqual = !long.Equals(fieldValue, tmp.value.PackLong());
                            else if (t == typeof(decimal) || t == typeof(float)) isNotEqual = !decimal.Equals(fieldValue.PackDecimal(28), tmp.value.PackDecimal(28));
                            else if (t == typeof(DateTime)) isNotEqual = !DateTime.Equals(fieldValue, tmp.value.PackDateTime());
                            else isNotEqual = !object.Equals(tmp.value, fieldValue);
                            if (isNull || isNotEqual)
                                ChangedValues.Add(new ChangedValues
                                {
                                    fieldName = tmp.fieldName,
                                    remark = GetCNameFromFieldName(tmp.fieldName),
                                    oldValue = fieldValue,
                                    newValue = tmp.value
                                });
                        }
                        if (ChangedValues.Count < 1) return true;//无变化
                    }

                    catch (Exception e)
                    {
                        conn?.Dispose();
                        sql.AppendLine(e.Message);
                        sql.AppendLine(e.StackTrace);
                        return false;
                    }

                }
                #endregion
                switch (this.DataBaseType)
                {
                    case DataBaseType.MySQL:
                        sql.Clear();
                        new TranslatorForMYSQL().FuseUpdata(ref updateStr, updates, Params, notes, ref sql, top, tableName, whereStr);
                        break;
                    case DataBaseType.SQLServer:
                    default:
                        #region 更新参数SQL拼接
                        for (int i = 0, len = updates.Count; i < len; i++)
                        {
                            updateStr.AppendFormat("[{0}]=@UpdateValue{1},", updates[i].fieldName, i);
                            Params.Add(string.Format("UpdateValue{0}", i), updates[i].value);
                        }
                        #endregion

                        sql.Clear();
                        sql.Append(notes);
                        sql.AppendFormat("UPDATE {0} [{1}] SET {2} WHERE 1=1 {3}"
                        , top > 0 ? string.Format("TOP({0})", top) : string.Empty, tableName, updateStr.ToString().Trim().Trim(','), whereStr);
                        break;
                }
                bool val = true;
                try { val = conn.Execute(sql.ToString(), Params) > 0; }
                catch (Exception e)
                {
                    val = false;
                    sql.AppendLine(e.Message);
                    sql.AppendLine(e.StackTrace);
                }
                finally { conn?.Dispose(); }
                return val;
            }
        }

        /// <summary>
        /// 更新数据 异步
        /// </summary>
        /// <param name="needChanged">是否需要获取更新前后的值</param>
        /// <param name="top">更新条数</param>
        /// <returns>变更值列表</returns>
        public async Task<bool> UpdateAsync(bool needChanged, int top = 0)
        {
            #region 验证参数
            if (string.IsNullOrWhiteSpace(notes)) new Exception("必须添加注释 *AddNotes");
            if (string.IsNullOrWhiteSpace(dataBaseName) || string.IsNullOrWhiteSpace(tableName)) throw new Exception("必须传入库名与表名 *dataBaseName *tableName");
            if (updates == null || updates.Count < 1) throw new Exception("必须传入至少一个更新参数 *AddUpdate");
            if (wheres == null || wheres.Count < 1) throw new Exception("必须传入至少一个查询条件 *AddWhere");
            if (needChanged && updates.Count(t => !t.isSql) < 1) throw new Exception("至少一个参数先判断更新 *isChangeSave");
            if (needChanged && fieldsEmum == null) throw new Exception("必须传入字段枚举类型 *FieldsEmum");
            #endregion

            sql.Clear();
            StringBuilder showStr = new StringBuilder();
            StringBuilder updateStr = new StringBuilder();
            string whereStr = GetWhereStr();
            using (DbConnection conn = GetSqlConnection(dataBaseName, true))
            {
                #region 获取原字段值
                if (needChanged)
                {
                    var tmpShows = from t in updates where !t.isSql select t;
                    foreach (var tmp in tmpShows) showStr.AppendFormat("{0},", tmp.fieldName);
                    sql.Append(notes);
                    switch (this.DataBaseType)
                    {
                        case DataBaseType.MySQL:
                            sql.Append(new TranslatorForMYSQL().FuseSearch(1, showStr.ToString().Trim().Trim(','), tableName, alias, string.Empty, whereStr, string.Empty)); break;
                        case DataBaseType.SQLServer:
                        default:
                            sql.AppendFormat("SELECT TOP 1 {0} FROM [{1}] {2} WHERE 1=1 {3}", showStr.ToString().Trim().Trim(','), tableName, alias, whereStr);
                            break;
                    }
                    //List<dynamic> list = conn.Query<dynamic>(sql.ToString(), Params).ToList();
                    try
                    {
                        List<dynamic> list = conn.Query<dynamic>(sql.ToString(), Params).ToList();
                        if (list == null || list.Count < 1) return false;
                        foreach (var tmp in tmpShows)
                        {
                            IDictionary<string, object> valueDic = list[0];
                            object fieldValue = valueDic[tmp.fieldName];
                            bool isNull = tmp.value != null && fieldValue == DBNull.Value;
                            bool isNotEqual = false;
                            Type t = fieldValue.GetType();
                            if (t == typeof(string) || t == typeof(char)) isNotEqual = !string.Equals(fieldValue.PackString(), tmp.value.PackString());
                            else if (t == typeof(int)) isNotEqual = !int.Equals(fieldValue, tmp.value.PackInt());
                            else if (t == typeof(long)) isNotEqual = !long.Equals(fieldValue, tmp.value.PackLong());
                            else if (t == typeof(decimal) || t == typeof(float)) isNotEqual = !decimal.Equals(fieldValue.PackDecimal(28), tmp.value.PackDecimal(28));
                            else if (t == typeof(DateTime)) isNotEqual = !DateTime.Equals(fieldValue, tmp.value.PackDateTime());
                            else isNotEqual = !object.Equals(tmp.value, fieldValue);
                            if (isNull || isNotEqual)
                                ChangedValues.Add(new ChangedValues
                                {
                                    fieldName = tmp.fieldName,
                                    remark = GetCNameFromFieldName(tmp.fieldName),
                                    oldValue = fieldValue,
                                    newValue = tmp.value
                                });
                        }
                        if (ChangedValues.Count < 1) return true;//无变化
                    }
                    catch (Exception e)
                    {
                        conn?.Dispose();
                        sql.AppendLine(e.Message);
                        sql.AppendLine(e.StackTrace);
                        return false;
                    }
                }
                #endregion
                switch (this.DataBaseType)
                {
                    case DataBaseType.MySQL:
                        sql.Clear();
                        new TranslatorForMYSQL().FuseUpdata(ref updateStr, updates, Params, notes, ref sql, top, tableName, whereStr);
                        break;
                    case DataBaseType.SQLServer:
                    default:
                        #region 更新参数SQL拼接
                        for (int i = 0, len = updates.Count; i < len; i++)
                        {
                            updateStr.AppendFormat("[{0}]=@UpdateValue{1},", updates[i].fieldName, i);
                            Params.Add(string.Format("UpdateValue{0}", i), updates[i].value);
                        }
                        #endregion

                        sql.Clear();
                        sql.Append(notes);
                        sql.AppendFormat("UPDATE {0} [{1}] SET {2} WHERE 1=1 {3}"
                        , top > 0 ? string.Format("TOP({0})", top) : string.Empty, tableName, updateStr.ToString().Trim().Trim(','), whereStr);
                        break;
                }
                bool val = true;
                try { val = await conn.ExecuteAsync(sql.ToString(), Params) > 0; }
                catch (Exception e)
                {
                    val = false;
                    sql.AppendLine(e.Message);
                    sql.AppendLine(e.StackTrace);
                    throw e;
                }
                finally { conn?.Dispose(); }
                return val;
            }
        }


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <param name="fieldNames">需要更新的字段</param>
        /// <returns></returns>
        public static bool UpdateByEntity<T>(T entity, string dataBaseName, params Enum[] fieldNames) where T : class
        {
            if (entity == null) return false;
            if (fieldNames == null || fieldNames.Length < 1)
            {
                if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(entity);
                DbConnection conn = GetSqlConnection(dataBaseName, true);
                bool val = true;
                try { val = conn.Update(entity); }
                catch { val = false; }
                finally { conn?.Dispose(); }
                return val;
            }
            CommonORM cdal = new CommonORM();
            cdal.dataBaseName = dataBaseName;
            foreach (var item in fieldNames)
            {
                cdal.AddUpdate(item, entity.GetPropertyValue(item.PackString()));
            }
            string primaryKey = DALAttrHelper.GetKey(typeof(T));
            cdal.AddWhere(primaryKey, entity.GetPropertyValue(primaryKey));
            return cdal.Update();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="fieldNames">需要更新的字段</param>
        /// <returns></returns>
        public static bool UpdateByEntity<T>(T entity, params Enum[] fieldNames) where T : class
        {
            return UpdateByEntity(entity, string.Empty, fieldNames);
        }

        /// <summary>
        /// 更新数据 异步
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <param name="fieldNames">需要更新的字段</param>
        /// <returns></returns>
        public static async Task<bool> UpdateByEntityAsync<T>(T entity, string dataBaseName, params Enum[] fieldNames) where T : class
        {
            if (entity == null) return false;
            if (fieldNames == null || fieldNames.Length < 1)
            {
                if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(entity);
                DbConnection conn = GetSqlConnection(dataBaseName, true);
                var val = await conn.UpdateAsync(entity);
                conn?.Dispose();
                return val;
            }
            CommonORM cdal = new CommonORM();
            cdal.dataBaseName = dataBaseName;
            foreach (var item in fieldNames)
            {
                cdal.AddUpdate(item, entity.GetPropertyValue(item.PackString()));
            }
            string primaryKey = DALAttrHelper.GetKey(typeof(T));
            cdal.AddWhere(primaryKey, entity.GetPropertyValue(primaryKey));
            return await cdal.UpdateAsync();
        }

        /// <summary>
        /// 更新数据 异步
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="fieldNames">需要更新的字段</param>
        /// <returns></returns>
        public static async Task<bool> UpdateByEntityAsync<T>(T entity, params Enum[] fieldNames) where T : class
        {
            return await UpdateByEntityAsync(entity, string.Empty, fieldNames);
        }

        /// <summary>
        /// 根据分片key更新数据
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <param name="fieldNames">需要更新的字段</param>
        /// <returns></returns>
        public static bool UpdateEntityByShardKeys<T>(T entity, string dataBaseName, params Enum[] fieldNames) where T : class
        {
            if (entity == null) return false;
            CommonORM cdal = new CommonORM();
            if (string.IsNullOrWhiteSpace(dataBaseName)) cdal.dataBaseName = DALAttrHelper.GetDataBase(entity);
            else cdal.dataBaseName = dataBaseName;
            cdal.tableName = DALAttrHelper.GetTableName(entity);
            foreach (PropertyInfo field in typeof(T).GetProperties())
            {
                var obj = field.GetCustomAttributes(typeof(WriteAttribute), true)?.FirstOrDefault();
                if (obj != null && !(obj as WriteAttribute).Write) continue;
                ShardKeyAttribute da = null;
                ExplicitKeyAttribute ke = null;
                object[] arrobj = field.GetCustomAttributes(typeof(ShardKeyAttribute), true);
                if (arrobj.Length > 0) da = arrobj[0] as ShardKeyAttribute;
                object[] keyobj = field.GetCustomAttributes(typeof(ExplicitKeyAttribute), true);
                if (keyobj.Length > 0) ke = keyobj[0] as ExplicitKeyAttribute;
                var fieldValue = field.GetValue(entity, null);
                if (da != null || (ke != null && fieldValue != null && fieldValue != (object)0 && fieldValue != (object)"")) cdal.AddWhere(field.Name, fieldValue);
                else if (fieldNames == null || fieldNames.Length < 1 || fieldNames.Any(t => t.ToString() == field.Name))
                {
                    cdal.AddUpdate(field.Name, fieldValue);
                }
            }
            return cdal.Update();
        }

        /// <summary>
        /// 根据分片key更新数据
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="fieldNames">需要更新的字段</param>
        /// <returns></returns>
        public static bool UpdateEntityByShardKeys<T>(T entity, params Enum[] fieldNames) where T : class
        {
            return UpdateEntityByShardKeys(entity, string.Empty, fieldNames);
        }

        /// <summary>
        /// 根据分片key更新数据 异步
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="fieldNames">需要更新的字段</param>
        /// <returns></returns>
        public static async Task<bool> UpdateEntityByShardKeysAsync<T>(T entity, params Enum[] fieldNames) where T : class
        {
            return await UpdateEntityByShardKeysAsync(entity, string.Empty, fieldNames);
        }

        /// <summary>
        /// 根据分片key更新数据 异步
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <param name="fieldNames">需要更新的字段</param>
        /// <returns></returns>
        public static async Task<bool> UpdateEntityByShardKeysAsync<T>(T entity, string dataBaseName, params Enum[] fieldNames) where T : class
        {
            if (entity == null) return false;
            CommonORM cdal = new CommonORM();
            if (string.IsNullOrWhiteSpace(dataBaseName)) cdal.dataBaseName = DALAttrHelper.GetDataBase(entity);
            else cdal.dataBaseName = dataBaseName;
            cdal.tableName = DALAttrHelper.GetTableName(entity);
            foreach (PropertyInfo field in typeof(T).GetProperties())
            {
                var obj = field.GetCustomAttributes(typeof(WriteAttribute), true)?.FirstOrDefault();
                if (obj != null && !(obj as WriteAttribute).Write) continue;
                ShardKeyAttribute da = null;
                ExplicitKeyAttribute ke = null;
                object[] arrobj = field.GetCustomAttributes(typeof(ShardKeyAttribute), true);
                if (arrobj.Length > 0) da = arrobj[0] as ShardKeyAttribute;
                object[] keyobj = field.GetCustomAttributes(typeof(ExplicitKeyAttribute), true);
                if (keyobj.Length > 0) ke = keyobj[0] as ExplicitKeyAttribute;
                var fieldValue = field.GetValue(entity, null);
                if (da != null || (ke != null && fieldValue != null && fieldValue != (object)0 && fieldValue != (object)"")) cdal.AddWhere(field.Name, fieldValue);
                else if (fieldNames == null || fieldNames.Length < 1 || fieldNames.Any(t => t.ToString() == field.Name))
                {
                    cdal.AddUpdate(field.Name, fieldValue);
                }
            }
            return await cdal.UpdateAsync();
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entitys">实体列表</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <returns></returns>
        public static bool UpdateBatch<T>(List<T> entitys, string dataBaseName = "") where T : class
        {
            if (entitys == null || entitys.Count < 1) return false;
            bool success = true;
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
            DbConnection conn = GetSqlConnection(dataBaseName, true);
            try
            {
                DbTransaction tran = conn.BeginTransaction();
                foreach (var entity in entitys)
                {
                    if (!conn.Update(entity, tran))
                    {
                        success = false;
                        tran.Rollback();
                        break;
                    }
                }
                if (success) tran.Commit();
            }
            catch { success = false; }
            finally { conn?.Dispose(); }
            return success;
        }

        /// <summary>
        /// 批量更新 异步
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entitys">实体列表</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <returns></returns>
        public static async Task<bool> UpdateBatchAsync<T>(List<T> entitys, string dataBaseName = "") where T : class
        {
            if (entitys == null || entitys.Count < 1) return false;
            bool success = true;
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
            DbConnection conn = GetSqlConnection(dataBaseName, true);
            try
            {
                DbTransaction tran = conn.BeginTransaction();
                foreach (var entity in entitys)
                {
                    if (!await conn.UpdateAsync(entity, tran))
                    {
                        success = false;
                        tran.Rollback();
                        break;
                    }
                }
                if (success) tran.Commit();
            }
            catch { success = false; }
            finally { conn?.Dispose(); }
            return success;
        }
        #endregion

        #region 增
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dataBaseName">指定数据库</param>
        public static bool Add<T>(T entity, string dataBaseName = "") where T : class
        {
            bool val = true;
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
            DbConnection conn = GetSqlConnection(dataBaseName, true);
            try { val = conn.Insert(entity) > 0; }
            catch (OverflowException) { val = true; }
            catch { val = false; }
            finally { conn?.Dispose(); }
            if (string.IsNullOrWhiteSpace(DALAttrHelper.GetKey(typeof(T)))) val = true;//无自增key的无需判断大于0
            return val;
        }

        /// <summary>
        /// 新增 异步
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dataBaseName">指定数据库</param>
        public static async Task<bool> AddAsync<T>(T entity, string dataBaseName = "") where T : class
        {
            bool val = true;
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
            DbConnection conn = GetSqlConnection(dataBaseName, true);
            try { val = await conn.InsertAsync(entity) > 0; }
            catch (OverflowException) { val = true; }
            catch { val = false; }
            finally { conn?.Dispose(); }
            if (string.IsNullOrWhiteSpace(DALAttrHelper.GetKey(typeof(T)))) val = true;//无自增key的无需判断大于0
            return val;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entitys">实体列表</param>
        /// <param name="dataBaseName">指定数据库</param>
        public static bool AddBatch<T>(IEnumerable<T> entitys, string dataBaseName = "") where T : class
        {
            if (entitys == null || entitys.Count() < 1) return false;
            bool success = true;
            bool hasKey = !string.IsNullOrWhiteSpace(DALAttrHelper.GetKey(typeof(T)));
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
            DbConnection conn = GetSqlConnection(dataBaseName, true);
            try
            {
                DbTransaction tran = conn.BeginTransaction();
                foreach (var entity in entitys)
                {
                    bool val = false;
                    try { val = conn.Insert(entity, tran) < 1; }
                    catch (OverflowException) { val = false; }
                    if (val && hasKey)
                    {
                        success = false;
                        tran.Rollback();
                        break;
                    }
                }
                if (success) tran.Commit();
            }
            catch { success = false; }
            finally { conn?.Dispose(); }
            return success;
        }

        /// <summary>
        /// 批量新增 异步
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entitys">实体列表</param>
        /// <param name="dataBaseName">指定数据库</param>
        public static async Task<bool> AddBatchAsync<T>(IEnumerable<T> entitys, string dataBaseName = "") where T : class
        {
            if (entitys == null || entitys.Count() < 1) return false;
            bool success = true;
            bool hasKey = !string.IsNullOrWhiteSpace(DALAttrHelper.GetKey(typeof(T)));
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
            DbConnection conn = GetSqlConnection(dataBaseName, true);
            try
            {
                DbTransaction tran = conn.BeginTransaction();
                foreach (var entity in entitys)
                {
                    bool val = false;
                    try { val = await conn.InsertAsync(entity, tran) < 1; }
                    catch (OverflowException) { val = false; }
                    if (val && hasKey)
                    {
                        success = false;
                        tran.Rollback();
                        break;
                    }
                }
                if (success) tran.Commit();
            }
            catch { success = false; }
            finally { conn?.Dispose(); }
            return success;
        }

        /// <summary>
        /// 新增并返回ID
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <returns>自增ID</returns>
        public static long AddByIdentity<T>(T entity, string dataBaseName = "") where T : class
        {
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
            DbConnection conn = GetSqlConnection(dataBaseName, true);
            long val = 0;
            try { val = conn.Insert(entity); }
            finally { conn?.Dispose(); }
            return val;
        }

        /// <summary>
        /// 新增并返回ID 异步
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dataBaseName">指定数据库</param>
        /// <returns>自增ID</returns>
        public static async Task<long> AddByIdentityAsync<T>(T entity, string dataBaseName = "") where T : class
        {
            if (string.IsNullOrWhiteSpace(dataBaseName)) dataBaseName = DALAttrHelper.GetDataBase(typeof(T));
            DbConnection conn = GetSqlConnection(dataBaseName, true);
            long val = 0;
            try { val = await conn.InsertAsync(entity); }
            finally { conn?.Dispose(); }
            return val;
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <returns></returns>
        void GetDataBaseType<T>()
        {
            if (hasReadDataBase)//判断是否读取过数据库类型
            {
                return;
            }
            hasReadDataBase = true;
            var dataBaseTypeName = DALAttrHelper.GetDataBaseType(typeof(T));
            if (dataBaseTypeName == DataBaseType.SQLServer.GetHashCode().PackEnumValue(typeof(DataBaseType)))
            {
                dataBaseTypeInit = DataBaseType.SQLServer;
                return;
            }
            if (dataBaseTypeName == DataBaseType.MySQL.GetHashCode().PackEnumValue(typeof(DataBaseType)))
            {
                dataBaseTypeInit = DataBaseType.MySQL;
                return;
            }
        }
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <returns></returns>
        void GetDataBaseTypeByFields(Enum field)
        {
            if (hasReadDataBase)//判断是否读取过数据库类型
            {
                return;
            }
            var dataBaseTypeName = DALAttrHelper.GetDataBaseTypeByFields(field);
            hasReadDataBase = true;
            if (dataBaseTypeName == DataBaseType.SQLServer.GetHashCode().PackEnumValue(typeof(DataBaseType)))
            {
                dataBaseTypeInit = DataBaseType.SQLServer;
                return;
            }
            if (dataBaseTypeName == DataBaseType.MySQL.GetHashCode().PackEnumValue(typeof(DataBaseType)))
            {
                dataBaseTypeInit = DataBaseType.MySQL;
                return;
            }
        }
        #endregion
    }
}
