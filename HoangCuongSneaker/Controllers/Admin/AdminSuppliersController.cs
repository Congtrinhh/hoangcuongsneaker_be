using HoangCuongSneaker.Core.Model.Admin.SupplyBill;
using HoangCuongSneaker.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangCuongSneaker.Api.Controllers.Admin
{
    public class AdminSuppliersController : BaseController<Supplier>
    {
        public AdminSuppliersController(IBaseRepository<Supplier> baseRepository) : base(baseRepository)
        {
        }
    }
}
