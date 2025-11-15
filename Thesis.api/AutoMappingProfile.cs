using AutoMapper;
using Thesis.api.Extensions;
using Thesis.api.Modules.Answer.Details;
using Thesis.api.Modules.Answer.Update;
using Thesis.api.Modules.Exercise.Details;
using Thesis.data.Data;

namespace Thesis.api
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Exercise, ExerciseDetails>()
             .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LevelEnum.GetDescription()))
             .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject.Name))
             .ForMember(dest => dest.LearningPath, opt => opt.MapFrom(src =>
                 src.LearningPathExercises
                    .Select(lp => lp.LearningPath.EnumType.GetDescription())
                    .ToList()
             ));


            CreateMap<Answer, AnswerDetails>();

        }
    }
}
