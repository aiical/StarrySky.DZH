﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.SQLORM.Common
{
    /// <summary>
    /// the type of database 
    /// </summary>
    public enum DBTypeEnum
    {
        MySQL,
        SqlServer,
        Oracle,
    }

    public enum DBOperateStatusEnum
    {
        Default,
        Add,
        Edit,
        Query,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 无效
        /// </summary>
        Invalid,

    }
}
