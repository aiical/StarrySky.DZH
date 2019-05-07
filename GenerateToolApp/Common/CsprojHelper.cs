using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using System.IO;

namespace GenerateToolApp.Common
{
    /// <summary>
    /// 解决方案的文件处理 using Microsoft.Build.Evaluation;
    /// </summary>
    public class CsprojHelper
    {
        /// <summary>
        /// 向csproj加入配置
        /// </summary>
        /// <param name="path">解决方案路径</param>
        /// <param name="filePath">文件路径</param>
        public static void AddProject(string path,string filePath)
        {
            path = Path.GetFullPath("../../..");
            ProjectCollection pc = new ProjectCollection();
            Project poj = pc.LoadProject(path);
            //获取文件的配置项
            var items = poj.Items;
            //加入配置
            IList<ProjectItem> pilist = poj.AddItem("Compile", filePath);//filePath:"Controllers\\TestController.cs"
            //编译与保存
            poj.Build();
            poj.Save();
        }
    }
}
