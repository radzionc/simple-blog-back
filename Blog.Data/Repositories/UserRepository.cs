using Blog.Data.Abstract;
using Blog.Model;

namespace Blog.Data.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(BlogContext context)
            : base(context)
        { }
    }
}