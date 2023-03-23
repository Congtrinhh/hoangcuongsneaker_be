using HoangCuongSneaker.Core.Model;
using HoangCuongSneaker.Repository.Admin.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangCuongSneaker.Repository.Admin.Implementation
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        public ImageRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
