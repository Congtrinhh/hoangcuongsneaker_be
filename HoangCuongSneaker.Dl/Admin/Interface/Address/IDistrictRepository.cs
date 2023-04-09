using HoangCuongSneaker.Core.Model.Admin.Address;
using HoangCuongSneaker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Interface.Address
{
    public interface IDistrictRepository : IBaseRepository<District>
    {
        Task<List<District>> GetDistricts(int provinceId);
    }
}
