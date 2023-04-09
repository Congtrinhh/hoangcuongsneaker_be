using HoangCuongSneaker.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model
{
    public class Order:BaseModel
    {
        public int UserId { get; set; }
        public bool? IsActive { get; set; } = true;
        // mã code để làm mã đơn hàng cho khách 
        public string Code { get; set; } = string.Empty;
        public ShippingStatusEnum ShippingStatus { get; set; }
        public DateTime? ShippedAt { get; set; }
        public decimal BillPrice { get; set; }
    }
}
