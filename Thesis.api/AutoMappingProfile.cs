using AutoMapper;
using Thesis.api.Extensions;
using Thesis.api.Resolvers;
using Thesis.app.Dtos.Admin;
using Thesis.app.Dtos.Answer;
using Thesis.app.Dtos.Badge;
using Thesis.app.Dtos.Classroom;
using Thesis.app.Dtos.Exercise;
using Thesis.app.Dtos.HomeWork;
using Thesis.app.Dtos.LearningPath;
using Thesis.app.Dtos.Notification;
using Thesis.app.Dtos.Student;
using Thesis.app.Queries;
using Thesis.app.Resolvers;
using Thesis.data.Data;
using Thesis.data.Enums;

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
                    .First()
             ));

            CreateMap<Achievement, AchievementDetails>();
            CreateMap<Exercise, ExerciseHomeWorkDetails>();
            CreateMap<Answer, AnswerDetails>();   
            CreateMap<Badge, BadgeDetails>();
            CreateMap<User, UserListModel>();

            CreateMap<HomeWork, HomeworkDetails>()
                .ForMember(p => p.Type, o => o.MapFrom(p => p.TypeEnum.GetDescription()))
                .ForMember(p => p.PublicId, o => o.MapFrom(p => p.PublicId.ToString()));

            CreateMap<Student, StudentDetails>()
                .ForMember(p => p.Level, o => o.MapFrom(p => p.AccountLevel.Level))
                .ForMember(p => p.Name, o => o.MapFrom(p => p.FullName))
                .ForMember(p => p.BadgesCount, o => o.MapFrom(p => p.CountBadges()))
                .ForMember(p => p.LastSeenAt, o => o.MapFrom<StudentLastLoginResolver>());


            CreateMap<Teacher, TeacherAttemptListModel>()
                .ForMember(p => p.IsAccepted, o => o.MapFrom(p => p.IsAccepted.HasValue ? p.IsAccepted.Value : false))
                .ForMember(p => p.PublicId, o => o.MapFrom(p => p.PublicId.ToString()));


            CreateMap<Classroom, ClassroomListModel>()
                .ForMember(p => p.TeacherName, o => o.MapFrom(p => p.Teacher.FullName));

            CreateMap<LoginHistory, LogginHistoryListModel>()
                .ForMember(p => p.Login, o => o.MapFrom(p => p.User.Login));


            CreateMap<Exercise, PathExercise>()
                .ForMember(p => p.PublicId, o => o.MapFrom(p => p.PublicId.ToString()))
                .ForMember(p => p.IsCompleted, o => o.MapFrom<PathExerciseResolver>());


            CreateMap<LearningPath, LearningPathDetails>()
                .ForMember(p => p.Type, o => o.MapFrom(p => p.EnumType.GetDescription()))
                .ForMember(p => p.PublicId, o => o.MapFrom(p => p.PublicId.ToString()));

            CreateMap<Classroom, ClassroomDetails>()
                .ForMember(p => p.TeacherName, o => o.MapFrom(p => p.Teacher.FullName))
                .ForMember(p => p.PublicId, o => o.MapFrom(p => p.PublicId.ToString()))
                .ForMember(p => p.ClassroomKey, o => o.MapFrom<ClassroomKeyResolver>())
                .ForMember(p => p.TeacherPublicId, o => o.MapFrom(p => p.Teacher.PublicId.ToString()));

            CreateMap<Classroom, ClassroomList>()
                .ForMember(p => p.PublicId, o => o.MapFrom(p => p.PublicId.ToString()));

            CreateMap<NotificationMessage, MessageType>()
                .ForMember(p => p.Message, o => o.MapFrom(p => p.Name))
                .ForMember(p => p.MessageId, o => o.MapFrom(p => p.PublicId.ToString()));

            CreateMap<Notification, NotificationList>()
                .ForMember(p => p.NotificationType, m => m.MapFrom(p => p.TypeEnum.GetDescription()))
                .ForMember(p => p.NotifiedBy, m => m.MapFrom(p => !p.IsSystemNotification ? p.UserFrom.FullName : "Powiadomienie systemowe"));


            CreateMap<Notification, NotificationDetails>()
                .ForMember(p => p.NotificationType, m => m.MapFrom(p => p.TypeEnum.GetDescription()))
                .ForMember(p => p.NotifiedBy, m => m.MapFrom(p => !p.IsSystemNotification ? p.UserFrom.FullName : "Powiadomienie systemowe"))
                .ForMember(p => p.NotificationDate, m => m.MapFrom(p => p.DateCreated.ToString("g")))
                .ForMember(p => p.Message, m => m.MapFrom(p => p.Message.Name));


            CreateMap<Student, StudentDetailsWithClassroom>()
                .IncludeBase<Student, StudentDetails>()
                .ForMember(p => p.TeacherPublicId, o => o.MapFrom(p => p.Classroom.Teacher.PublicId.ToString()))
                .ForMember(p => p.TeacherName, o => o.MapFrom(p => p.Classroom.Teacher.FullName));

            CreateMap<HomeWorkType, HomeWorkTypePair>()
                .ForMember(p => p.Key, o => o.MapFrom(p => p.ToString()))
                .ForMember(p => p.Value, o => o.MapFrom(p => p.GetDescription()));

        }
    }
}
