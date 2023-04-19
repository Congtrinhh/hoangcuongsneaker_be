using HoangCuongSneaker.Core.Dto.Paging.AdminStatistic;
using HoangCuongSneaker.Core.Enum;
using HoangCuongSneaker.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.AdminStatistic
{
    public interface IAdminStatisticRepository:IBaseRepository<BaseModel>
    {
        /// <summary>
        /// lấy ra danh sách sản phẩm cho trang admin dashboard tuỳ vào điều kiện lọc
        /// có thể là sản phẩm bán chạy nhất
        /// có thể là sản phẩm tồn kho nhiều nhất
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        Task<List<StatisticProductDto>> GetProducts(StatisticProductPagingRequest pagingRequest);

        Task<List<StatisticItem>> GetStatisticItems(StatisticTypeEnum statisticType);
    }
}
