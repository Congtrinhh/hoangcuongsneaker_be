using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model
{
    public class Image:BaseModel
    {
        public int ProductInventoryId { get; set; }
        // chứa dữ liệu thực sự của ảnh để hiển thị lên giao diện
        public object Content { get; set; }
        public string Desc { get; set; }
    }
}
