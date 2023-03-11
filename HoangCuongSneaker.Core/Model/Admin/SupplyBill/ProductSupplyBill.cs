using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.Admin.SupplyBill
{
    /// <summary>
    /// Sản phẩm trong 1 phiếu nhập
    /// </summary>
    public class ProductSupplyBill:BaseModel
    {
        public int ProductInventoryId { get; set; }
        public int SupplyBillId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }

    }
}
