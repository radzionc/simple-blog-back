using System;
using System.Linq;
using AutoMapper;
using Blog.API.Notifications;
using Blog.API.Notifications.Models;
using Blog.API.Services.Abstraction;
using Blog.API.ViewModels;
using Blog.Data.Abstract;
using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        IStoryRepository storyRepository;
        ILikeRepository likeRepository;
        IUserRepository userRepository;
        IShareRepository shareRepository;
        IHubContext<NotificationsHub> hubContext;
        IMapper mapper;

        public StoriesController(
            IStoryRepository storyRepository,
            ILikeRepository likeRepository,
            IUserRepository userRepository,
            IShareRepository shareRepository,
            IHubContext<NotificationsHub> hubContext,
            IMapper mapper
        )
        {
            this.storyRepository = storyRepository;
            this.likeRepository = likeRepository;
            this.hubContext = hubContext;
            this.userRepository = userRepository;
            this.shareRepository = shareRepository;
            this.mapper = mapper;
        }

        [HttpGet()]
        public ActionResult<StoriesViewModel> GetStories()
        {
            var stories = storyRepository.AllIncluding(s => s.Owner);
            return new StoriesViewModel {
                Stories = stories.Select(mapper.Map<StoryViewModel>).ToList()
            };
        }

        [HttpGet("{id}")]
        public ActionResult<StoryDetailViewModel> GetStoryDetail(string id)
        {
            var story = storyRepository.GetSingle(s => s.Id == id, s => s.Owner, s => s.Likes);
            var userId = HttpContext.User.Identity.Name;
            var liked = story.Likes.Exists(l => l.UserId == userId);
            
            return mapper.Map<Story, StoryDetailViewModel>(
                story,
                opt => opt.AfterMap((src, dest) => dest.Liked = liked)
            );
        }
        
        [HttpPost]
        public ActionResult<StoryCreationViewModel> Post([FromBody]UpdateStoryViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ownerId = HttpContext.User.Identity.Name;
            var creationTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            var storyId = Guid.NewGuid().ToString();
            var story = new Story {
                Id = storyId,
                Title = model.Title,
                Content = model.Content,
                Tags = model.Tags,
                CreationTime = creationTime,
                LastEditTime = creationTime,
                OwnerId = ownerId,
                Draft = true
            };

            storyRepository.Add(story);
            storyRepository.Commit();

            return new StoryCreationViewModel {
                StoryId = storyId
            };
        }

        [HttpPatch("{id}")]
        public ActionResult Patch(string id, [FromBody]UpdateStoryViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var userId = HttpContext.User.Identity.Name;
            if (!storyRepository.IsOwner(id, userId) || !storyRepository.IsInvited(id, userId)) return Forbid("You are not the owner of this story");

            var newStory = storyRepository.GetSingle(id);
            newStory.Title = model.Title;
            newStory.LastEditTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            newStory.Tags = model.Tags;
            newStory.Content = model.Content;

            storyRepository.Update(newStory);
            storyRepository.Commit();

            return NoContent();
        }

        [HttpPost("{id}/publish")]
        public ActionResult Publish(string id)
        {
            var ownerId = HttpContext.User.Identity.Name;
            if (!storyRepository.IsOwner(id, ownerId)) return Forbid("You are not the owner of this story");

            var newStory = storyRepository.GetSingle(id);
            newStory.Draft = false;
            newStory.PublishTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

            storyRepository.Update(newStory);
            storyRepository.Commit();

            return NoContent();
        }

        [HttpPost("{id}/share")]
        public ActionResult Share(string id, [FromBody]ShareViewModel model)
        {
            var ownerId = HttpContext.User.Identity.Name;
            if (!storyRepository.IsOwner(id, ownerId)) return Forbid("You are not the owner of this story");

            var userToShare = userRepository.GetSingle(u => u.Username == model.Username);
            if (userToShare == null) {
                return BadRequest(new { username = "No user with this name" });
            }
            var owner = userRepository.GetSingle(ownerId);
            var story = storyRepository.GetSingle(s => s.Id == id, s => s.Shares);
            var existingShare = story.Shares.Find(l => l.UserId == userToShare.Id);
            if (existingShare == null)
            {
                shareRepository.Add(new Share
                {
                    UserId = userToShare.Id,
                    StoryId = id
                });
                shareRepository.Commit();
            }
            return NoContent();
        }

        [HttpGet("drafts")]
        public ActionResult<DraftsViewModel> GetDrafts()
        {
            var ownerId = HttpContext.User.Identity.Name;

            var drafts = storyRepository.FindBy(story => story.OwnerId == ownerId && story.Draft);
            return new DraftsViewModel {
                Stories = drafts.Select(mapper.Map<DraftViewModel>).ToList()
            };
        }

        [HttpGet("shared")]
        public ActionResult<SharedDraftsViewModel> GetSharedToYouDrafts()
        {
            var userId = HttpContext.User.Identity.Name;

            var stories = shareRepository.StoriesSharedToUser(userId).Where(s => s.Draft);
            var usernames = stories.Select(s => s.Owner.Username).Distinct().ToList();
            
            return new SharedDraftsViewModel {
                UsersDrafts = usernames.Select(username => new UserDrafts {
                    Username = username,
                    Drafts = stories
                        .Where(s => s.Owner.Username == username)
                        .Select(mapper.Map<DraftViewModel>)
                        .ToList()
                }).ToList()
            };
        }

        [HttpGet("user/{id}")]
        public ActionResult<OwnerStoriesViewModel> Get(string id)
        {
            var stories = storyRepository.FindBy(story => story.OwnerId == id && !story.Draft);
            return new OwnerStoriesViewModel {
                Stories = stories.Select(mapper.Map<OwnerStoryViewModel>).ToList()
            };
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var ownerId = HttpContext.User.Identity.Name;
            if (!storyRepository.IsOwner(id, ownerId)) return Forbid("You are not the owner of this story");

            storyRepository.DeleteWhere(story => story.Id == id);
            storyRepository.Commit();

            return NoContent();
        }

        [HttpPost("{id}/toggleLike")]
        public ActionResult ToggleLike(string id)
        {
            var userId = HttpContext.User.Identity.Name;

            var story = storyRepository.GetSingle(s => s.Id == id, s => s.Likes);
            if (userId == story.OwnerId) return BadRequest("You can't like your own story");

            var user = userRepository.GetSingle(userId);
            var existingLike = story.Likes.Find(l => l.UserId == userId);
            var payload = new LikeRelatedPayload
            {
                Username = user.Username,
                StoryTitle = story.Title
            };
            if (existingLike == null)
            {
                hubContext.Clients.User(story.OwnerId).SendAsync(
                    "notification",
                    new Notification<LikeRelatedPayload>
                    {
                        NotificationType = NotificationType.LIKE,
                        Payload = payload
                    }
                );
                likeRepository.Add(new Like
                {
                    UserId = userId,
                    StoryId = id
                });
            }
            else 
            {
                hubContext.Clients.User(story.OwnerId).SendAsync(
                    "notification",
                    new Notification<LikeRelatedPayload>
                    {
                        NotificationType = NotificationType.UNLIKE,
                        Payload = payload
                    }
                );
                likeRepository.Delete(existingLike);
            }
            likeRepository.Commit();
            return NoContent();
        }
    }
}