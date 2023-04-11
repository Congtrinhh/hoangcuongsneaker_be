using HoangCuongSneaker.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model
{
    // Class Kho sản phẩm
    public class ProductInventory : BaseModel
    {
        /// <summary>
        /// mã duy nhất phân biệt các sản phẩm với nhau.
        /// vẫn cần dùng thêm id để dùng base crud
        /// </summary>
        public string Sku { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        // giá bán do nhà cung cấp đề xuất
        public bool? IsActive { get; set; } = true;
        public decimal? RrPrice { get; set; }
        // giá bán thực sẽ bán
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public GenderEnum Gender { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int? BrandId { get; set; }
    }
}
