using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Dto.Paging.Admin
{
    public class ProductInventoryPagingRequest:PagingRequest
    {
        public int? ProductId { get; set; }
    }
}
