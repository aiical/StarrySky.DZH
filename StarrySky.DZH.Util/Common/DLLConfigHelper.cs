using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Common
{
    /// <summary>
    /// DLL的config读取
    /// <para>Author:丁振华</para>
    /// </summary>
    public class DLLConfigHelper
    {
        private static string DllConfigFilePath {
            get {
                Assembly _assembly = Assembly.GetCallingAssembly();
                Uri _uri = new Uri(Path.GetDirectoryName(_assembly.CodeBase));
                
                //这个时候读取的路径是应用程序/网站下面的bin目录，或者应用程序/网站编译运行dll的目录（自定义输出路径的情况）
                //而不是类库的生成dll的输出路径
                //所以，如果采用引用项目的方式引用，要将配置文件一同复制到和dll同一目录（bin目录），否则读取不到配置信息；
                //修改配置文件后同样记得此目录下覆盖原文件
                return Path.Combine(_uri.LocalPath, _assembly.GetName().Name + ".dll.config"); 
            }
        }

        public static Configuration CurrentDllConfiguration { 
        
            get{
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();

                configFile.ExeConfigFilename = DllConfigFilePath;
                return ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
            }
        }
    }
}
