using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Enum
{
    public enum ShippingStatusEnum
    {
        // đơn hàng đã được tiếp nhận
        OrderAccepted = 1,
        // đang giao
        Delivering = 2,
        // đã giao
        Delivered = 3,
        // giao không thành công
        DeliverFailed = 4,
    }
}
