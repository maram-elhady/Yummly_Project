using AutoMapper;
using Yummly.DTO.Activities;
using Yummly.DTO.Follow;
using Yummly.DTO.Like;
using Yummly.DTO.Post;
using Yummly.Models.DbModels;

namespace Yummly.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<PostModel, CreatePostDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, src => src.MapFrom(src => src.PostId));

            CreateMap<GetUserPostsDto, PostModel>()
                .ReverseMap()
                .ForMember(dest => dest.PostId , src=>src.MapFrom(src=>src.Id))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var baseUrl = context.Items["BaseUrl"]?.ToString();
                    return string.IsNullOrEmpty(src.ImageUrl) ? null : $"{baseUrl}{src.ImageUrl.Replace("wwwroot/", "")}";
                }));

            CreateMap<PostLikeModel, LikeDto>()
                .ReverseMap();

            CreateMap<PostCommentModel, AddCommentDto>()
                .ReverseMap();

            CreateMap<EditCommentDto, PostCommentModel>()
                .ReverseMap();

            CreateMap<FollowDto, FollowModel>()
                .ReverseMap();
        }
        
        
    }
}
