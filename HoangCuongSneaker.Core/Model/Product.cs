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
        public string Slug { get; set; } = string.Empty ;
        public int BrandId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
