using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Product;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Interface
{
    public interface IProductInventoryRepository: IBaseRepository<ProductInventory>
    {
        Task<PagingResponse<ProductInventoryDto>> GetPaging(ProductInventoryPagingRequest pagingRequest, MySqlConnection connection = null);
    }
}
