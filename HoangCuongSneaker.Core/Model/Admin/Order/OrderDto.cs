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
        public bool? IsActive { get; set; } = true;
        // mã code để làm mã đơn hàng cho khách 
        public string? Code { get; set; } = DateTime.Now.Millisecond.ToString();
        public ShippingStatusEnum? ShippingStatus { get; set; } = ShippingStatusEnum.PendingAccept;
        public DateTime? ShippedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        private decimal? billPrice;
        public decimal? BillPrice
        {
            get
            {
                decimal total = 0;
                OrderItems.ForEach(item =>
                {
                    total += item.Quantity * item.Price;
                });
                return total;
            }
            set
            {
                billPrice = value;
            }
        }
    }
}
