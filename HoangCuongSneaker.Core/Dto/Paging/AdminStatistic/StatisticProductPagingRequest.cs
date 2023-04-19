using HoangCuongSneaker.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Dto.Paging.AdminStatistic
{
    /// <summary>
    /// paging phần admin dashboard
    /// </summary>
    public class StatisticProductPagingRequest : PagingRequest
    {
        public static int DefaultLimit = 5;
        public static DateTime BaseDate = DateTime.Today;

        public DateTime StartDate { get; set; } = BaseDate.AddDays(1 - BaseDate.Day);
        public DateTime EndDate { get; set; } = BaseDate.AddDays(1 - BaseDate.Day).AddMonths(1).AddSeconds(-1);
        public StatisticProductTypeEnum StatisticProductType { get; set; } = StatisticProductTypeEnum.BestSeller;
        /// <summary>
        /// giới hạn số sp muốn lấy
        /// </summary>
        public int Limit { get; set; } = DefaultLimit;
    }
}
