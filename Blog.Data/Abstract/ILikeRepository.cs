using Blog.Model;

namespace Blog.Data.Abstract
{
    public interface ILikeRepository
    {
        void Add(Like entity);
        void Delete(Like entity);
        void Commit();
    }
}