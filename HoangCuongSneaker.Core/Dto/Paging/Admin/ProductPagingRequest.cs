﻿using HoangCuongSneaker.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Dto.Paging.Admin
{
    /// <summary>
    /// lọc sản phẩm thoả mãn tất cả điều kiện trong pagingRequest này
    /// </summary>
    public class ProductPagingRequest:PagingRequest
    {
        public string? Name { get; set; }
        /// <summary>
        /// tìm sản phẩm có size thuộc danh sách size
        /// </summary>
        public List<int>? SizeIds { get; set; } = new List<int>();
        /// <summary>
        /// tìm sản phẩm có màu thuộc danh sách màu
        /// </summary>
        public List<int>? ColorIds { get; set; } = new List<int>();
        /// <summary>
        /// tìm sản phẩm có giá trong khoảng giá thuộc danh sách khoảng giá
        /// </summary>
        public List<PriceRangeFilterEnum>? PriceRangeFilters { get; set; } = new List<PriceRangeFilterEnum>();
        public bool? IsHot { get; set; }
        public bool? IsBestSeller { get; set; }
        public bool? IsActive { get; set; }
        public GenderEnum? Gender { get; set; }
        public int? BrandId { get; set; } = 0;
        public List<int>? BrandIds { get; set; } = new List<int>();
        /// <summary>
        /// sort - chỉ đáp ứng sort theo 1 tiêu chí
        /// </summary>
        public SortOptionEnum? SortOption { get; set; }
    }
}
