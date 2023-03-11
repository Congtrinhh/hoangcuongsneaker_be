using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HoangCuongSneaker.Core.Model;
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
                //.ForMember(dest => dest.Dept, act => act.MapFrom(src => src.Department));
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
