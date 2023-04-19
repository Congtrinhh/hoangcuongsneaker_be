using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository.Admin;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        public BrandRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public override string GetSqlGetAllOrderBy()
        {
            return " order by name ";
        }
    }
}
