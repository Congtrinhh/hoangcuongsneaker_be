using HoangCuongSneaker.Core.Model.Admin.Address;
using HoangCuongSneaker.Repository.Admin.Interface.Address;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;

namespace HoangCuongSneaker.Repository.Admin.Implementation.Address
{
    public class DistrictRepository : BaseRepository<District>, IDistrictRepository
    {
        public DistrictRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<District>> GetDistricts(int provinceId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "select * from district where province_id=@provinceId";
                var districts = await conn.QueryAsync<District>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @provinceId = provinceId });
                if (districts is not null)
                {
                    return districts.ToList();
                }
                return new List<District>();
            }
        }
    }
}
