using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Service
{
    public class BaseService<T>:IBaseService<T> where T : BaseModel
    {
        public BaseService(IBaseRepository<T> baseRepository)
        {

        }
    }
}
