using System.Collections.Generic;
using Blog.Model;

namespace Blog.Data.Abstract
{
    public interface IShareRepository : IBaseRepository<Share>
    {
        List<Story> StoriesSharedToUser(string userId);
    }
}