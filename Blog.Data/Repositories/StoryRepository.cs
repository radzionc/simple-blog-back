using Blog.Data.Abstract;
using Blog.Model;

namespace Blog.Data.Repositories
{
    public class StoryRepository : EntityBaseRepository<Story>, IStoryRepository 
    {
        public StoryRepository (BlogContext context) : base (context) { }

        public bool IsInvited(string storyId, string userId)
        {
            var story = this.GetSingle(s => s.Id == storyId, s => s.Shares);
            return story.Shares.Exists(s => s.UserId == userId);
        }

        public bool IsOwner(string storyId, string userId)
        {
            var story = this.GetSingle(s => s.Id == storyId);
            return story.OwnerId == userId;
        }
  }
}