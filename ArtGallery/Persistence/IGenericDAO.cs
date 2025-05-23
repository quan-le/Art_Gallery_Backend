using ArtGallery.Models;

namespace ArtGallery.Persistence
{
    public interface IGenericDAO<T> where T: class
    {
        T GetById(Guid id);
        List<T> GetAll();
        T Add(T entity);
        void Update(Guid id, T entity);
        void Delete(Guid id);
    }
}