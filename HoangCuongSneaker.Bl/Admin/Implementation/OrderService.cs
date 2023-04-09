using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Order;
using HoangCuongSneaker.Core.Model.Admin.Product;
using HoangCuongSneaker.Repository;
using HoangCuongSneaker.Repository.Admin.Interface;
using HoangCuongSneaker.Service.Admin.Interface;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Service.Admin
{
    public class OrderService: BaseService<OrderDto>, IOrderService
    {
        protected IProductInventoryRepository _productInventoryRepository;
        protected IOrderRepository _orderRepository; 

        public OrderService(IConfiguration configuration, IOrderRepository orderRepository, IProductInventoryRepository productInventoryRepository) : base(configuration, orderRepository)
        { 
            _productInventoryRepository = productInventoryRepository;
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> Create(OrderDto model)
        {
            var connection = GetSqlConnection();
            // check sl đủ bán
            var res = await _orderRepository.Create(model, connection);
            return res;
        }
        
        /// <summary>
        /// Tìm trong ds sản phẩm trong giỏ hàng 1 sản phẩm đã hết hàng
        /// </summary>
        /// <param name="orderItems"></param>
        /// <returns>
        /// - Sản phẩm có số lượng trong kho không đủ đầu tiên tìm thấy
        /// - Nếu mọi sản phẩm trong giỏ hàng đều đủ, trả về null
        /// </returns>
        public async Task<ProductInventoryDto?> FindOutOfStockPrductInventory(List<OrderItemDto> orderItems)
        {
            // chỉ 1 item ko đủ => false  
            for (int i = 0; i < orderItems.Count; i++)
            {
                var item = orderItems[i];
                var productInventoryFromDb = await _productInventoryRepository.Get(item.Id);
                if (productInventoryFromDb.Quantity < item.Quantity)
                {
                    var result = _mapper.Map<ProductInventory, ProductInventoryDto>(productInventoryFromDb);
                    return result;
                }
            }
            return null;
        }
    }
}
