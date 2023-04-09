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
    public class WardRepository : BaseRepository<Ward>, IWardRepository
    {
        public WardRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<Ward>> GetWards(int districtId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "select * from ward where district_id=@districtId";
                var wards = await conn.QueryAsync<Ward>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @districtId = districtId });
                if (wards is not null)
                {
                    return wards.ToList();
                }
                return new List<Ward>();
            }
        }
    }
}
