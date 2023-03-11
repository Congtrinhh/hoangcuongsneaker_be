using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Dto.Paging
{
    public class PagingRequest
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 0;

    }
}
