using AutoMapper;
using CourseApi.Entities;
using CourseApi.Services.DailyChoices.Dtos;

namespace CourseApi.MapProfiles
{
    public class DailyChoiceProfile : Profile
    {
        public DailyChoiceProfile()
        {
            CreateMap<DailyChoice, DailyChoiceDto>();
            CreateMap<DailyChoiceDto, DailyChoice>();
        }
    }
}