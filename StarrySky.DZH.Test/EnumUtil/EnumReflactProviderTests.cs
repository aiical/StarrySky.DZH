using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.TopORM.Common;
using StarrySky.DZH.TopORM.CustomAttribute;
using StarrySky.DZH.TopORM.Entity;
using StarrySky.DZH.Util.EnumUtil;
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StarrySky.DZH.Util.EnumUtil.MoneyBillEnums;

namespace StarrySky.DZH.Util.EnumUtil.Tests
{
    [TestClass()]
    public class EnumReflactProviderTests
    {
        [TestMethod()]
        public void GetEnumDescriptionsTest()
        {
            //10次 2 1，1 1
            //100次 4 1，7 0
            //1000次 
            //100000次 
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 100; i++)
            {
                FlowStatus.CancelCheck.GetEnumDescription();
                FlowHandleStatus.Hangup.GetEnumDescription();
                FlowHandleStatus.Refuse.GetEnumDescription();
                EventType.Refund.GetEnumDescription();
                MoneyBillEnums.CoinLimitCyc.OneDay.GetEnumDescription();
                MoneyBillEnums.FlowType.Income.GetEnumDescription();
                MoneyBillEnums.ProjectClassify.CaiZheKou.GetEnumDescription();
                MoneyBillEnums.ProjectClassify.CatViP.GetEnumDescription();
                MoneyBillEnums.ProjectClassify.DrawGifts.GetEnumDescription();
                MoneyBillEnums.ProjectClassify.DrawGifts.GetEnumDescription();
                MoneyBillEnums.ProjectClassify.ShoppingPDD.GetEnumDescription();
            }
            long sec1 = sw.ElapsedMilliseconds;
            Console.WriteLine($"优化前：{sec1}");
            sw.Reset();
            sw.Start();
            long sec3 = sw.ElapsedMilliseconds;
            Console.WriteLine($"反射优化{sec3}");
            for (int i = 0; i < 100; i++)
            {
                EnumReflactProvider.GetEnumDescriptions(FlowStatus.CancelCheck);
                EnumReflactProvider.GetEnumDescriptions(FlowHandleStatus.Hangup);
                EnumReflactProvider.GetEnumDescriptions(FlowHandleStatus.Refuse);
                EnumReflactProvider.GetEnumDescriptions(EventType.Refund);
                EnumReflactProvider.GetEnumDescriptions(MoneyBillEnums.CoinLimitCyc.OneDay);
                EnumReflactProvider.GetEnumDescriptions(MoneyBillEnums.FlowType.Income);
                EnumReflactProvider.GetEnumDescriptions(MoneyBillEnums.ProjectClassify.CaiZheKou);
                EnumReflactProvider.GetEnumDescriptions(MoneyBillEnums.ProjectClassify.CatViP);
                EnumReflactProvider.GetEnumDescriptions(MoneyBillEnums.ProjectClassify.DrawGifts);
                EnumReflactProvider.GetEnumDescriptions(MoneyBillEnums.ProjectClassify.DrawGifts);
                EnumReflactProvider.GetEnumDescriptions(MoneyBillEnums.ProjectClassify.ShoppingPDD);
            }
            long sec2 = sw.ElapsedMilliseconds;
            Console.WriteLine($"反射优化{sec2}");
            sw.Stop();
        }


        [TestMethod()]
        public void GetkeyDescriptionsTest()
        {
            //10次 4ms 0ms 0
            //100次 
            //1000次 
            //100000次 
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                var ss = GenericMappingProvider.GetClassConstruction(new DemoEntity()).Properities.FirstOrDefault(p => p.AttrNameList.Contains(typeof(PrimaryKeyAttribute).Name));
                var ss1 = GenericMappingProvider.GetClassConstruction(new PersonEntity()).Properities.FirstOrDefault(p => p.AttrNameList.Contains(typeof(PrimaryKeyAttribute).Name));
                var ss2 = GenericMappingProvider.GetClassConstruction(new AnimalEntity()).Properities.FirstOrDefault(p => p.AttrNameList.Contains(typeof(PrimaryKeyAttribute).Name));
                long seco = sw.ElapsedMilliseconds;
                Console.WriteLine($"优化1：I={i},毫秒：{seco}");
            }
            long sec1 = sw.ElapsedMilliseconds;
            Console.WriteLine($"优化1：{sec1}");//197 毫秒 209
            sw.Reset();
            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                var ss = GenericMappingProvider.GetClassConstruction(new DemoEntity()).Properities.FirstOrDefault(p => p.AttrNameList.Contains(nameof(PrimaryKeyAttribute)));
                var ss1 = GenericMappingProvider.GetClassConstruction(new PersonEntity()).Properities.FirstOrDefault(p => p.AttrNameList.Contains(nameof(PrimaryKeyAttribute)));
                var ss2 = GenericMappingProvider.GetClassConstruction(new AnimalEntity()).Properities.FirstOrDefault(p => p.AttrNameList.Contains(nameof(PrimaryKeyAttribute)));
                Console.WriteLine($"优化2：I={i},毫秒：{sw.ElapsedMilliseconds}");
            }
            long sec4 = sw.ElapsedMilliseconds;
            Console.WriteLine($"优化2：{sec4}");//162 毫秒 176 
            sw.Reset();
            sw.Start();
            long sec3 = sw.ElapsedMilliseconds;
            Console.WriteLine($"反射优化{sec3}");
            for (int i = 0; i < 10; i++)
            {
                var ss = "";
                var ss1 = "";
                var ss2 = "";
                Type type = typeof(DemoEntity);
                var properties = type.GetProperties();//获取属性
                foreach (var p in properties)
                {
                    object[] PrimaryKey = p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                    if (!PrimaryKey.CollectionIsNullOrEmpty()) {
                        ss = p.Name;
                    }
                }

                Type type1 = typeof(PersonEntity);
                var properties1 = type1.GetProperties();//获取属性
                foreach (var p in properties1)
                {
                    object[] PrimaryKey = p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                    if (!PrimaryKey.CollectionIsNullOrEmpty())
                    {
                        ss1 = p.Name;
                    }
                }

                Type type2 = typeof(AnimalEntity);
                var properties2 = type2.GetProperties();//获取属性
                foreach (var p in properties2)
                {
                    object[] PrimaryKey = p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                    if (!PrimaryKey.CollectionIsNullOrEmpty())
                    {
                        ss2 = p.Name;
                    }
                }
            }
            long sec2 = sw.ElapsedMilliseconds;
            Console.WriteLine($"反射优化{sec2}");//2564 毫秒 2596
            sw.Stop();
        }

        [TestMethod()]
        public void GetTableInfoTest()
        {
            //10次 4ms 0ms
            //100次 13 1 ， 6 1，4 0
            //1000次 6 11 ，8 11，6 8
            //100000次 264ms 864ms
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                var ss = GenericMappingProvider.GetClassConstruction(new DemoEntity());
                var tableInfo = (TableInfoAttribute)ss.AttributeList.FirstOrDefault(x => x.TypeId.ToString().Contains(nameof(TableInfoAttribute)));
                var ss1 = GenericMappingProvider.GetClassConstruction(new PersonEntity());
                var tableInfo2 = (TableInfoAttribute)ss1.AttributeList.FirstOrDefault(x => x.TypeId.ToString().Contains(nameof(TableInfoAttribute)));
                var ss2 = GenericMappingProvider.GetClassConstruction(new AnimalEntity());
                var tableInfo3= (TableInfoAttribute)ss2.AttributeList.FirstOrDefault(x => x.TypeId.ToString().Contains(nameof(TableInfoAttribute)));
            }
            long sec1 = sw.ElapsedMilliseconds;
            Console.WriteLine($"优化1：{sec1}");//265 毫秒 256
            sw.Reset();
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                Type type = typeof(DemoEntity);
                var tableInfo = (TableInfoAttribute)(type.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault());

                Type type1 = typeof(PersonEntity);
                var tableInfo2 = (TableInfoAttribute)(type1.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault());

                Type type2 = typeof(AnimalEntity);
                var tableInfo3 = (TableInfoAttribute)(type2.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault());
            }
            long sec2 = sw.ElapsedMilliseconds;
            Console.WriteLine($"反射优化{sec2}");//864 毫秒 854
            sw.Stop();
        }
    }
}