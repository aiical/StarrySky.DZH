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
        /// <summary>
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
                    t = new T();
                    IRow row = sheet.GetRow(i);

                    for (int j = 0, len2 = fields.Length; j < len2; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null) break;
                        cell.SetCellType(CellType.String);
                        if (cell.StringCellValue != null)
                        {
                            //stuUser.setPhone(row.getCell(0).getStringCellValue());
                            var cellValue = cell.StringCellValue.Trim();
                            typeof(T).GetProperty(fields[j])?.SetValue(t, cellValue, null);
                        }
                        //string cellValue = null;
                        //switch (cell.CellType)
                        //{
                        //    case CellType.String: //文本
                        //        cellValue = cell.StringCellValue.Trim();
                        //        break;
                        //    case CellType.Numeric: //数值
                        //        cellValue = cell.NumericCellValue.ToString().Trim();
                        //        break;
                        //    case CellType.Boolean: //bool
                        //        cellValue = cell.BooleanCellValue.ToString().Trim();
                        //        break;
                        //    case CellType.Blank: //空白
                        //        cellValue = "";
                        //        break;
                        //    default:
                        //        cellValue = "ERROR";
                        //        break;
                        //}
                        //if (cellValue == "ERROR")
                        //{
                        //    return null;
                        //}
                        //typeof(T).GetProperty(firstRow.GetCell(j).StringCellValue).SetValue(t, cellValue, null);
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
