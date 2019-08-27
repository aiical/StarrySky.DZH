using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.EnumUtil
{
    /// <summary>
    /// 资金流水相关枚举
    /// </summary>
    public class MoneyBillEnums
    {
        /// <summary>
        /// 项目分类
        /// </summary>
        public enum ProjectClassify
        {
            /// <summary>
            /// 购物（自营）
            /// </summary>
            [Description("购物(自营)")]
            ShopZiYin = 3050101,
            /// <summary>
            /// 购物（拼多多）
            /// </summary>
            [Description("购物（拼多多）")]
            ShoppingPDD = 3050102,
            /// <summary>
            /// 购物（京东）
            /// </summary>
            [Description("购物（京东）")]
            ShoppingJD = 3050103,
            /// <summary>
            /// 好福利
            /// </summary>
            [Description("好福利")]
            GoodFuLi = 3050104,
            /// <summary>
            /// 猫掌柜
            /// </summary>
            [Description("猫掌柜")]
            CatViP = 3050105,
            /// <summary>
            /// 提现
            /// </summary>
            [Description("提现")]
            Withdraw = 3050106,
            /// <summary>
            /// 抽礼物
            /// </summary>
            [Description("抽礼物")]
            DrawGifts = 3050107,
            /// <summary>
            /// 开心辞典
            /// </summary>
            [Description("开心辞典")]
            FunDictory = 3050108,
            /// <summary>
            /// 新人礼
            /// </summary>
            [Description("新人礼")]
            NewManGift = 3050109,
            /// <summary>
            /// 白拿
            /// </summary>
            [Description("白拿")]
            BaiNa = 3050110,
            /// <summary>
            /// 任务中心
            /// </summary>
            [Description("任务中心")]
            MissionCenter = 3050111,
            /// <summary>
            /// 猜折扣
            /// </summary>
            [Description("猜折扣")]
            CaiZheKou = 3050112,
            /// <summary>
            /// 虚拟粉丝福利
            /// </summary>
            [Description("虚拟粉丝福利")]
            VirtualFanFuLi = 3050113,
            /// <summary>
            /// 用户红包
            /// </summary>
            [Description("用户红包")]
            UserRedMoney = 3050114
        }
        /// <summary>
        /// 主体
        /// </summary>
        public enum MainBody
        {
            /// <summary>
            /// 
            /// </summary>
            [Description("用户")]
            User = 3050201,
            /// <summary>
            /// 
            /// </summary>
            [Description("平台")]
            Plat = 3050202
        }
        /// <summary>
        /// 流水类型
        /// </summary>
        public enum FlowType
        {
            /// <summary>
            /// 收入
            /// </summary>
            [Description("收入")]
            Income = 3050301,
            /// <summary>
            /// 支出
            /// </summary>
            [Description("支出")]
            Pay = 3050302
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        public enum EventType
        {
            /// <summary>
            /// 抵扣
            /// </summary>
            [Description("抵扣")]
            Deduct = 3050401,
            /// <summary>
            /// 订单支付
            /// </summary>
            [Description("订单支付")]
            OrderPay = 3050402,
            /// <summary>
            /// 退款
            /// </summary>
            [Description("退款")]
            Refund = 3050403,
            /// <summary>
            /// 提现
            /// </summary>
            [Description("提现")]
            Withdraw = 3050404,
            /// <summary>
            /// 上级分成
            /// </summary>
            [Description("上级分成")]
            ShangJiFenCheng = 3050405,
            /// <summary>
            /// 用户收入
            /// </summary>
            [Description("用户收入")]
            UserIncome = 3050406,
            /// <summary>
            /// 平台收入
            /// </summary>
            [Description("平台收入")]
            PlatIncome = 3050407,
            /// <summary>
            /// 成本
            /// </summary>
            [Description("成本")]
            Cost = 3050408,
            /// <summary>
            /// 用户支出
            /// </summary>
            [Description("用户支出")]
            UserPay = 3050409

        }
        /// <summary>
        /// 流水状态
        /// </summary>
        public enum FlowStatus
        {
            /// <summary>
            /// 待核算
            /// </summary>
            [Description("待核算")]
            WaitCheck = 3050501,
            /// <summary>
            /// 【已废弃】已核算
            /// </summary>
            [Obsolete("废弃")]
            [Description("已核算")]
            Checked = 3050502,
            /// <summary>
            /// 取消核算
            /// </summary>
            [Description("取消核算")]
            CancelCheck = 3050503,
            /// <summary>
            /// 待入账
            /// </summary>
            [Description("待入账")]
            PendingEntry = 3050504,
            /// <summary>
            /// 已入账
            /// </summary>
            [Description("已入账")]
            Entered = 3050505,
            /// <summary>
            /// 取消入账
            /// </summary>
            [Description("取消入账")]
            CancelEntered = 3050506
        }
        /// <summary>
        /// 流水处理状态
        /// </summary>
        public enum FlowHandleStatus
        {
            /// <summary>
            /// 待执行
            /// </summary>
            [Description("待执行")]
            WatiConfirm = 3050601,
            /// <summary>
            /// 挂起
            /// </summary>
            [Description("挂起")]
            Hangup = 3050602,
            /// <summary>
            /// 已确认
            /// </summary>
            [Description("已确认")]
            Confirmed = 3050603,
            /// <summary>
            /// 已否决
            /// </summary>
            [Description("已否决")]
            Refuse = 3050604
        }

        /// <summary>
        /// 货币发放限制类型
        /// </summary>
        public enum CoinLimitType
        {
            /// <summary>
            /// 单笔金额限制
            /// </summary>
            [Description("单笔金额限制")]
            SingleMoney = 3050701,
            /// <summary>
            /// 单用户周期限制
            /// </summary>
            [Description("单用户周期限制")]
            SingleUser = 3050702,
            /// <summary>
            /// 单项目周期限制
            /// </summary>
            [Description("单项目周期限制")]
            singleProjectCyc = 3050703,
            /// <summary>
            /// 全局周期限制
            /// </summary>
            [Description("全局周期限制")]
            GlobalCyc = 3050704,
            /// <summary>
            /// 项目缓冲期限制
            /// </summary>
            [Description("项目缓冲期限制")]
            ProjectLimit = 3050705,
        }
        /// <summary>
        /// 货币限制周期
        /// </summary>
        public enum CoinLimitCyc
        {
            /// <summary>
            /// 24小时
            /// </summary>
            [Description("24小时")]
            OneDay = 3050801,
            /// <summary>
            /// 7天
            /// </summary>
            [Description("7天")]
            OneWeek = 3050802,
            /// <summary>
            /// 30天
            /// </summary>
            [Description("30天")]
            OneMonth = 3050803,
        }


    }
}
