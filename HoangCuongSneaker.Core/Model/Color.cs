using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model
{
    public class Color : BaseModel
    {
        public string Value { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
    }
}
