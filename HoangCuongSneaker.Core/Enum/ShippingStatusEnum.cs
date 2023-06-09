﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Enum
{
    /// <summary>
    /// Trạng thái đơn hàng
    /// </summary>
    public enum ShippingStatusEnum
    {
        /// đơn hàng chờ tiếp nhận
        PendingAccept = 1,
        // đơn hàng đã được tiếp nhận
        Accepted = 1,
        // đang giao
        Delivering = 2,
        // đã giao
        Delivered = 3,
        // giao không thành công
        DeliverFailed = 4,
    }
}
