using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Dto.Paging
{
    public abstract class PagingRequest
    {
        private int pageSize;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value <= 0) pageSize = 15;
                else pageSize = value;
            }
        }
        private int pageIndex;
        public int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                if (value < 0) pageIndex = 0;
                else pageIndex = value;
            }
        }

    }
}
