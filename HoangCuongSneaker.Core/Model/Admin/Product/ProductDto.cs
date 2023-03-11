using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.Admin.Product
{
    /// <summary>
    /// 1 sp trả về client
    /// </summary>
    public class ProductDto:BaseModel
    {
        // mapper
        // luu y - data table con thieu: bang product

        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// bằng giá của phần tử đầu tiên từ list product inventory
        /// </summary>
        public decimal Price { get; set; }
        public List<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();
        public List<Image> Images { get; set; } = new List<Image>();
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Brand Brand { get; set; } = new Brand();

    }
}
