using HoangCuongSneaker.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.Admin.Product
{
    public class ProductInventoryDto : BaseModel
    {
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
        public Size Size { get; set; }
        public Color Color { get; set; }

        private decimal? discount;
        public decimal? Discount
        {
            get
            {
                if (SellPrice == 0 || RrPrice == 0) return 0;
                decimal discountPercent = ((decimal)((decimal)1.0 * SellPrice / RrPrice));
                if (discountPercent < 1)
                {
                    return discountPercent;
                }
                return 0;
            }
            set { discount = value; }
        }
    }
}
