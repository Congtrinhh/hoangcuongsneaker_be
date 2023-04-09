using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.Admin.Order
{
    public class OrderItemDto:BaseModel
    {
        public int ProductInventoryId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal Price{ get; set; }
    }
}
