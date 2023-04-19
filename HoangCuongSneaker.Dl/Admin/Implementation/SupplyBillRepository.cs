using Dapper;
using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Dto.Paging.Admin;
using HoangCuongSneaker.Core.Model.Admin.SupplyBill;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Implementation
{
    public class SupplyBillRepository : BaseRepository<SupplyBillDto>, ISupplyBillRepository
    {
        protected IProductInventoryRepository _productInventoryRepository;
        protected ISupplierRepository _supplierRepository;
        protected IProductSupplyBillRepository _productSupplyBillRepository;
        public SupplyBillRepository(IConfiguration configuration, IProductInventoryRepository productInventoryRepository, ISupplierRepository supplierRepository, IProductSupplyBillRepository productSupplyBillRepository) : base(configuration)
        {
            _productInventoryRepository = productInventoryRepository;
            _supplierRepository = supplierRepository;
            _productSupplyBillRepository = productSupplyBillRepository;
        }

        public override async Task<SupplyBillDto?> Get(int id, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            var sql = "select * from supply_bill where id=@id";

            var queriedSupplyBill = await connection.QueryFirstOrDefaultAsync<SupplyBill>(sql: sql, param: new { @Id = id }, commandType: System.Data.CommandType.Text);
            if (queriedSupplyBill is not null)
            {
                var sqlSelectProductSupplyBill = "select * from product_supply_bill where supply_bill_id=@supplyBillId";

                var supplyBillDto = _mapper.Map<SupplyBillDto>(queriedSupplyBill);

                var queriedProductSupplyBills = (await connection.QueryAsync<ProductSupplyBill>(sql: sqlSelectProductSupplyBill,
                    commandType: System.Data.CommandType.Text, param: new { @supplyBillId = queriedSupplyBill.Id })).ToList();

                var productSupplyBillDtos = new List<ProductSupplyBillDto>();
                foreach (var productSupplyBill in queriedProductSupplyBills)
                {
                    var productSupplyBillDto = _mapper.Map<ProductSupplyBillDto>(productSupplyBill);

                    var productInventory = await _productInventoryRepository.Get(productSupplyBill.ProductInventoryId);
                    productSupplyBillDto.ProductInventory = productInventory;
                    productSupplyBillDto.SupplyBill = queriedSupplyBill;

                    productSupplyBillDtos.Add(productSupplyBillDto);
                }
                var supplier = await _supplierRepository.Get(queriedSupplyBill.Id);
                supplyBillDto.Supplier = supplier;
                supplyBillDto.SupplyBillItems = productSupplyBillDtos;
                return supplyBillDto;
            }
            return null;
        }

        public async Task<PagingResponse<SupplyBillDto>> GetPaging(PagingRequest pagingRequest, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            connection = connection ?? GetSqlConnection();

            var sqlPaging = GetSqlGetPaging(pagingRequest);
            var sqlTotalCount = GetSqlGetTotalCountPaging(pagingRequest);
            var queriedSupplyBills = (await connection.QueryAsync<SupplyBill>(sql: sqlPaging, commandType: System.Data.CommandType.Text)).ToList();

            int totalCount = await connection.QueryFirstOrDefaultAsync<int>(sql: sqlTotalCount, commandType: System.Data.CommandType.Text);

            var supplyBillDtos = new List<SupplyBillDto>();

            var sqlSelectProductSupplyBill = "select * from product_supply_bill where supply_bill_id=@supplyBillId";
            foreach (var supplyBill in queriedSupplyBills)
            {
                var supplyBillDto = _mapper.Map<SupplyBillDto>(supplyBill);
                var supplier = await _supplierRepository.Get(supplyBill.SupplierId);
                supplyBillDto.Supplier = supplier;
                var queriedProductSupplyBills = (await connection.QueryAsync<ProductSupplyBill>(sql: sqlSelectProductSupplyBill,
                    commandType: System.Data.CommandType.Text, param: new { @supplyBillId = supplyBill.Id })).ToList();

                var productSupplyBillDtos = new List<ProductSupplyBillDto>();
                foreach (var productSupplyBill in queriedProductSupplyBills)
                {
                    var productSupplyBillDto = _mapper.Map<ProductSupplyBillDto>(productSupplyBill);

                    var productInventory = await _productInventoryRepository.Get(productSupplyBill.ProductInventoryId);
                    productSupplyBillDto.ProductInventory = productInventory;
                    productSupplyBillDto.SupplyBill = supplyBill;

                    productSupplyBillDtos.Add(productSupplyBillDto);
                }
                supplyBillDto.SupplyBillItems = productSupplyBillDtos;

                supplyBillDtos.Add(supplyBillDto);
            }

            var response = new PagingResponse<SupplyBillDto>();
            response.Items = supplyBillDtos;
            response.TotalRecord = totalCount;

            return response;
        }

        public override string GetSqlGetPaging(PagingRequest pagingRequest)
        {
            var sql = $"select * from supply_bill where 1=1";

            if (pagingRequest is SupplyBillPagingRequest p)
            {
                if (!string.IsNullOrEmpty(p.SupplyCode))
                {
                    sql += $" and supply_code like '%{p.SupplyCode}%'";
                }
            }

            int limit = pagingRequest.PageSize;
            int offset = pagingRequest.PageSize * pagingRequest.PageIndex;
            sql += " order by updated_at desc, created_at desc ";
            sql += $" limit {limit} offset {offset}";

            return sql;
        }

        public override string GetSqlGetTotalCountPaging(PagingRequest pagingRequest)
        {
            var sql = "select count(1) from supply_bill where 1=1";

            if (pagingRequest is SupplyBillPagingRequest p)
            {
                if (!string.IsNullOrEmpty(p.SupplyCode))
                {
                    sql += $" and supply_code like '%{p.SupplyCode}%'";
                }
            }

            return sql;
        }

        public override async Task<SupplyBillDto?> Create(SupplyBillDto model, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();
            transaction = transaction ?? connection.BeginTransaction();
            var procSupplyBillInsert = "proc_supply_bill_insert";

            var modelToInsert = _mapper.Map<SupplyBill>(model);
            var insertedId = await connection.ExecuteScalarAsync<int>(sql: procSupplyBillInsert, param: modelToInsert, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);
            if (insertedId > 0)
            {
                int createdItemCount = 0;
                foreach (var productSupplyBillDto in model.SupplyBillItems)
                {
                    var productSupplyBillToInsert = _mapper.Map<ProductSupplyBill>(productSupplyBillDto);
                    productSupplyBillToInsert.SupplyBillId = insertedId;
                    var insertedProductSupplyBill = await _productSupplyBillRepository.Create(productSupplyBillToInsert, connection, transaction);

                    if (insertedProductSupplyBill is not null) createdItemCount++;
                }
                if (createdItemCount != model.SupplyBillItems.Count)
                {
                    transaction.Rollback();
                    return null;
                }

                // update số lượng sp theo sl mới nhập
                int updatedProductInventoryCount = 0;
                foreach (var productSupplyBillDto in model.SupplyBillItems)
                {
                    var productInventoryId = productSupplyBillDto.ProductInventory.Id;
                    var productInventory = await _productInventoryRepository.Get(productInventoryId);
                    if (productInventory is not null)
                    {
                        productInventory.Quantity += productSupplyBillDto.Quantity;
                        var updatedProductInventory = await _productInventoryRepository.Update(productInventory, connection, transaction);
                        if (updatedProductInventory is not null) updatedProductInventoryCount++;
                    }
                }
                if (updatedProductInventoryCount != model.SupplyBillItems.Count)
                {
                    transaction.Rollback();
                    return null;
                }

                transaction.Commit();
                return model;
            }
            transaction.Rollback();
            return null;
        }

        public override Task<SupplyBillDto?> Update(SupplyBillDto model, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            return base.Update(model, connection, transaction);
        }

        public override Task<int> Delete(int id, MySqlConnection connection = null)
        {
            return base.Delete(id, connection);
        }
    }
}
