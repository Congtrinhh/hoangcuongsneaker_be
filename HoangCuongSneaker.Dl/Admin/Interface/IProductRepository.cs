using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model.Admin.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Interface
{
    public interface IProductRepository:IBaseRepository<ProductDto>
    {
        Task<ProductDto> GetBySlug(string slug);
    }
}
