using AutoMapper;
using Blog.Model;

namespace Blog.API.ViewModels.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Story, StoryDetailViewModel>()
                .ForMember(s => s.OwnerUsername, map => map.MapFrom(s => s.Owner.Username))
                .ForMember(s => s.LikesNumber, map => map.MapFrom(s => s.Likes.Count))
                .ForMember(s => s.Liked, map => map.Ignore());
            CreateMap<Story, DraftViewModel>();
            CreateMap<Story, OwnerStoryViewModel>();
            CreateMap<Story, StoryViewModel>()
                .ForMember(s => s.OwnerUsername, map => map.MapFrom(s => s.Owner.Username));
        }
    }
}