using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model
{
    public class BaseModel
    {
        // khoá chính
        public int Id { get; set; }
        // thời gian model được tạo
        public DateTime CreatedAt { get; set; }
        // id người tạo
        public int CreatedBy { get; set; }
        // thời gian model được sửa
        public DateTime UpdatedAt { get; set; }
        // id người sửa
        public int UpdatedBy { get; set; }
    }
}
