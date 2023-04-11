using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model.Admin.SupplyBill;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Interface
{
    public interface ISupplyBillRepository: IBaseRepository<SupplyBillDto>
    {
        Task<PagingResponse<SupplyBillDto>> GetPaging(PagingRequest pagingRequest, MySqlConnection connection = null, MySqlTransaction transaction = null);
    }
}
