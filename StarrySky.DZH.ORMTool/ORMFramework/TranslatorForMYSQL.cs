using StarrySky.DZH.Util.DataConvert;
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StarrySky.DZH.ORMTool.ORMFramework.ORMEnum;

namespace StarrySky.DZH.ORMTool.ORMFramework
{
    /// <summary>
    /// MySql方言
    /// </summary>
    public class TranslatorForMYSQL
    {
        /// <summary>
        /// 简单查询拼接TOP功能
        /// </summary>
        /// <param name="top"></param>
        /// <param name="show"></param>
        /// <param name="tableName"></param>
        /// <param name="alias"></param>
        /// <param name="join"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public string FuseSearch(int top, string show, string tableName, string alias, string join, string where, string order)
        {
            return string.Format(" select {1} from {2} {3} {4} where 1=1 {5} {6} {0};"
               , top > 0 ? string.Format("limit {0} ", top) : string.Empty, show
               , tableName, alias, join, where, order);

        }
        /// <summary>
        /// 拼接where字段
        /// </summary>
        /// <param name="wheres"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public string FuseGetWhereStr(List<ParamValue> wheres, Dictionary<string, object> Params)
        {
            StringBuilder whereStr = new StringBuilder();
            for (int i = 0, len = wheres.Count; i < len; i++)
            {
                if (wheres[i].isSql)//使用SQL查询
                {
                    if (string.IsNullOrEmpty(wheres[i].SqlStr)) continue;
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
                        whereStr.AppendFormat(" {0} {1} {2} concat('%',@{1}{3},'%')", wheres[i].opr.PackString(), wheres[i].fieldName, "like", i);
                    }
                    else if (wheres[i].cdt == ConditionEnum.LeftLike)
                    {
                        whereStr.AppendFormat(" {0} {1} {2} concat('%',@{1}{3})", wheres[i].opr.PackString(), wheres[i].fieldName, "like", i);
                    }
                    else if (wheres[i].cdt == ConditionEnum.RightLike)
                    {
                        whereStr.AppendFormat(" {0} {1} {2} concat(@{1}{3},'%')", wheres[i].opr.PackString(), wheres[i].fieldName, "like", i);
                    }
                    else if (wheres[i].cdt == ConditionEnum.InOpertion)
                    {
                        string inStr = wheres[i].value.PackString().Trim().TrimStart('(').TrimEnd(')').Trim(',');
                        var enumerable = GetMultiExec(wheres[i].value);
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

                        var instrall = string.Empty;
                        if (!string.IsNullOrEmpty(inStr))
                        {
                            var arrIn = inStr.Split(',');
                            for (int j = 0; j < arrIn.Length; j++)
                            {
                                instrall += "@" + wheres[i].fieldName + j + ",";
                                Params.Add(string.Format("{0}{1}", wheres[i].fieldName, j), arrIn[j]);
                            }
                        }
                        whereStr.AppendFormat(" {0} {1} {2} ({3})", wheres[i].opr.PackString(), wheres[i].fieldName, "in", instrall.TrimEnd(','));
                    }
                    else whereStr.AppendFormat(" {0} {1} {2} @{1}{3}", wheres[i].opr.PackString(), wheres[i].fieldName, wheres[i].cdt.PackEnumDescription(), i);
                    if (wheres[i].cdt != ConditionEnum.InOpertion) Params.Add(string.Format("{0}{1}", wheres[i].fieldName, i), wheres[i].value);
                }
            }
            return whereStr.ToString();
        }
        /// <summary>
        /// 拼更新的sql
        /// </summary>
        /// <param name="updateStr"></param>
        /// <param name="updates"></param>
        /// <param name="Params"></param>
        /// <param name="notes"></param>
        /// <param name="sql"></param>
        /// <param name="top"></param>
        /// <param name="tableName"></param>
        /// <param name="whereStr"></param>
        public void FuseUpdata(ref StringBuilder updateStr, List<ParamValue> updates, Dictionary<string, object> Params, string notes, ref StringBuilder sql, int top, string tableName, string whereStr)
        {
            #region 更新参数SQL拼接
            for (int i = 0, len = updates.Count; i < len; i++)
            {
                updateStr.AppendFormat("{0}=@UpdateValue{1},", updates[i].fieldName, i);
                Params.Add(string.Format("UpdateValue{0}", i), updates[i].value);
            }
            #endregion

            sql.Append(notes);
            sql.AppendFormat("UPDATE {1} SET {2} WHERE 1=1 {3} {0}"
            , top > 0 ? string.Format("LIMIT {0} ", top) : string.Empty, tableName, updateStr.ToString().Trim().Trim(','), whereStr);
        }
        /// <summary>
        /// 子查询分页
        /// </summary>
        /// <param name="showStr"></param>
        /// <param name="tableName"></param>
        /// <param name="alias"></param>
        /// <param name="join"></param>
        /// <param name="where"></param>
        /// <param name="from"></param>
        /// <param name="pageSize"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public string FusePagination(string showStr, string tableName, string alias, string join, string where, int from, int pageSize, string orders)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select {1} from {2} {3} {4} where 1=1 {5} {6} limit {0},{7};", from >= 0 ? from : 0, showStr, tableName, alias, join, where, orders, pageSize >= 0 ? pageSize : 0);
            //以下的高效分页会被驳回索引，暂时无法使用sql.AppendFormat(" SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;SELECT {0} FROM {1} WHERE {2} {9} (SELECT {2} FROM {1} {8} {7} where 1=1 {4} order by {2} {10} {3} LIMIT {5}, 1) order by {2} {10} {3} LIMIT {6};COMMIT; "
            //    , showStr, tableName, orderKey, orderStr, where, from>=0? from:0, pageSize, join, alias, orderInt==0?"<=":">=", orderInt == 0 ?"DESC":"ASC");            
            return sql.ToString();
        }

        /// <summary>
        /// 获取列表枚举
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IEnumerable GetMultiExec(object param)
        {
            return (param is IEnumerable
                    && !(param is string
                      || param is IEnumerable<KeyValuePair<string, object>>)
                ) ? (IEnumerable)param : null;
        }
    }
}
