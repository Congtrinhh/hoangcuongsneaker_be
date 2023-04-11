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
using HoangCuongSneaker.Core.Dto.Paging.Admin;

namespace HoangCuongSneaker.Repository.Admin.Implementation
{
    public class ProductRepository : BaseRepository<ProductDto>, IProductRepository
    {
        protected ISizeRepository _sizeRepository;
        protected IColorRepository _colorRepository;
        protected IImageRepository _imageRepository;
        protected IProductInventoryRepository _productInventoryRepository;
        public ProductRepository(IConfiguration configuration, ISizeRepository sizeRepository, IColorRepository colorRepository, IImageRepository imageRepository, IProductInventoryRepository productInventoryRepository) : base(configuration)
        {
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
            _imageRepository = imageRepository;
            _productInventoryRepository = productInventoryRepository;
        }

        public override async Task<ProductDto?> Create(ProductDto model, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();
            transaction = transaction ?? connection.BeginTransaction();
            var procInsertProduct = "proc_product_insert";

            var productToInsert = _mapper.Map<Product>(model);
            var insertedProductId = await connection.ExecuteScalarAsync<int>(sql: procInsertProduct, commandType: System.Data.CommandType.StoredProcedure, param: productToInsert, transaction: transaction);

            if (insertedProductId > 0)
            {
                // luu image, product inventory
                var procInsertImage = "proc_image_insert";
                var procInsertProductInventory = "proc_product_inventory_insert";
                int insertedImageCount = 0, insertedProductInventoryCount = 0;

                for (int i = 0; i < model.Images.Count; i++)
                {
                    var insertModel = model.Images[i];
                    insertModel.ProductId = insertedProductId;
                    var insertedId = await connection.ExecuteScalarAsync<int>(sql: procInsertImage, commandType: System.Data.CommandType.StoredProcedure, param: insertModel, transaction: transaction);
                    if (insertedId > 0)
                    {
                        insertedImageCount++;
                    }
                }
                if (insertedImageCount != model.Images.Count)
                {
                    transaction.Rollback();
                    return null;
                }

                for (int i = 0; i < model.ProductInventories.Count; i++)
                {
                    var insertModel = _mapper.Map<ProductInventory>(model.ProductInventories[i]);
                    insertModel.ProductId = insertedProductId;
                    var insertedId = await connection.ExecuteScalarAsync<int>(sql: procInsertProductInventory, commandType: System.Data.CommandType.StoredProcedure, param: insertModel, transaction: transaction);
                    if (insertedId > 0)
                    {
                        insertedProductInventoryCount++;
                    }
                }
                if (insertedProductInventoryCount != model.ProductInventories.Count)
                {
                    transaction.Rollback();
                    return null;
                }
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
                return null;
            }

            // lấy ra sản phẩm vừa đc insert vào db và trả về client
            var newlyInsertedProduct = await Get(insertedProductId);
            if (newlyInsertedProduct != null)
            {
                newlyInsertedProduct.Images = model.Images;
                newlyInsertedProduct.ProductInventories = model.ProductInventories;
                return newlyInsertedProduct;

            }
            return null;
        }

        public async override Task<int> Delete(int id, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();

            connection.Open();

            var sql = "update product set is_active = false where id=@id";
            var affectedRow = await connection.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
            return affectedRow;
        }

        public override async Task<ProductDto?> Get(int id, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();

            connection.Open();

            var sql = "select * from product where id=@id";

            var product = await connection.QueryFirstOrDefaultAsync<Product>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @id = id });
            if (product != null)
            {
                var sqlProductInventories = "select * from product_inventory where product_id = @productId";
                var sqlImages = "select * from image where product_id = @productId";

                var productId = product.Id;
                var productInventories = (await connection.QueryAsync<ProductInventory>(sql: sqlProductInventories, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();
                var images = (await connection.QueryAsync<Image>(sql: sqlImages, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();

                var sqlBrand = "select * from brand where id = @brandId";
                var brandId = product.BrandId;
                var brand = await connection.QueryFirstOrDefaultAsync<Brand>(sqlBrand, commandType: System.Data.CommandType.Text, param: new { @brandId = brandId });

                var productInventoryDtos = await Task.WhenAll(productInventories.Select(async (productInventory) =>
                {
                    var tmpProductInventory = _mapper.Map<ProductInventoryDto>(productInventory);
                    var size = await _sizeRepository.Get(productInventory.SizeId);
                    var color = await _colorRepository.Get(productInventory.ColorId);
                    tmpProductInventory.Size = size;
                    tmpProductInventory.Color = color;
                    return tmpProductInventory;
                }));

                var modelResult = _mapper.Map<ProductDto>(product);
                modelResult.Brand = brand;
                modelResult.ProductInventories = productInventoryDtos.ToList();
                modelResult.Images = images;

                return modelResult;
            }
            return new ProductDto();
        }

        public async override Task<List<ProductDto>> GetAll(MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();

            connection.Open();

            var sql = "select * from product";


            var products = (await connection.QueryAsync<Product>(sql: sql, commandType: System.Data.CommandType.Text)).ToList();
            if (products.Count > 0)
            {
                var productsResult = new List<ProductDto>();
                for (int i = 0; i < products.Count; i++)
                {
                    var product = products.ElementAt(i);
                    var sqlProductInventories = "select * from product_inventory where product_id = @productId";
                    var sqlImages = "select * from image where product_id = @productId";

                    var productId = product.Id;
                    var productInventories = (await connection.QueryAsync<ProductInventory>(sql: sqlProductInventories, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();
                    var images = (await connection.QueryAsync<Image>(sql: sqlImages, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();

                    var sqlBrand = "select * from brand where id = @brandId";
                    var brandId = product.BrandId;
                    var brand = await connection.QueryFirstOrDefaultAsync<Brand>(sqlBrand, commandType: System.Data.CommandType.Text, param: new { @brandId = brandId });

                    var productInventoryDtos = await Task.WhenAll(productInventories.Select(async (productInventory) =>
                    {
                        var tmpProductInventory = _mapper.Map<ProductInventoryDto>(productInventory);
                        var size = await _sizeRepository.Get(productInventory.SizeId);
                        var color = await _colorRepository.Get(productInventory.ColorId);
                        tmpProductInventory.Size = size;
                        tmpProductInventory.Color = color;
                        return tmpProductInventory;
                    }));

                    var modelResult = _mapper.Map<ProductDto>(product);
                    modelResult.Brand = brand;
                    modelResult.ProductInventories = productInventoryDtos.ToList();
                    modelResult.Images = images;

                    productsResult.Add(modelResult);

                }
                return productsResult;
            }
            return new List<ProductDto>();
        }

        public async Task<ProductDto> GetBySlug(string slug, MySqlConnection connection = null)
        {
            connection = connection ?? GetSqlConnection();
            connection.Open();

            var sql = "select * from product where slug=@slug";


            var product = await connection.QueryFirstOrDefaultAsync<Product>(sql: sql, commandType: System.Data.CommandType.Text, param: new { @slug = slug });
            if (product != null)
            {
                var sqlProductInventories = "select * from product_inventory where product_id = @productId";
                var sqlImages = "select * from image where product_id = @productId";

                var productId = product.Id;
                var productInventories = (await connection.QueryAsync<ProductInventory>(sql: sqlProductInventories, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();
                var images = (await connection.QueryAsync<Image>(sql: sqlImages, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();

                var sqlBrand = "select * from brand where id = @brandId";
                var brandId = product.BrandId;
                var brand = await connection.QueryFirstOrDefaultAsync<Brand>(sqlBrand, commandType: System.Data.CommandType.Text, param: new { @brandId = brandId });


                var productInventoryDtos = await Task.WhenAll(productInventories.Select(async (productInventory) =>
                {
                    var tmpProductInventory = _mapper.Map<ProductInventoryDto>(productInventory);
                    var size = await _sizeRepository.Get(productInventory.SizeId);
                    var color = await _colorRepository.Get(productInventory.ColorId);
                    tmpProductInventory.Size = size;
                    tmpProductInventory.Color = color;
                    return tmpProductInventory;
                }));

                var modelResult = _mapper.Map<ProductDto>(product);
                modelResult.Brand = brand;
                modelResult.ProductInventories = productInventoryDtos.ToList();
                modelResult.Images = images;

                return modelResult;
            }
            return new ProductDto();
        }

        public async Task<PagingResponse<ProductDto>> GetPaging(PagingRequest pagingRequest, MySqlConnection connection = null)
        {

            connection = connection ?? GetSqlConnection();

            connection.Open();

            var sql = GetSqlGetPaging(pagingRequest);
            var sqlTotalCount = GetSqlGetTotalCountPaging(pagingRequest);

            var products = (await connection.QueryAsync<Product>(sql: sql, commandType: System.Data.CommandType.Text)).ToList();
            int totalCount = await connection.QueryFirstOrDefaultAsync<int>(sql: sqlTotalCount, commandType: System.Data.CommandType.Text); 
            if (products.Count > 0)
            {
                var productsResult = new List<ProductDto>();
                for (int i = 0; i < products.Count; i++)
                {
                    var product = products.ElementAt(i);
                    var sqlProductInventories = "select * from product_inventory where product_id = @productId";
                    var sqlImages = "select * from image where product_id = @productId";

                    var productId = product.Id;
                    var productInventories = (await connection.QueryAsync<ProductInventory>(sql: sqlProductInventories, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();
                    var images = (await connection.QueryAsync<Image>(sql: sqlImages, commandType: System.Data.CommandType.Text, param: new { @productId = productId })).ToList();

                    var sqlBrand = "select * from brand where id = @brandId";
                    var brandId = product.BrandId;
                    var brand = await connection.QueryFirstOrDefaultAsync<Brand>(sqlBrand, commandType: System.Data.CommandType.Text, param: new { @brandId = brandId });

                    var productInventoryDtos = await Task.WhenAll(productInventories.Select(async (productInventory) =>
                    {
                        var tmpProductInventory = _mapper.Map<ProductInventoryDto>(productInventory);
                        var size = await _sizeRepository.Get(productInventory.SizeId);
                        var color = await _colorRepository.Get(productInventory.ColorId);
                        tmpProductInventory.Size = size;
                        tmpProductInventory.Color = color;
                        return tmpProductInventory;
                    }));

                    var modelResult = _mapper.Map<ProductDto>(product);
                    modelResult.Brand = brand;
                    modelResult.ProductInventories = productInventoryDtos.ToList();
                    modelResult.Images = images;

                    productsResult.Add(modelResult);

                }
                var pagingResponse = new PagingResponse<ProductDto>();
                pagingResponse.Items = productsResult;
                pagingResponse.TotalRecord = totalCount;
                return pagingResponse;
            }
            return new PagingResponse<ProductDto>();
        }

        public override string GetSqlGetPaging(PagingRequest pagingRequest)
        {
            var sql = $"select * from product where 1=1";

            if (pagingRequest is ProductPagingRequest productPagingRequest)
            {
                if (!string.IsNullOrEmpty(productPagingRequest.FilterValue))
                {
                    sql += $" and name like '%{productPagingRequest.FilterValue}%'";
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
            var sql = "select count(1) from product where 1=1";

            if (pagingRequest is ProductPagingRequest productPagingRequest)
            {
                if (!string.IsNullOrEmpty(productPagingRequest.FilterValue))
                {
                    sql += $" and name like '%{productPagingRequest.FilterValue}%'";
                }
            }
            return sql;
        }

        public async override Task<ProductDto?> Update(ProductDto model, MySqlConnection connection = null, MySqlTransaction transaction = null)
        {
            connection = connection ?? GetSqlConnection();

            var productFromDb = Get(model.Id);
            if (productFromDb != null)
            {
                connection.Open();
                transaction = transaction ?? connection.BeginTransaction();

                var sql = "proc_product_update";

                var productToUpdate = _mapper.Map<Product>(model);

                var affectedRows = await connection.ExecuteAsync(sql: sql, commandType: System.Data.CommandType.StoredProcedure, param: productToUpdate, transaction: transaction);
                if (affectedRows > 0)
                {
                    // thêm/xoá ảnh; thêm/xoá/update sản phẩm 
                    int ImageOperationCount = 0, ProductInventoryOperationCount = 0;

                    for (int i = 0; i < model.Images.Count; i++)
                    {
                        var image = model.Images[i];
                        bool isSuccessful = await HandleImageOperation(image, connection, transaction);
                        if (isSuccessful)
                        {
                            ImageOperationCount++;
                        }
                    }
                    if (ImageOperationCount != model.Images.Count)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    for (int i = 0; i < model.ProductInventories.Count; i++)
                    {
                        var productInventory = model.ProductInventories[i];
                        bool isSuccessful = await HandleProductInventoryOperation(productInventory, connection, transaction);
                        if (isSuccessful)
                        {
                            ProductInventoryOperationCount++;
                        }
                    }
                    if (ProductInventoryOperationCount != model.ProductInventories.Count)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    transaction.Commit();

                    // get relevant data
                    var updatedProductDto = await Get(model.Id);
                    return updatedProductDto;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// xử lý khi update sản phẩm cha: thêm/xoá/update sản phẩm con
        /// </summary>
        /// <param name="productInventory"></param>
        /// <returns></returns>
        private async Task<bool> HandleProductInventoryOperation(ProductInventoryDto productInventory, MySqlConnection connection, MySqlTransaction transaction)
        {
            bool isSuccessful = false;

            if (productInventory.ModelState == Core.Enum.ModelStateEnum.Create)
            {
                var model = _mapper.Map<ProductInventory>(productInventory);
                var createdModel = await _productInventoryRepository.Create(model, connection, transaction: transaction);
                if (createdModel is not null)
                {
                    return true;
                }
                return false;
            }
            else if (productInventory.ModelState == Core.Enum.ModelStateEnum.Delete)
            {
                var affectedRows = await _productInventoryRepository.Delete(productInventory.Id);
                if (affectedRows > 0)
                {
                    return true;
                }
                return false;
            }
            else if (productInventory.ModelState == Core.Enum.ModelStateEnum.Update)
            {
                var model = _mapper.Map<ProductInventory>(productInventory);
                var updatedModel = await _productInventoryRepository.Update(model, connection, transaction: transaction);
                if (updatedModel is not null)
                {
                    return true;
                }
                return false;
            }
            else if (productInventory.ModelState == Core.Enum.ModelStateEnum.None)
            {
                return true;
            }

            return isSuccessful;
        }

        /// <summary>
        /// xử lý khi update sản phẩm cha: thêm/xoá ảnh
        /// </summary>
        /// <param name="image"></param>
        /// <returns>true nếu thành công; false nếu ngược lại</returns>
        private async Task<bool> HandleImageOperation(Image image, MySqlConnection connection, MySqlTransaction transaction)
        {
            bool isSuccessful = false;
            if (image.ModelState == Core.Enum.ModelStateEnum.Create)
            {
                var res = await _imageRepository.Create(image, connection, transaction: transaction);
                if (res is not null)
                {
                    return true;
                }
                return false;
            }
            else if (image.ModelState == Core.Enum.ModelStateEnum.Delete)
            {
                var res = await _imageRepository.Delete(image.Id);
                if (res > 0)
                {
                    return true;
                }
                return false;
            }
            else if (image.ModelState == Core.Enum.ModelStateEnum.None)
            {
                return true;
            }
            return isSuccessful;
        }
    }

}

