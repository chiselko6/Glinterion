using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Glinterion.Models;

namespace Glinterion.DAL.Repository
{
    public interface IPhotoRepository : IDisposable
    {
        IQueryable<Photo> GetPhotos();
        IQueryable<Photo> GetPhotos(Expression<Func<Photo, bool>> predicate);
        Photo GetPhoto(int id);
        Photo GetPhoto(Expression<Func<Photo, bool>> predicate);
        IQueryable<Photo> GetPhotos(string userLogin);
        void AddPhoto(Photo photo);
        void DeletePhoto(int photoId);
        void UpdatePhoto(Photo photo);
        void Save();
    }
}
