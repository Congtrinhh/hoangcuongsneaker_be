using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Enum
{
    /// <summary>
    /// Trạng thái của 1 object đc gửi từ client lên server
    /// </summary>
    public enum ModelStateEnum
    {
        None= 1,
        Create= 2,
        Update= 3,
        Delete= 4,
    }
}
