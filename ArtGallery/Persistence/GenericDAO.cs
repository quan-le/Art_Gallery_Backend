
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Persistence
{
    public class GenericDAO<T> : IGenericDAO<T> where T : class
    {
        private readonly GalleryDBContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericDAO(GalleryDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public T Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Delete(Guid id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public T GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public void Update(Guid id, T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }
    }
}
