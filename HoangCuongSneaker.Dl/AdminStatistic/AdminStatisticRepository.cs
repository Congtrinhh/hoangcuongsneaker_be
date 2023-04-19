using Dapper;
using HoangCuongSneaker.Core.Dto.Paging.AdminStatistic;
using HoangCuongSneaker.Core.Enum;
using HoangCuongSneaker.Core.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.AdminStatistic
{
    public class AdminStatisticRepository : BaseRepository<BaseModel>, IAdminStatisticRepository
    {
        public AdminStatisticRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<StatisticProductDto>> GetProducts(StatisticProductPagingRequest pagingRequest)
        {
            var connection = GetSqlConnection();
            connection.Open();

            string sql = GetSqlSelectProducts(pagingRequest);
            var products = (await connection.QueryAsync<StatisticProductDto>(sql, commandType: System.Data.CommandType.Text)).ToList();

            return products;
        }

        public Task<List<StatisticItem>> GetStatisticItems(StatisticTypeEnum statisticType)
        {
            throw new NotImplementedException();
        }

        private string GetSqlSelectProducts(StatisticProductPagingRequest pagingRequest)
        {
            var sql = string.Empty;
            if (pagingRequest.StatisticProductType== StatisticProductTypeEnum.BestSeller)
            {
                sql = $"SELECT p.id, p.name , SUM(oi.quantity) as QuantitySoldMonth FROM  purchase_order po JOIN order_item oi ON po.id = oi.order_id  JOIN product_inventory pi ON oi.product_inventory_id = pi.id   JOIN product p ON pi.product_id = p.id WHERE  po.created_at BETWEEN '{pagingRequest.StartDate.ToString("yyyy-MM-dd h:mm:ss tt")}' AND '{pagingRequest.EndDate.ToString("yyyy-MM-dd h:mm:ss tt")}' GROUP BY p.id ORDER BY SUM(oi.quantity) DESC LIMIT {pagingRequest.Limit}; ";
            }
            else if (pagingRequest.StatisticProductType == StatisticProductTypeEnum.BestInventory)
            {
                sql = $"SELECT p.id, p.name , SUM(pi.quantity) as QuantityInventory FROM   product_inventory pi   JOIN product p ON pi.product_id = p.id WHERE   1 = 1 GROUP BY p.id ORDER BY SUM(pi.quantity) DESC LIMIT {pagingRequest.Limit}";
            }
            return sql;
        }
    }
}
