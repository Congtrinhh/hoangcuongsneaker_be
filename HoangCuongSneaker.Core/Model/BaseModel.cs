using HoangCuongSneaker.Core.Enum;
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
        // trạng thái của model để lưu vào db
        public ModelStateEnum? ModelState { get; set; } = ModelStateEnum.None;
    }
}
