using HoangCuongSneaker.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Dto.Paging
{
    public class Sort
    {
        public string Field { get; set; } = string.Empty;
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;
    }
}
