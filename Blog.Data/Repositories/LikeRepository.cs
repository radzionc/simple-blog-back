using Blog.Data.Abstract;
using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Blog.Data.Repositories
{
    public class LikeRepository : EntityBaseRepository<Like>, ILikeRepository
    {
        public LikeRepository(BlogContext context) : base(context) {}
    }
}