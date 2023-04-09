using HoangCuongSneaker.Core.Model.Admin.Address;
using HoangCuongSneaker.Repository.Admin.Interface.Address;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Implementation.Address
{
    public class ProvinceRepository : BaseRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
