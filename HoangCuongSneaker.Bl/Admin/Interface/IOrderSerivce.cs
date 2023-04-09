using HoangCuongSneaker.Core.Model.Admin.Order;
using HoangCuongSneaker.Core.Model.Admin.Product;
using HoangCuongSneaker.Repository.Admin.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Service.Admin
{
    public interface IOrderService:IBaseService<OrderDto>// be careful of generic Order or OrderDto
    {
        Task<OrderDto> Create(OrderDto model);
    }
}
