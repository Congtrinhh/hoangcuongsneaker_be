using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Dto.Paging.Admin
{
    public class ProductPagingRequest:PagingRequest
    {
        public string? Name { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public decimal? Price { get; set; }
    }
}
