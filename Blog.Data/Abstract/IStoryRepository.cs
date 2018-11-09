using Blog.Model;

namespace Blog.Data.Abstract
{
    public interface IStoryRepository: IEntityBaseRepository<Story>
    {
        bool IsOwner(string storyId, string userId);
        bool IsInvited(string storyId, string userId);
    }
}