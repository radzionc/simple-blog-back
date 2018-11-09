using System.Collections.Generic;

namespace Blog.API.Notifications.Models
{
    public class StoryEditPayload
    {
      public string Id { get; set; }
      public string Title { get; set; }
      public long LastEditTime { get; set; }
      public List<string> Tags { get; set; }
      public string Content { get; set; }
    }
}