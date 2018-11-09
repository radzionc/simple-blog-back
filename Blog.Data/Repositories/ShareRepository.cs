using Blog.Data.Abstract;
using Blog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Blog.Data.Repositories
{
    public class ShareRepository : BaseRepository<Share>, IShareRepository
    {
      public ShareRepository(BlogContext context) : base(context) {}
    }
}