using AutoMapper;
using Thesis.api.Extensions;
using Thesis.app.Dtos.Answer;
using Thesis.app.Dtos.Badge;
using Thesis.app.Dtos.Exercise;
using Thesis.app.Dtos.Notification;
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
            CreateMap<Badge, BadgeDetails>();


            CreateMap<Notification, NotificationList>()
                .ForMember(p => p.NotificationType, m => m.MapFrom(p => p.TypeEnum.GetDescription()))
                .ForMember(p => p.NotifiedBy, m => m.MapFrom(p => !p.IsSystemNotification ? p.UserFrom.FullName : "Powiadomienie systemowe"));


            CreateMap<Notification, NotificationDetails>()
                .ForMember(p => p.NotificationType, m => m.MapFrom(p => p.TypeEnum.GetDescription()))
                .ForMember(p => p.NotifiedBy, m => m.MapFrom(p => !p.IsSystemNotification ? p.UserFrom.FullName : "Powiadomienie systemowe"))
                .ForMember(p => p.NotificationDate, m => m.MapFrom(p => p.DateCreated.ToString("g")))
                .ForMember(p => p.Message, m => m.MapFrom(p => p.Message.Name));



        }
    }
}
