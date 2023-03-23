using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Core.Model.FileUpload
{
    public class FileUploadModel
    {
        public IFormFile FileDetails { get; set; } 
    }
}
