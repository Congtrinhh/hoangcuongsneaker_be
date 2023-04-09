using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model.Admin.Product;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Interface
{
    public interface IProductRepository:IBaseRepository<ProductDto>
    {
        /// <summary>
        /// get sản phẩm theo slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        /// 
        Task<ProductDto> GetBySlug(string slug, MySqlConnection connection = null); 

        Task<PagingResponse<ProductDto>> GetPaging(PagingRequest pagingRequest, MySqlConnection connection = null);
    }
}
