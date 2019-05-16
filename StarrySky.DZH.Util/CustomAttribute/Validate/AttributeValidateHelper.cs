using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.CustomAttribute.Validate
{
    /// <summary>
    /// 针对使用特性的属性进行统一验证数据的帮助类
    /// 针对类属性使用的特性
    /// </summary>
    public class AttributeValidateHelper
    {
        public static (bool, string) CheckValid(T obj)
        {
            if (obj == null)
            {
                return (false, "请求参数不能为空");
            }
            //获取实例对象的类型对象
            Type t = typeof(T);
            //t = obj.GetType();
            //创建类型
            //System.Activator提供了方法来根据类型动态创建对象
            //获取Property
            var properties = t.GetProperties();
            foreach (var property in properties)
            {

                //这里只做一个长度的验证，如果要做很多验证，需要好好设计一下,不要用if elseif
                //会非常难于维护，类似这样的开源项目很多
                if (!property.IsDefined(typeof(RangeAttribute), false)) continue;

                var attributes = property.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    //这里的MaximumLength 最好用常量去做
                    var maxinumLength = (int)attribute.GetType().
                      GetProperty("MaximumLength").
                      GetValue(attribute);

                    //获取属性的值
                    var propertyValue = property.GetValue(obj) as string;
                    if (propertyValue == null)
                        throw new Exception("exception info");//这里可以自定义，也可以用具体系统异常类

                    if (propertyValue.Length > maxinumLength)
                        throw new Exception(string.Format("属性{0}的值{1}的长度超过了{2}", property.Name, propertyValue, maxinumLength));
                }
            }
            return (true, "");
        }
    }
}
