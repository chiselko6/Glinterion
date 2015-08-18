using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Glinterion.Models;

namespace Glinterion.DAL.Repository
{
    public interface IPhotoRepository : IDisposable
    {
        IQueryable<Photo> GetPhotos();
        Photo GetPhotoById(int id);
        void AddPhoto(Photo photo);
        void DeletePhoto(int photoId);
        void UpdatePhoto(Photo photo);
        void Save();
    }
}
