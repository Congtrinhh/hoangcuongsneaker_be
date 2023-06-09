﻿using HoangCuongSneaker.Core.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.Admin.Product
{
    /// <summary>
    /// 1 sp trả về client
    /// </summary>
    public class ProductDto : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public List<ProductInventoryDto> ProductInventories { get; set; } = new List<ProductInventoryDto>();
        public List<Image> Images { get; set; } = new List<Image>();
        private string? slug;
        public string Slug
        {
            get
            {
                var str = string.Empty;
                if (!string.IsNullOrWhiteSpace(Name)) str = FunctionUtil.Slugify(Name);
                return str;
            }
            set { slug = value; }
        }
        public Brand Brand { get; set; } = new Brand();
        public bool? IsActive { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public bool? IsHot { get; set; } = false;
        public bool? IsBestSeller { get; set; } = false;
        public decimal? Price { get; set; } = 0;
        public List<IFormFile>? ImageFiles { get; set; }
    }
}
