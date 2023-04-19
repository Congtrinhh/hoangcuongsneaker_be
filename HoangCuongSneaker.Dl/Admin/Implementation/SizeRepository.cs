using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Implementation
{
    public class SizeRepository : BaseRepository<Size>, ISizeRepository
    {
        public SizeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override string GetSqlGetAllOrderBy()
        {
            return " order by name ";
        }
    }
}
