using System.Collections.Generic;

namespace Blog.API.ViewModels
{
    public class DraftViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public long LastEditTime { get; set; }
    }
}