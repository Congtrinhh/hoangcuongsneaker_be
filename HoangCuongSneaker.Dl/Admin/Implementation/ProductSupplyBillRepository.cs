using HoangCuongSneaker.Core.Model.Admin.SupplyBill;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Implementation
{
    public class ProductSupplyBillRepository : BaseRepository<ProductSupplyBill>, IProductSupplyBillRepository
    {
        public ProductSupplyBillRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
