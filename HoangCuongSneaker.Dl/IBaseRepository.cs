using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        Task<T> Get(int id);
        Task<List<T>> GetAll();
        Task<PagingResponse<T>> GetPaging(PagingRequest pagingRequest);
        Task<T> Create(T model);
        Task<T> Update(T model);
        Task<int> Delete(int id);
    }
}
