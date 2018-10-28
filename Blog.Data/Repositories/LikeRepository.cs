using Blog.Data.Abstract;
using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Blog.Data.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private BlogContext _context;
        public LikeRepository(BlogContext context)
        {
            _context = context;
        }

        public void Add(Like entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<Like>(entity);
            _context.Set<Like>().Add(entity);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(Like entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<Like>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }
    }
}