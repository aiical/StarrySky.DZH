using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Common
{
    public class TypeConvert
    {

        public static Dictionary<string, object> ModelToDictionary<T>(T model)
        {

            Dictionary<String, Object> dict = new Dictionary<string, object>();
            if (model != null)
            {
                Type t = model.GetType();

                PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo p in pi)
                {
                    MethodInfo mi = p.GetGetMethod();

                    if (mi != null && mi.IsPublic)
                    {
                        dict.Add(p.Name, mi.Invoke(model, new Object[] { }));
                    }
                }
            }
            return dict;
        }
        public static string DictionaryToQueryStringASC(Dictionary<string, object> dict)
        {
            if (dict != null && dict.Any())
            {
                dict = dict.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                StringBuilder builder = new StringBuilder();
                if (dict.Count > 0)
                {
                    builder.Append("?");
                    int i = 0;
                    foreach (var item in dict)
                    {
                        if (i > 0)
                            builder.Append("&");
                        builder.AppendFormat("{0}={1}", item.Key, item.Value);
                        i++;
                    }
                }
                return builder.ToString();
            }
            return null;
        }
    }
}
