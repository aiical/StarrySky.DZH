using GenerateToolApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateToolApp.Template
{
    /// <summary>
    /// 实体模版类
    /// </summary>
    public static class ModelTemplate
    {
        #region 原基于dapper 生成的实体 

        
        /// <summary>
        /// 生成视图实体字符串
        /// </summary>
        /// <param name="tableName"></param>

        /// <param name="tables"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static string GetViewModelString(string tableName, dynamic tables, string projectName = "ORMCore")
        {
            if (tables == null)
            {
                return "error:没有查询到表的字段数据";
            }

            var mappingFunc = new StringBuilder();
            var mappingFunc2 = new StringBuilder();

            var viewModelStr = new StringBuilder();
            var tempStr = new StringBuilder();
            foreach (var item in tables)
            {
                viewModelStr.Append($@"
        /// <summary>
        /// 获取或设置{item.COLUMN_COMMENT}
        /// </summary>
        public string {item.COLUMN_NAME}{{get;set;}}");


                mappingFunc.Append($@"
                    {item.COLUMN_NAME} = {item.COLUMN_NAME}.{ORMUtil.GetPackTypeString(item.DATA_TYPE)}(),");

                mappingFunc2.Append($@"
            {item.COLUMN_NAME} = dataModel.{item.COLUMN_NAME}.PackString();");

            }

            tempStr.Append($@"using System;
using Utility;
using {projectName}.Entity;
using {projectName}.Common;
namespace {projectName}.Model
{{ 
    /// <summary>
    /// {tableName}表ViewModel
    /// </summary>
    public partial class {tableName}View
    {{
        {viewModelStr}

        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public  {tableName}View()
        {{}}

        /// <summary>
        /// 数据实体转视图实体
        /// </summary>
        public {tableName}View({tableName}Model dataModel)
        {{
           {mappingFunc2}
        }}

        /// <summary>
        /// 视图实体转数据实体
        /// </summary>
        /// <returns></returns>
        public {tableName}Entity ToDataEntity()
        {{
            try
            {{
                var dataModel = new {tableName}Entity
                {{{mappingFunc}
                
                }};
                return dataModel;
            }}
            catch (Exception)
            {{
                return null;
            }}
        }}
    }}
 
    /// <summary>
    /// 列表查询时用到的实体
    /// </summary>
    public partial class {tableName}Search: BaseRequestPageList
    {{
        {viewModelStr}

    }}
}}  ");
            return tempStr.ToString();
        }
        #endregion

    }
}