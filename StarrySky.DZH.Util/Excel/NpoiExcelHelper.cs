using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Excel
{
    public class NpoiExcelHelper
    {
        #region 生成模板 包含指定excel单元格为下拉框
        /*
         *  /// <summary>
        /// 转list到execl
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="workbook">book</param>
        /// <param name="list">集合</param>
        /// <param name="sheetName">sheet名</param>
        private void ListToExecl<T>(XSSFWorkbook workbook, List<T> list, string sheetName)
            where T : class, new()
        {
            sheetName = string.IsNullOrEmpty(sheetName) ? "Sheet1" : sheetName;
            workbook.CreateSheet(sheetName);
            var sheetOne = workbook.GetSheet(sheetName); // 获取名称为xx的工作表
            var headerRow = sheetOne.CreateRow(0);  // 获取Sheet1工作表的首行
            // Header格式
            ICellStyle cellStyle = workbook.CreateCellStyle();

            // 边框
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            // 获取所有列名
            var cellNames = new T().GetType().GetProperties();
            for (var j = 0; j < cellNames.Length; j++)
            {
                var cell = headerRow.CreateCell(j);
                cell.SetCellValue(cellNames[j].Name);

                // 设置单元的宽度
                sheetOne.SetColumnWidth(j, 20 * 256);
                cell.CellStyle = cellStyle;
                headerRow.Cells.Add(cell);
            }

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                var sheetRow = sheetOne.CreateRow(i + 1);
                var properties = item.GetType().GetProperties();
                for (var j = 0; j < properties.Length; j++)
                {
                    var cell = sheetRow.CreateCell(j, CellType.String);
                    cell.SetCellValue(properties[j].GetValue(item, null)?.ToString() ?? string.Empty);
                    cell.CellStyle = cellStyle;
                    sheetRow.Cells.Add(cell);
                }
            }

            #region 处理需要下拉框的列
            var invTypeList = new List<string>();
            foreach (Enum enumValue in Enum.GetValues(typeof(BW_Stand_Tickets_InvoiceTypeEnum)))
            {
                int key = enumValue.GetHashCode();
                string description = enumValue.GetEnumDescription();
                invTypeList.Add(description);
            }

            var sheetName_Dropdown = "InvoiceType";
            var sheet2 = workbook.CreateSheet(sheetName_Dropdown);
            workbook.SetSheetHidden(workbook.GetSheetIndex(sheetName_Dropdown), SheetState.Hidden); // 隐藏Sheet
            for (int i = 0; i < invTypeList.Count; i++)
            {
                sheet2.CreateRow(i).CreateCell(0).SetCellValue(invTypeList[i]);
            }

            var rangeName = $"{sheetName_Dropdown}Range";
            IName range = workbook.CreateName();
            range.RefersToFormula = $"{sheetName_Dropdown}!$A$1:$A${(invTypeList.Count == 0 ? 1 : invTypeList.Count)}";
            range.NameName = rangeName;
            #endregion
            #region xssf用法
            CellRangeAddressList addressList = new CellRangeAddressList(1, 65535, 2, 2); // 设置生成下拉框的行和列，从第1到第65535行，从第3列到第3列
            XSSFDataValidationHelper dvHelper = new XSSFDataValidationHelper((XSSFSheet)sheetOne);

            XSSFDataValidationConstraint dvConstraint = (XSSFDataValidationConstraint)dvHelper
                    .CreateFormulaListConstraint(rangeName);

            // 设置区域边界
            XSSFDataValidation validation = (XSSFDataValidation)dvHelper
                    .CreateValidation(dvConstraint, addressList);

            // 输入非法数据时，弹窗警告框
            validation.ShowErrorBox = true;

            // 设置提示框
            validation.CreateErrorBox("输入不合法", "请输入或选择下拉列表中的值。");
            validation.ShowPromptBox = true;
            sheetOne.AddValidationData(validation);
            #endregion
            #region hssf

            // CellRangeAddressList regions = new CellRangeAddressList(1, 65535, 3, 3);
            // DVConstraint constraint = DVConstraint.CreateFormulaListConstraint(rangeName);
            // HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
            // dataValidate.CreateErrorBox("输入不合法", "请输入或选择下拉列表中的值。");
            // dataValidate.ShowPromptBox = true;
            // sheetOne.AddValidationData(dataValidate);
            #endregion

        }
       
         */

        #endregion

        /// <summary>
        /// 转execl到list（execl的第一行需要严格对应到class的第一个属性，速度较慢，数据量大请误用）
        /// 只支持属性全是string的T对象（防止类型转换异常）
        /// excel第一行需为列名
        /// </summary>
        /// <typeparam name="T">只支持属性全是string的T对象（防止类型转换异常）</typeparam>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static List<T> ExportToList<T>(string fileName, Stream stream, string sheetName) where T : class, new()
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            try
            {
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                {
                    workbook = new XSSFWorkbook(stream);
                }
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                {
                    workbook = new HSSFWorkbook(stream);
                }
                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }

                var t = new T();
                var properties = t.GetType().GetProperties();
                var fields = properties.Select(x => x.Name).ToArray();

                List<T> list = new List<T>();
                //遍历每一行数据
                for (int i = sheet.FirstRowNum + 1, len = sheet.LastRowNum + 1; i < len; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null || IsRowEmpty(row))
                    {
                        break;
                    }
                    t = new T();
                    for (int j = 0, len2 = fields.Length; j < len2; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null) continue;
                        // fix excel中日期格式会变成numeric,直接转string获取的是double类型的字串问题
                        // 不能直接cell.DateCellValue尝试取值，会抛异常，没有该属性
                        // 如果是数值型，直接使用 cell.NumericCellValue会有精度问题。需要调用SetCellType(CellType.String)转换一下
                        if (cell.CellType == CellType.Numeric && DateUtil.IsValidExcelDate(cell.NumericCellValue) && DateUtil.IsCellDateFormatted(cell))
                        {
                            // excel中传“2020年11月23日”如果不设置excel格式，这里还是会cell.CellType == CellType.Numeric 但是DateUtil.IsCellDateFormatted(cell) 为false
                            // 但是DateUtil.IsCellDateFormatted(cell)判断不能去掉，否则常规数字也会转成日期
                            // DateUtil.IsValidExcelDate(cell.NumericCellValue) 没啥用处， 153.6也能识别为true,因为1970-01-01为1;
                            string strValue = cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                            typeof(T).GetProperty(fields[j])?.SetValue(t, strValue, null);
                            continue;
                        }
                        else if (cell.CellType != CellType.String)
                        {
                            // 如果是数值型，直接使用 cell.NumericCellValue会有精度问题。需要调用SetCellType(CellType.String)转换一下
                            cell.SetCellType(CellType.String);
                        }
                        
                        if (cell.StringCellValue != null)
                        {
                            var cellValue = cell.StringCellValue.Trim();
                            typeof(T).GetProperty(fields[j])?.SetValue(t, cellValue, null);
                        }
                    }
                    list.Add(t);
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 判断是否空行
        /// </summary>
        /// <param name="row">行</param>
        /// <returns>true false</returns>
        private static bool IsRowEmpty(IRow row)
        {
            for (int c = row.FirstCellNum; c < row.LastCellNum; c++)
            {
                var cell = row.GetCell(c);
                if (cell != null && cell.CellType != CellType.Blank)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 转list到execl
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static NpoiMemoryStream ListToExecl<T>(List<T> list) where T : class, new()
        {
            var workbook = new XSSFWorkbook();
            workbook.CreateSheet("Sheet1");
            var sheetOne = workbook.GetSheet("Sheet1"); //获取名称为Sheet1的工作表

            var sheetRow0 = sheetOne.CreateRow(0);  //获取Sheet1工作表的首行
            var properties0 = new T().GetType().GetProperties();
            for (var j = 0; j < properties0.Length; j++)
            {
                var col = sheetRow0.CreateCell(j);
                col.SetCellValue(properties0[j].Name);
                sheetRow0.Cells.Add(col);
            }

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                var sheetRow = sheetOne.CreateRow(i + 1);
                var properties = item.GetType().GetProperties();
                for (var j = 0; j < properties.Length; j++)
                {
                    var col = sheetRow.CreateCell(j);
                    col.SetCellValue(properties[j].GetValue(item, null)?.ToString() ?? "");
                    sheetRow.Cells.Add(col);
                }
            }
            var ms = new NpoiMemoryStream
            {
                AllowClose = false
            };
            workbook.Write(ms);
            return ms;
        }


        /// <summary>
        /// 转list到execl,美化了excel
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="workbook">book</param>
        /// <param name="list">集合</param>
        /// <param name="sheetName">sheet名</param>
        public static void ListToExecl_Format<T>(XSSFWorkbook workbook, List<T> list, string sheetName)
            where T : class, new()
        {
            sheetName = string.IsNullOrEmpty(sheetName) ? "Sheet1" : sheetName;
            workbook.CreateSheet(sheetName);
            var sheetOne = workbook.GetSheet(sheetName); // 获取名称为xx的工作表
            var headerRow = sheetOne.CreateRow(0);  // 获取Sheet1工作表的首行
            // Header格式
            ICellStyle cellStyle = workbook.CreateCellStyle();

            // 边框
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            // 获取所有列名
            var cellNames = new T().GetType().GetProperties();
            for (var j = 0; j < cellNames.Length; j++)
            {
                var cell = headerRow.CreateCell(j);
                cell.SetCellValue(cellNames[j].Name);

                // 设置单元的宽度
                sheetOne.SetColumnWidth(j, 20 * 256);
                cell.CellStyle = cellStyle;
                headerRow.Cells.Add(cell);
            }

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                var sheetRow = sheetOne.CreateRow(i + 1);
                var properties = item.GetType().GetProperties();
                for (var j = 0; j < properties.Length; j++)
                {
                    var cell = sheetRow.CreateCell(j, CellType.String);
                    cell.SetCellValue(properties[j].GetValue(item, null)?.ToString() ?? string.Empty);
                    cell.CellStyle = cellStyle;
                    sheetRow.Cells.Add(cell);
                }
            }
        }



        #region DataTable
        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        //public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        //{
        //    int i = 0;
        //    int j = 0;
        //    int count = 0;
        //    try
        //    {
        //        if (workbook != null)
        //        {
        //            sheet = workbook.CreateSheet(sheetName);
        //        }
        //        else
        //        {
        //            return -1;
        //        }

        //        if (isColumnWritten == true) //写入DataTable的列名
        //        {
        //            IRow row = sheet.CreateRow(0);
        //            for (j = 0; j < data.Columns.Count; ++j)
        //            {
        //                row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
        //            }
        //            count = 1;
        //        }
        //        else
        //        {
        //            count = 0;
        //        }

        //        for (i = 0; i < data.Rows.Count; ++i)
        //        {
        //            IRow row = sheet.CreateRow(count);
        //            for (j = 0; j < data.Columns.Count; ++j)
        //            {
        //                row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
        //            }
        //            ++count;
        //        }
        //        //workbook.Write(fs); //写入到excel
        //        return count;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: " + ex.Message);
        //        return -1;
        //    }
        //}

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        //public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        //{
        //    DataTable data = new DataTable();
        //    int startRow = 0;
        //    try
        //    {
        //        if (sheet != null)
        //        {
        //            IRow firstRow = sheet.GetRow(0);
        //            int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

        //            if (isFirstRowColumn)
        //            {
        //                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
        //                {
        //                    ICell cell = firstRow.GetCell(i);
        //                    if (cell != null)
        //                    {
        //                        string cellValue = cell.StringCellValue;
        //                        if (cellValue != null)
        //                        {
        //                            DataColumn column = new DataColumn(cellValue);
        //                            data.Columns.Add(column);
        //                        }
        //                    }
        //                }
        //                startRow = sheet.FirstRowNum + 1;
        //            }
        //            else
        //            {
        //                startRow = sheet.FirstRowNum;
        //            }

        //            //最后一列的标号
        //            int rowCount = sheet.LastRowNum;
        //            for (int i = startRow; i <= rowCount; ++i)
        //            {
        //                IRow row = sheet.GetRow(i);
        //                if (row == null) continue; //没有数据的行默认是null　　　　　　　

        //                DataRow dataRow = data.NewRow();
        //                for (int j = row.FirstCellNum; j < cellCount; ++j)
        //                {
        //                    if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
        //                        dataRow[j] = row.GetCell(j).ToString();
        //                }
        //                data.Rows.Add(dataRow);
        //            }
        //        }

        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: " + ex.Message);
        //        return null;
        //    }
        //}

        #endregion
    }

    /// <summary>
    /// 支持Npoi到内存流的直接操作
    /// </summary>
    public class NpoiMemoryStream : MemoryStream
    {
        /// <summary>
        /// 
        /// </summary>
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        /// <summary>
        /// 总是关闭的
        /// </summary>
        public bool AllowClose { get; set; }

        /// <summary>
        /// 手动关闭
        /// </summary>
        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }
}
