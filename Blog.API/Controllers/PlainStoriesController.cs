using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blog.Data.Abstract;
using AutoMapper;
using Blog.API.ViewModels;
using Blog.Model;

namespace Blog.API.Controllers
{
    [Route("[controller]")]
    public class PlainStoriesController : Controller
    {
        IStoryRepository storyRepository;
        IMapper mapper;

        public PlainStoriesController(IStoryRepository storyRepository, IMapper mapper)
        {
            this.storyRepository = storyRepository;
            this.mapper = mapper;
        }

        [HttpGet()]
        public IActionResult Stories()
        {
            var stories = storyRepository.AllIncluding(s => s.Owner);
            var viewModel = new StoriesViewModel {
                Stories = stories.Select(mapper.Map<StoryViewModel>).ToList()
            };
            return View(viewModel);
        }

        [HttpGet("{id}")]
        public ActionResult<StoryDetailViewModel> StoryDetail(string id)
        {
            var story = storyRepository.GetSingle(s => s.Id == id, s => s.Owner, s => s.Likes);
            var userId = HttpContext.User.Identity.Name;
            var liked = story.Likes.Exists(l => l.UserId == userId);
            
            var viewModel = mapper.Map<Story, StoryDetailViewModel>(
                story,
                opt => opt.AfterMap((src, dest) => dest.Liked = liked)
            );

            return View(viewModel);
        }
    }
}