using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.Admin.SupplyBill
{
    /// <summary>
    /// 1 sản phẩm trong phiếu nhập
    /// dùng khi:
    ///     + tạo mới 1 phiếu nhập
    ///     + hiển thị thông tin phiếu nhập trong màn hình chi tiết phiếu nhập
    /// </summary>
    public class ProductSupplyBillDto:BaseModel
    {
        public ProductInventory ProductInventory { get; set; } = new ProductInventory();
        public SupplyBill SupplyBill { get; set; } = new SupplyBill();
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
    }
}
