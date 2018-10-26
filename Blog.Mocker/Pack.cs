using System.Collections.Generic;
using Blog.Model;

namespace Blog.Mocker
{
    public class Pack
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Story> Stories { get; set; } = new List<Story>();
    }
}