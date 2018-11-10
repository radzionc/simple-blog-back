using System.Collections.Generic;
using Blog.Model;

namespace Blog.Data.Abstract
{
    public interface IShareRepository : IEntityBaseRepository<Share>
    {
        List<Story> StoriesSharedToUser(string userId);
    }
}