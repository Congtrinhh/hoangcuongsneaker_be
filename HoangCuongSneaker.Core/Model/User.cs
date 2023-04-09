using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model
{
    public class User : BaseModel
    {
        public string UserName { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public bool? IsActive { get; set; } = true;
        public string? Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public bool? EmailVerified { get; set; } = true;
        public string? VerificationCode { get; set; } = string.Empty;
    }
}
