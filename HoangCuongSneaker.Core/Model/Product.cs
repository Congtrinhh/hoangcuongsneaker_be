using HoangCuongSneaker.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model
{
    /// <summary>
    /// Class sản phẩm
    /// </summary>
    public class Product:BaseModel
    {
        public string Name { get; set; } = string.Empty;
        private string? slug;
        public string Slug
        {
            get {
                var str = string.Empty;
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    str = FunctionUtil.Slugify(Name);
                }
                return str;
            }
            set { slug = value; }
        }
        public int BrandId { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? Description { get; set; }
        public bool? IsHot { get; set; } = false;
        public bool? IsBestSeller { get; set; } = false;
        public decimal? Price { get; set; } = 0;
    }
}
