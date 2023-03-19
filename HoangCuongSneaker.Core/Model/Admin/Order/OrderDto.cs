using HoangCuongSneaker.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.Admin.Order
{
    public class OrderDto:BaseModel
    {
        public UserDto User { get; set; } = new UserDto();
        public bool IsActive { get; set; }
        // mã code để làm mã đơn hàng cho khách 
        public string Code { get; set; } = string.Empty;
        public ShippingStatusEnum ShippingStatus { get; set; }
        public DateTime ShippedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public decimal BillPrice { get; set; }
    }
}
