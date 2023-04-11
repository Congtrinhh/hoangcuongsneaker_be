using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Enum
{
    /// <summary>
    /// Các option lọc ngày cho đơn hàng - lọc theo thời gian đơn hàng được tạo
    /// </summary>
    public enum DateFilterOptionEnum
    {
        Today = 1,
        ThisWeek = 2,
        ThisMonth = 3,
        All = 4,
    }
}
