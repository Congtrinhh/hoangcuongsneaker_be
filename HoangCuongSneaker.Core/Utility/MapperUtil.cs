using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Order;
using HoangCuongSneaker.Core.Model.Admin.Product;

namespace HoangCuongSneaker.Core.Utility
{
    public class MapperUtil
    {
        public static Mapper Initialize()
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                //Configuring Employee and EmployeeDTO
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<ProductDto, Product>();

                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto, User>();

                cfg.CreateMap<Order, OrderDto>();

                cfg.CreateMap<ProductInventory, ProductInventoryDto>();
                cfg.CreateMap<ProductInventoryDto, ProductInventory>();

            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
