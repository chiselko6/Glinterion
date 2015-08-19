using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Glinterion.Models;

namespace Glinterion.DAL.IRepository
{
    public interface IImageRepository
    {
        void Save(Stream dataStream, string userLogin, string photoDescription, double rating);

    }
}
