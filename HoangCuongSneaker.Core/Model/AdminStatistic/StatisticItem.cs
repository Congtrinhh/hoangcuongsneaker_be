using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model
{
    /// <summary>
    /// 1 item của biểu đồ thống kê
    /// ví dụ: thống kê doanh thu trong 1 tuần gồm có 1 list StatisticItem. mỗi StatisticItem sẽ có dạng: {Text: 'Thứ 7', Value: 70000000 vnđ}
    /// </summary>
    public class StatisticItem
    {
        public string Text { get; set; }=string.Empty;
        public object Value { get; set; }
    }
}
