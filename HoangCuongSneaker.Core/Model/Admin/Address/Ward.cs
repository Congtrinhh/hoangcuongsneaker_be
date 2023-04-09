using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.Admin.Address
{
    /// <summary>
    /// Class này chỉ dùng để hiển thị lên để lấy địa chỉ - không cần kế thừa từ Base
    /// </summary>
    public class Ward:BaseModel
    {
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public string? Name { get; set; }
        public string? Prefix { get; set; }
    }
}
