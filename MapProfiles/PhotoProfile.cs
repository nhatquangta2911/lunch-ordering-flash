using AutoMapper;
using CourseApi.Entities;
using CourseApi.Services.PhotoServices.Dtos;

namespace CourseApi.MapProfiles
{
    public class PhotoProfile : Profile
    {
        public PhotoProfile()
        {
            CreateMap<Photo, PhotoDto>();
            CreateMap<PhotoDto, Photo>();
        }
    }
}