using System.Collections.Generic;

namespace Blog.Model
{
  public class Story : IEntityBase
  {
    public Story()
    {
      Likes = new List<Like>();
      Shares = new List<Share>();
    }
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public long CreationTime { get; set; }
    public long LastEditTime { get; set; }
    public long PublishTime { get; set; }
    public bool Draft { get; set; }

    public User Owner { get; set; }
    public string OwnerId { get; set; }
    public List<Like> Likes { get; set; }

    public List<Share> Shares { get; set; }
  }
}