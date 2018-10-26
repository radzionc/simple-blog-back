using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Mocker
{
    class Program
    {
        static readonly List<string> MEDIUM_USERS = new List<string> {
            "@geekrodion",
            "@umairh",
            "@tomkuegler",
            "@benjaminhardy",
            "@krisgage",
            "@dan.jeffries",
            "@zdravko",
            "@JessicaLexicus",
            "@tiffany.sun",
            "@Michael_Spencer",
            "@larrykim",
            "@nicolascole77",
            "@alltopstartups",
            "@ngoeke"
        };
        static async Task Main(string[] args)
        {
            var medium = new Medium();
            var pack = await medium.GetPack(MEDIUM_USERS);

            var contextOptions = new DbContextOptionsBuilder<BlogContext>()
                .UseNpgsql("Server=localhost;Database=blog;Username=blogadmin;Password=blogadmin")
                .Options;
            var blogContext = new BlogContext(contextOptions);

            var usersRepository = new UserRepository(blogContext);
            var users = pack.Users.Where(u => usersRepository.IsUsernameUniq(u.Username)).ToList();
            users.ForEach(usersRepository.Add);
            usersRepository.Commit();
            Console.WriteLine($"{users.Count} new users added");

            var storiesRepository = new StoryRepository(blogContext);
            var stories = pack.Stories.Where(s => 
                storiesRepository.GetSingle(os => os.Title == s.Title && os.PublishTime == s.PublishTime) == null
            ).ToList();
            stories.ForEach(storiesRepository.Add);
            storiesRepository.Commit();
            Console.WriteLine($"{stories.Count} new stories added");
        }
    }
}