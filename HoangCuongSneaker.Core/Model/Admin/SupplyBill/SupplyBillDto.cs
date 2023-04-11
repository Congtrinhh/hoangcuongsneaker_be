using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.Admin.SupplyBill
{
    /// <summary>
    /// Phiếu nhập - chứa đầy đủ thông tin 1 phiếu nhập cần
    /// Dùng khi:
    ///     + tạo mới phiếu nhập
    ///     + hiện thông tin chi tiết phiếu nhập
    /// </summary>
    public class SupplyBillDto:BaseModel
    { 
        public DateTime SupplyDate { get; set; }
        public string SupplyCode { get; set; } = string.Empty;
        public List<ProductSupplyBillDto> SupplyBillItems { get; set; } = new List<ProductSupplyBillDto>();
        public Supplier Supplier { get; set; } = new Supplier();
        /// <summary>
        /// tổng giá trị của phiếu nhập
        /// </summary>
        private decimal? totalPrice;
        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in SupplyBillItems) 
                {
                    total += item.Quantity * item.Price;
                }
                return total;
            }
            set
            {
                totalPrice = value;
            }
        }
    }
}
