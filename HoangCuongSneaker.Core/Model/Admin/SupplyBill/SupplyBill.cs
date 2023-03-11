using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.Admin.SupplyBill
{
    /// <summary>
    /// Phiếu nhập
    /// </summary>
    public class SupplyBill:BaseModel
    {
        public int SupplierId { get; set; }
        public DateTime SupplyDate { get; set; }
        public string SupplyCode { get; set; } = string.Empty;
    }
}
