using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZHCodeBuilder.Common
{
    /// <summary>
    /// 需要放在指定项目下面，会更新\GenerateToolApp\bin\Debug下exe执行生成的config文件，具体看方法里调用是不是OpenExeConfiguration
    /// </summary>
    public class ConfigSettingHelper
    {
        #region AppSettings节点
        /// <summary>
        /// 判断是否存在配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasKey(string key)
        {
            return ConfigurationManager.AppSettings.AllKeys.Contains(key);
        }

        /// <summary>
        /// 获取app
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAppStr(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// 获取app
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetAppStr<T>(string name)
        {
            return (T)Convert.ChangeType(GetAppStr(name), typeof(T));
        }
        /// <summary>  
        /// 读取指定key的值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static string GetValue(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
                return "";
            else
                return config.AppSettings.Settings[key].Value;
        }
        /// <summary>  
        /// 写入值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static void SetAppValue(string key, string value)
        {
            //增加的内容写在appSettings段下 <add key="RegCode" value="0"/>  
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
        }

        #endregion AppSettings节点

        #region ConnectionStrings节点
        /// <summary>
        /// 获取当前应用程序默认的connectionstring
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConnectionStr(string name)
        {
            if (ConfigurationManager.ConnectionStrings[name] != null)
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            return "";
        }

        public static string GetExeConnectionStr(string name)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.ConnectionStrings.ConnectionStrings[name] != null)
            {
                return config.ConnectionStrings.ConnectionStrings[name].ConnectionString;
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetExeConnectionStrDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.ConnectionStrings.ConnectionStrings != null)
            {
                for (int i = 0, length = config.ConnectionStrings.ConnectionStrings.Count; i < length; i++)
                {
                    dictionary.Add(config.ConnectionStrings.ConnectionStrings[i].Name, config.ConnectionStrings.ConnectionStrings[i].ConnectionString);
                }
            }
            return dictionary;

        }

        /// <summary>
        /// 更新执行文件config的连接字符串 
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="newConString"></param>
        /// <param name="newProviderName"></param>
        public static void UpdateExeConnStriConfig(string newName, string newConString, string newProviderName = "")
        {
            //新建一个连接字符串实例 
            ConnectionStringSettings mySettings = new ConnectionStringSettings(newName, newConString, newProviderName);

            // 打开可执行的配置文件*.exe.config 
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //file路径获取同上作用一致
            //string file = System.Windows.Forms.Application.ExecutablePath;//D:\我的资料\StarrySky.DZH\GenerateToolApp\bin\Debug\GenerateToolApp.exe
            //Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            if (config.ConnectionStrings.ConnectionStrings[newName] != null)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            // 将新的连接串添加到配置文件中. 
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            // 保存对配置文件所作的更改 
            config.Save(ConfigurationSaveMode.Modified);
            // 强制重新载入配置文件的ConnectionStrings配置节  
            ConfigurationManager.RefreshSection("ConnectionStrings");

        }
        #endregion
    }
}
