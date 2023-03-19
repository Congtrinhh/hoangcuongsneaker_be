using HoangCuongSneaker.Core.Dto.Paging;
using HoangCuongSneaker.Core.Model;

namespace HoangCuongSneaker.Core
{
    public class PagingResponse<T> where T : BaseModel
    {
        public int TotalRecord { get; set; }
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public PagingResponse()
        {
            Items = new List<T>();
            PageIndex = 0;
            PageSize = 20;

        }
    }
}