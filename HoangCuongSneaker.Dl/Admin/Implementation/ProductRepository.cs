using HoangCuongSneaker.Core;
using HoangCuongSneaker.Core.Dto.Paging;
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
using Dapper;
using HoangCuongSneaker.Core.Utility;

namespace HoangCuongSneaker.Repository.Admin.Implementation
{
    public class ProductRepository : BaseRepository<ProductDto>, IProductRepository
    {
        public ProductRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override async Task<ProductDto> Create(ProductDto model)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                var procInsertProduct = "proc_product_insert";

                // mapper.map => productDto => product to store in the db

                var insertedProduct = await conn.ExecuteScalarAsync<ProductDto>(sql: procInsertProduct, commandType: System.Data.CommandType.StoredProcedure, param: model);

                if (insertedProduct != null)
                {
                    // luu image, product inventory
                    var procInsertImage = "proc_image_insert";
                    var procInsertProductInventory = "proc_product_inventory_insert";
                    int insertedImageCount = 0, insertedProductInventoryCount = 0;
                    // using transaction
                    for (int i = 0; i < insertedProduct.Images.Count; i++)
                    {
                        var affectedRow = await conn.ExecuteAsync(sql: procInsertImage, commandType: System.Data.CommandType.StoredProcedure, param: insertedProduct.Images[i]);
                        if (affectedRow == 1)
                        {
                            insertedImageCount++;
                        }
                    }
                    if (insertedImageCount != insertedProduct.Images.Count)
                    {
                        // rollback
                    }
                    for (int i = 0; i < insertedProduct.ProductInventories.Count; i++)
                    {
                        var affectedRow = await conn.ExecuteAsync(sql: procInsertProductInventory, commandType: System.Data.CommandType.StoredProcedure, param: insertedProduct.ProductInventories[i]);
                        if (affectedRow == 1)
                        {
                            insertedProductInventoryCount++; 
                        }
                    }
                    if (insertedProductInventoryCount != insertedProduct.ProductInventories.Count)
                    {
                        // rollback
                        trans.Rollback();
                    }
                    //trans.Commit();
                }
                else
                {
                    // roll back
                    trans.Rollback();
                }

                return insertedProduct;
            }
        }

        public async override Task<int> Delete(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "update product set is_active = false where id=@id";
                var affectedRow = await conn.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
                return affectedRow;
            }
        }

        public override async Task<ProductDto> Get(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "select * from product where id=@id";


                var product = await conn.QueryFirstOrDefaultAsync<Product>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
                if (product != null)
                {
                    var sqlProductInventories = "select * from product_inventory where product_id = @productId";
                    var sqlImages = "select * from image where product_id = @productId";

                    var productId = product.Id;
                    var productInventories = (await conn.QueryAsync<ProductInventory>(sql: sqlProductInventories, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();
                    var images = (await conn.QueryAsync<Image>(sql: sqlImages, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();

                    var sqlBrand = "select * from brand where id = @brandId";
                    var brandId = product.BrandId;
                    var brand = await conn.QueryFirstOrDefaultAsync<Brand>(sqlBrand, commandType: System.Data.CommandType.Text, param: new { @brandId = brandId });

                    // mapper.map product => productDto
                    var mapper = MapperUtil.Initialize();
                    var modelResult = mapper.Map<ProductDto>(product);
                    modelResult.Brand = brand;
                    modelResult.ProductInventories = productInventories;
                    modelResult.Images = images;

                    return modelResult;
                }
                return new ProductDto();
            }

        }

        public async override Task<List<ProductDto>> GetAll()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var sql = "select * from product";


                var products = (await conn.QueryAsync<Product>(sql: sql, commandType: System.Data.CommandType.Text)).ToList();
                if (products.Count > 0)
                {
                    var productsResult = new List<ProductDto>();
                    for (int i = 0; i < products.Count; i++)
                    {
                        var product = products.ElementAt(i);
                        var sqlProductInventories = "select * from product_inventory where product_id = @productId";
                        var sqlImages = "select * from image where product_id = @productId";

                        var productId = product.Id;
                        var productInventories = (await conn.QueryAsync<ProductInventory>(sql: sqlProductInventories, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();
                        var images = (await conn.QueryAsync<Image>(sql: sqlImages, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();

                        var sqlBrand = "select * from brand where id = @brandId";
                        var brandId = product.BrandId;
                        var brand = await conn.QueryFirstOrDefaultAsync<Brand>(sqlBrand, commandType: System.Data.CommandType.Text, param: new { @brandId = brandId });

                        // mapper.map product => productDto
                        var mapper = MapperUtil.Initialize();
                        var modelResult = mapper.Map<ProductDto>(product);
                        modelResult.Brand = brand;
                        modelResult.ProductInventories = productInventories;
                        modelResult.Images = images;

                        productsResult.Add( modelResult);

                    }
                    return productsResult;
                }
                return new List<ProductDto>();
            }
        }

        public override Task<PagingResponse<ProductDto>> GetPaging(PagingRequest pagingRequest)
        {

            return base.GetPaging(pagingRequest);
        }

        public async override Task<ProductDto> Update(ProductDto model)
        {
            var productFromDb = Get(model.Id);
            if (productFromDb != null)
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    var sql = "proc_product_update";

                    var updatedProduct = await conn.ExecuteScalarAsync<Product>(sql: sql, commandType: System.Data.CommandType.StoredProcedure, param: model);
                    if (updatedProduct is not null)
                    {
                        // get relevant data
                        var updatedProductDto = await Get(updatedProduct.Id);
                        return updatedProductDto;
                    }
                    return null;
                }
            }
            return null;
        }
    }
}
