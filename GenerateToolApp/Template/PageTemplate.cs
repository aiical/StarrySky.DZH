using GenerateToolApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenerateToolApp.Common.ORMEnum;

namespace GenerateToolApp.Template
{
    /// <summary>
    /// 生成模板
    /// </summary>
    public static class PageTemplate
    {

        #region  基于Dapper的模板生成
        /// <summary>
        /// 获取列表页字符串
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static string GetListPageString(string tableName, dynamic tables)
        {
            var columnList = ORMUtil.GetOrmColumnList(tables);

            var formItem = new StringBuilder();
            var tableColumnName = new StringBuilder();
            var keyName = "Id";
            foreach (ORMColumn item in columnList)
            {
                if (item.IsPrimaryKey)
                {
                    keyName = item.Name;
                }

                tableColumnName.Append($@"
            <el-table-column prop=""{item.Name}"" label=""{item.Comment}""></el-table-column>");

                switch (item.UIControls)
                {
                    case UIControlsType.select:
                        formItem.Append($@"
        <el-form-item label=""{item.Comment}"" prop=""{item.Name}"">
            <el-select v-model=""Search.{item.Name}"" placeholder=""请选择{item.Comment}"" value-key size='small' >
            </el-select>
        </el-form-item>");
                        break;
                    case UIControlsType.date:
                        formItem.Append($@"                
        <el-form-item label=""{item.Comment}"" prop=""{item.Name}"">
            <el-date-picker size='small' type=""date"" placeholder=""选择{item.Comment}"" v-model=""Search.{item.Name}"" value-format=""yyyy-MM-dd"" style=""width: 100%;""></el-date-picker>
        </el-form-item>");
                        break;
                    case UIControlsType.checkbox:
                        formItem.Append($@"");
                        break;
                    case UIControlsType.radio:
                        formItem.Append($@"");
                        break;
                    default:
                        formItem.Append($@" 
        <el-form-item label=""{item.Comment}"" prop=""{item.Name}"">
            <el-input size='small' v-model=""Search.{item.Name}""></el-input>
        </el-form-item>");
                        break;

                }
            }


            return $@"

@{{
    ViewData[""Title""] = ""列表"";
    Layout = ""~/Views/Shared/_Layout.cshtml"";
}}

<div id=""wrap"" v-cloak>
    <div class=""filter-wrap"">
        <el-form :inline=""true"" :model=""Search"" class=""demo-form-inline"">
            
        	{formItem}
            <el-form-item>
                <el-button-group>
                    <el-button size=""mini"" type=""primary"" icon=""el-icon-search"" v-on:click=""handleSelect"">查询</el-button>
                    <el-button size=""mini"" type=""warning"" icon=""el-icon-edit"" v-on:click=""handleEdit(0)"">新增</el-button>
                </el-button-group>
            </el-form-item>
        </el-form>
    </div>
    <div class=""data-Wrap"">
        <el-table :data=""tableData"" style=""width: 100%;"" v-loading=""loading"">
            {tableColumnName}
            <el-table-column label=""操作"" width=""120"">
                <template slot-scope=""scope"">
                    <el-button size=""small"" type=""text"" v-on:click=""handleEdit(scope.row.{keyName})"">编辑</el-button>
                    <el-button size=""small"" type=""text"" v-on:click=""handleRowStatus(scope.row)"" >{{{{scope.row.RowStatus==1?""无效"":""有效""}}}}</el-button>
                </template>
            </el-table-column>
        </el-table>
    </div>
    <div class=""page-wrap"">
        <el-pagination v-on:current-change=""handleSelect""
                       :current-page.sync=""Search.PageIndex""
                       :page-size=""Search.PageSize""
                       layout=""total,prev, pager, next, jumper""
                       :total=""totalCount"">
        </el-pagination>
        <div style=""clear:both""></div>
    </div>
</div>
<script>
    var vm = new Vue({{
        el: ""#wrap"",
        data: function () {{
            return {{
                Search: {{
                    PageIndex: 1,
                    PageSize: 10,
                }},
                tableData: [],
                totalCount: 0,
                totalPage: 0,
                loading: false
            }};
        }},
        watch: {{}},
        computed: {{
        }},
        methods: {{
            handleEdit(id) {{
                const that = this; 
                const title = id > 0 ? '编辑' : ""新增"";
                const url = `/{tableName}/Edit?id=${{id}}`;
                OpenEditPage(url, title, ""800"", ""600"", that.GetList);
            }},
            handleRowStatus(row) {{
                const that = this;
                var text = row.RowStatus == 1 ? '无效' : '有效';
                var val = row.RowStatus == 1 ? '2' : '1';

                this.$confirm(`确定要${{text}}么?`, '提示', {{
                    confirmButtonText: '嗯,我确定',
                    cancelButtonText: '手滑了',
                    type: 'warning'
                }}).then(() => {{
                    var url = `/{tableName}/SetRowStatus?id=${{row.GEId}}&val=${{val}}`;
                    ajaxCall.Get(that, url, {{}}, function (that, data) {{
                        that.alert.Success(text + ""成功"");
                        that.GetList();
                    }});

                }}).catch(() => {{ }});

            }},
            GetList() {{
                const that = this;
                const url = `/{ tableName}/Get{tableName}List`;
                ajaxCall.PostForListPage(that, url, that.Search);
            }},
            handleSelect(val) {{
               const that = this;
                that.Search.PageIndex = val;
                that.GetList();
            }}
        }},
        mounted: function () {{
            this.handleSelect(1);
        }}
    }});

</script>

";
        }

        /// <summary>
        /// 生成编辑页面字符串
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static string GetEditPageString(string tableName, dynamic tables)
        {
            var columnList = ORMUtil.GetOrmColumnList(tables);

            var formItem = new StringBuilder();
            foreach (ORMColumn item in columnList)
            {

                switch (item.UIControls)
                {
                    case UIControlsType.select:
                        formItem.Append($@"
        <el-form-item label=""{item.Comment}"" prop=""{item.Name}"">
            <el-select  size='small' v-model=""form.{item.Name}"" placeholder=""请选择{item.Comment}"" value-key >
            </el-select>
        </el-form-item>");
                        break;
                    case UIControlsType.date:
                        formItem.Append($@"                
        <el-form-item label=""{item.Comment}"" prop=""{item.Name}"">
            <el-date-picker  size='small' type=""date"" placeholder=""选择{item.Comment}"" v-model=""form.{item.Name}"" value-format=""yyyy-MM-dd"" style=""width: 100%;""></el-date-picker>
        </el-form-item>");
                        break;
                    case UIControlsType.checkbox:
                        formItem.Append($@"");
                        break;
                    case UIControlsType.radio:
                        formItem.Append($@"");
                        break;
                    default:
                        formItem.Append($@" 
        <el-form-item label=""{item.Comment}"" prop=""{item.Name}"">
            <el-input  size='small' v-model=""form.{item.Name}""></el-input>
        </el-form-item>");
                        break;

                }


            }

            return $@"


@{{
    ViewBag.Title = ""编辑"";
    Layout = ""~/Views/Shared/_Layout.cshtml"";
}}
<div id=""editApp"">
    <el-form ref=""ruleForm"" :model=""form"" label-width=""80px""  :rules=""rules"" >
       {formItem}
        <el-form-item>
            <el-button type=""primary"" size=""small""  :loading=""saveLoading"" v-on:click=""OnSubmit('ruleForm')"">{{{{saveText}}}}</el-button>
            <el-button size=""small"" v-on:click=""ResetForm('ruleForm')"">重置</el-button>
        </el-form-item>
    </el-form>
</div>

<script>
    var vm = new Vue({{
        el: ""#editApp"",
        data: function () {{
            return {{
                saveLoading:false,
                form: {{}},
                rules: {{

                }}
            }};

        }},
        computed: {{
            saveText: function () {{

                return this.saveLoading ? '保存中...' : '保存';
            }}
        }},
        methods: {{
            checkData() {{
                var that = this;               
                return true;
            }},
            Get{tableName}Detail(id) {{
                const that = this;
                const url = `/{tableName}/Get{tableName}Detail?id=${{id}}`;
                ajaxCall.Get(that, url, {{}}, function (that, data) {{
                    that.form = data.Body;
                }}, {{}});
            }},
            PostToServer() {{
                const that = this;
                const url = `/{tableName}/Edit{tableName}`;
                ajaxCall.Post(that, url, that.form
                    , function (that, data) {{
                       if (data.ResultCode == 1) {{
                            that.alert.Success(""保存成功"");
                            that.saveLoading = false;
                        }}
                        else {{
                            that.alert.Error(data.ResultMsg);
                            that.saveLoading = false;
                        }}
                    }}, function (that) {{
                        that.saveLoading = false;
                    }});
            }},
            OnSubmit(formName) {{
                const  that = this;
                if (!that.checkData()) {{
                    that.saveLoading = false;
                    return;
                }}
                if (this.saveLoading) {{
                    that.alert.Error(""正在保存中,请耐心等待。。。"");
                    return;
                }}
                that.saveLoading = true;
                console.log('submit!');
                this.$refs[formName].validate((valid) => {{
                    if (valid) {{
                        that.PostToServer();
                    }} else {{
                        that.saveLoading = false;
                        console.log('error submit!!');
                        return false;
                    }}
                }});

            }},
            ResetForm(formName) {{
                this.$refs[formName].resetFields();
            }}
        }},
        mounted: function () {{
            const id = this.$utils.getUrlKey(""id"");
            if(id>0)
            {{
                this.Get{tableName}Detail(id);
            }}
        }}

    }});


</script>
";
        }

        /// <summary>
        /// 生成控制器字符串
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="projectName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static string GetControllerString(string tableName, string projectName = "ORMCore", string dbName = "")
        {
            return $@"

using Utility;
using System;
using System.Web.Mvc;
using {projectName}.Entity;
using {projectName}.Model;
using {projectName}.Services;


namespace {projectName}.Controllers
{{
    /// <summary>
    /// {tableName}控制器
    /// </summary>
    public partial class {tableName}Controller : BaseController
    {{
        public Lazy<{tableName}Services> Services = new Lazy<{tableName}Services>();


        #region  页面
        // GET: {tableName}
        public ActionResult Index()
        {{
            return Content(""{tableName}"");
        }}

        public ActionResult List()
        {{
            return View(""~/Views/{dbName}/{tableName}/{tableName}List.cshtml"");
        }}

        public ActionResult Edit()
        {{
            return View(""~/Views/{dbName}/{tableName}/{tableName}Edit.cshtml"");
        }}
        #endregion

        #region API

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name=""param""></param>
        /// <returns></returns>
        // GET: {tableName}Controller_Api
        [HttpPost]
        public JsonResult Get{tableName}List({tableName}Search param)
        {{
            if (param == null)
            {{
                return CustomResult(""参数序列化错误"");
            }}
            
            return Json(Services.Value.GetList(param));
        }}

        /// <summary>
        ///设置行状态
        /// </summary>
        /// <param name=""id""></param>
        /// <param name=""val""></param>
        /// <returns></returns>
        public JsonResult SetRowStatus(string id, string val)
        {{
            if (id.PackInt() <= 0)
            {{
                return CustomResult("""", 401, ""未获取到正确的ID"");
            }}

            var r = val.PackInt() == 1 ? 1 : 2;
            var flag = Services.Value.SetRowStatus(id, r, UserInfo.UserName);
            if (flag)
            {{
                return OkResult("""");
            }}

            return CustomResult("""", 2, ""设置失败"");
        }}
         

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public JsonResult Edit{tableName}({tableName}View model)
        {{
            if (model == null)
            {{
                return CustomResult( ""参数序列化错误"");
            }} 
            return Json(Services.Value.EditModel(model));
        }}

        /// <summary>
        /// 查询详细信息
        /// </summary>
        /// <returns></returns>
        public JsonResult Get{tableName}Detail(string id)
        {{
            return Json(Services.Value.Get{tableName}Detail(id));
        }}

        #endregion
    }}
}}";
        }

        /// <summary>
        /// 生成服务类字符串
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tables"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static string GetServiceString(string tableName, dynamic tables, string projectName = "ORMCore")
        {
            var columnList = ORMUtil.GetOrmColumnList(tables);
            var keyName = "Id";
            foreach (ORMColumn item in columnList)
            {
                if (item.IsPrimaryKey)
                {
                    keyName = item.Name;

                }
            }

            return $@"
using {projectName}.DAL;
using System;
using {projectName}.Entity;
using {projectName}.Model;
using {projectName}.Common;

namespace {projectName}.Services
{{
    /// <summary>
    /// {tableName}Services
    /// </summary>
    public partial class {tableName}Services: BaseResponse
    {{
        /// <summary>
        /// {tableName}Access
        /// </summary>
        public Lazy<{tableName}Access> DataAccess = new Lazy<{tableName}Access>();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name=""request""></param>
        /// <returns></returns>
        public ResultMessage<PageDataInfo<{tableName}View>> GetList({tableName}Search request)
        {{
            return OkResult(DataAccess.Value.GetList(request));
        }}
        
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name=""id""></param>
        /// <returns></returns>
        public ResultMessage<{tableName}View> Get{tableName}Detail(string id)
        {{
            var models= DataAccess.Value.Get{tableName}Detail(id);
            if (models == null)
            {{
                return FailResult(models,""数据为空"");
            }}
            else
            {{
                return OkResult(models);
            }}
        }}

        /// <summary>
        /// 编辑实体
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public ResultMessage<long> EditModel({tableName}View model)
        {{
            if (model == null)
            {{
                return  FailResult(0L,""实体不能为空"");
            }}
            var dataModel = model.ToDataModel();
            if (dataModel == null)
            {{
                return FailResult(0L, ""实体转换失败""); 
            }}
            if (dataModel.{keyName}  > 0)
            {{
               var tupleValue= DataAccess.Value.UpdateModelById(dataModel);
                if (tupleValue.Item1)
                {{
                    return OkResult(dataModel.{keyName} , tupleValue.Item2);
                }}
                else {{
                    return FailResult(0L, tupleValue.Item2);
                }}
            }}
            else {{
                var tupleValue = DataAccess.Value.AddModel(dataModel);
                return OkResult(tupleValue.Item1, tupleValue.Item2);
            }}
            
        }}

        /// <summary>
        /// 设置数据有效/无效
        /// </summary>
        /// <param name=""id""></param>
        /// <param name=""val""></param>
        /// <param name=""doUserName""></param>
        /// <returns></returns>
        public bool SetRowStatus(string id, int val, string doUserName)
        {{
            return DataAccess.Value.SetRowStatus(id, val, doUserName);
        }}

    }}
}}

";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <param name="tables"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static string GetDalString(string dbName, string tableName, dynamic tables,
            string projectName = "ORMCore")
        {
            var columnList = ORMUtil.GetOrmColumnList(tables);
            var keyName = "Id";
            var updateTime = "UpdateTime";
            var updateUserName = "UpdateUserName";
            var rowStatusName = "RowStatus";

            var whereItem = new StringBuilder();
            var showItem = new StringBuilder();
            var updateItem = new StringBuilder();
            foreach (ORMColumn item in columnList)
            {

                #region 找到字段
                if (item.IsPrimaryKey)
                {
                    //找key
                    keyName = item.Name;
                }
                if (item.Name.Contains("RowStatus"))
                {
                    //找状态字段
                    rowStatusName = item.Name;
                }
                if (item.Name.Contains("UpdateTime"))
                {
                    updateTime = item.Name;
                }
                if (item.Name.Contains("UpdateUserName") || item.Name.Contains("UpdateBy") || item.Comment.Contains("更新人") || item.Comment.Contains("修改人"))
                {
                    updateUserName = item.Name;

                }
                #endregion

                #region  更新域
                if (!item.IsPrimaryKey)
                {
                    var checkString = ORMUtil.GetEmptyCheckString($"model.{item.Name}", item.CodeType);
                    //
                    updateItem.Append($@"
            {checkString}
                {{
                cal.AddUpdate({tableName}Fields.{item.Name},model.{item.Name});
                }}");
                }
                #endregion

                #region show域
                showItem.Append($@"
            cal.AddShow({tableName}Fields.{item.Name});");
                #endregion

                #region where域
                whereItem.Append($@"
            if (!string.IsNullOrEmpty(search.{item.Name}))
            {{
                cal.AddWhere({tableName}Fields.{item.Name}, search.{item.Name});
            }}");
                #endregion

            }


            return $@"
using  {projectName}.Entity;
using  {projectName}.Model;
using ORM;
using System;
using System.Linq;
using Utility;

namespace {projectName}.DAL
{{
    /// <summary>
    /// {tableName}Access
    /// </summary>
    public partial class {tableName}Access
    {{
        /// <summary>
        /// GetList
        /// </summary>
        /// <param name=""search""></param>
        /// <returns></returns>
        public PageDataInfo<{tableName}View> GetList({tableName}Search search)
        {{
            var cal = new CommonDAL();
            cal.AddNotes(""系统生成"", ""通用列表查询"");
            cal.AddOrder({tableName}Fields.{keyName}, DALEnum.OrderEnum.Desc);
            
            {showItem}

            {whereItem}
            cal.pageIndex = search.PageIndex;
            cal.pageSize = search.PageSize;
            var modelList = cal.GetList<{tableName}View>();

            return new PageDataInfo<{tableName}View>
            {{
                List = modelList,
                TotalCount = cal.TotalCount,
                PageSize = cal.pageSize,
                PageIndex = cal.pageIndex
            }};
        }}

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""id""></param>
        /// <returns></returns>
        public {tableName}View Get{tableName}Detail(string id)
        {{
            if (id.PackLong() <= 0)
            {{
                return null;
            }}
            var cal = new CommonDAL();
            cal.AddNotes(""系统生成"", ""通用列表查询"");
            cal.AddOrder({tableName}Fields.{keyName}, DALEnum.OrderEnum.Desc);
             
 			{showItem}

            cal.AddWhere({tableName}Fields.{keyName}, id);

            var list = cal.GetList<{tableName}View>().ToList();
            return !list.Any() ? null : list.FirstOrDefault();
        }}

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public Tuple<long, string> AddModel({tableName}Model model)
        {{
            if (model == null)
            {{
                return new Tuple<long, string>(0L, ""实体不能为空"");
            }}
            var flag = CommonDAL.AddByIdentity(model);
            return new Tuple<long, string>(flag, ""新增成功"");
        }}

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public Tuple<bool, string> UpdateModelById({tableName}Model model)
        {{
            if (model == null)
            {{
                return new Tuple<bool, string>(false,""实体不能为空"");
            }}
            if (model.{keyName} <= 0)
            {{
                return new Tuple<bool, string>(false, ""更新时主键不能小于0"");
            }}
            var cal = new CommonDAL();
            cal.AddNotes(""系统生成"", ""通用列表查询"");
            cal.AddWhere({tableName}Fields.{keyName}, model.{keyName});
            
            {updateItem}
            var flag=cal.Update();
            return new Tuple<bool, string>(flag, flag?"""":""数据更新错误"");

        }}

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""id""></param>
        /// <param name=""val"">2代表无效</param>
        /// <param name=""doUserName""></param>
        /// <returns></returns>
        public bool SetRowStatus(string id, int val,string doUserName)
        {{
            if (id.PackLong() <= 0)
            {{
                return false;
            }}
            var cal = new CommonDAL();
            cal.AddNotes(""赵建"", ""更新状态"");
            cal.AddUpdate({tableName}Fields.{updateTime}, DateTime.Now);
            cal.AddUpdate({tableName}Fields.{updateUserName}, doUserName);
            cal.AddWhere({tableName}Fields.{keyName}, id);
            cal.AddUpdate({tableName}Fields.{rowStatusName}, val);
            return cal.Update();
        }}
    }}
}}



";
        }

        #endregion

    }
}
