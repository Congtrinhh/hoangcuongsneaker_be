using Dapper;
using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Core.Model.Admin.Product;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin
{
    public class ProductInventoryRepository : BaseRepository<ProductInventory>, IProductInventoryRepository
    {
        protected ISizeRepository _sizeRepository;
        protected IColorRepository _colorRepository;
        public ProductInventoryRepository(IConfiguration configuration, ISizeRepository sizeRepository, IColorRepository colorRepository) : base(configuration)
        {
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
        }

        public async Task<PagingResponse<ProductInventoryDto>> GetPaging(ProductInventoryPagingRequest pagingRequest, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();

            var sql = GetSqlGetPaging(pagingRequest);
            var productInventories = await connection.QueryAsync<ProductInventory>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @productId = pagingRequest.ProductId });
            if (productInventories is not null)
            {
                var response = new PagingResponse<ProductInventoryDto>();
                var productInventoryDtos = await Task.WhenAll(productInventories.Select(async (productInventory) =>
                {
                    var color = await _colorRepository.Get(productInventory.ColorId);
                    var size = await _sizeRepository.Get(productInventory.SizeId);

                    var dto = _mapper.Map<ProductInventoryDto>(productInventory);
                    dto.Color = color;
                    dto.Size = size;

                    return dto;
                }));
                response.Items = productInventoryDtos.ToList();
                response.TotalRecord = productInventoryDtos.ToList().Count;// TODO: lấy ra đc totalRecord thật vì nó # số bản ghi của lần lấy này

                return response;
            }
            return null;
        }

        protected override string GetSqlGetPaging(PagingRequest pagingRequest)
        {
            var sql = $"select * from {_tableName} where 1=1";

            if (pagingRequest is ProductInventoryPagingRequest productInventoryPagingRequest)
            {
                if (productInventoryPagingRequest.ProductId is not null && productInventoryPagingRequest.ProductId > 0)
                {
                    sql += $" and product_id={productInventoryPagingRequest.ProductId}";
                }
            }

            int limit = pagingRequest.PageSize;
            int offset = pagingRequest.PageSize * pagingRequest.PageIndex;
            sql += $" limit {limit} offset {offset}";

            return sql;
        }
    }
}
