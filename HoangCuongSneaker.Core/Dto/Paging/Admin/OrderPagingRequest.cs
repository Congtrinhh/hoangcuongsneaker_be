using HoangCuongSneaker.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Dto.Paging.Admin
{
    public class OrderPagingRequest:PagingRequest
    {
        public ShippingStatusEnum? ShippingStatus { get; set; }

        /// <summary>
        /// tìm kiếm đơn hàng được tạo từ ngày => đến ngày
        /// </summary>
        public DateFilterOptionEnum? DateFilter { get; set; } = DateFilterOptionEnum.Today; 
    }
}
