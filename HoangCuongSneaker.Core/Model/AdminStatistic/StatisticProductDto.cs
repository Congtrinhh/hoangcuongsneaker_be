using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model
{
    public class StatisticProductDto:BaseModel
    {
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ảnh đại diện của sản phẩm
        /// </summary>
        public string Avatar { get; set; } = string.Empty;
        /// <summary>
        /// giá sản phẩm
        /// </summary>
        //public decimal Price { get; set; } = 0;
        /// <summary>
        /// số lượng đã bán hôm nay
        /// </summary>
        public int QuantitySoldToday { get; set; } = 0;
        /// <summary>
        /// số lượng đã bán tuần này
        /// </summary>
        public int QuantitySoldWeek{ get; set; } = 0;
        /// <summary>
        /// số lượng đã bán tháng này
        /// </summary>
        public int QuantitySoldMonth { get; set; } = 0;
        /// <summary>
        /// số lượng đã bán theo tuần - để có thể theo dõi từng tuần
        /// </summary>
        public int QuantitySoldWeekly { get; set; } = 0;
        /// <summary>
        /// số lượng đã bán theo tháng - để có thể theo dõi từng tháng
        /// </summary>
        public int QuantitySoldMonthly { get; set; } = 0;
        /// <summary>
        /// số lượng hàng tồn kho - bao gồm tổng tất cả số lượng từ các product inventory của sản phẩm
        /// </summary>
        public int QuantityInventory { get; set; } = 0; 
    }
}
