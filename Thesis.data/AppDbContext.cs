using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> config) : base(config)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<HomeWork> HomeWorks { get; set; }
        public DbSet<LearningPath> LearningPaths { get; set; }
        public DbSet<LearningPathExercises> LearningPathExercises { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationMessage> NotificationMessages { get; set; }
        public DbSet<StudentBadges> StudentBadges { get; set; }
        public DbSet<StudentExercises> StudentExercises { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<TimeBlocker> TimeBlockers { get; set; }

        public DbSet<StudentFilter> StudentFilters{ get; set; }     

        public DbSet<AccountLevel> AccountLevels { get; set; }
        public DbSet<AchievementStudents> AchievementStudents { get; set; }    


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.Property(p => p.Email)
                    .HasMaxLength(60);
                
                e.Property(p => p.Login)
                    .HasMaxLength(30);

                e.Property(p => p.FirstName)
                    .HasMaxLength(50);

                e.Property(p => p.LastName)
                    .HasMaxLength(50);

                e.Ignore(p => p.FullName);

                e.Property(e => e.PublicId)
             .HasDefaultValueSql("NEWID()");

                e.HasDiscriminator<string>("Role")
                    .HasValue<Student>("Student")
                    .HasValue<Teacher>("Teacher")
                    .HasValue<Parent>("Parent")
                    .HasValue<Admin>("Admin");

                e.HasMany(p => p.RecivedNotifications)
                .WithOne(p => p.UserTo)
                .HasForeignKey(p => p.UserToId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(p => p.SentNotifications)
               .WithOne(p => p.UserFrom)
               .HasForeignKey(p => p.UserFromId)
               .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(p => p.LoginHistory)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Student>(e =>
            {
                e.HasOne(p => p.Parent)
                .WithMany(p => p.Students)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(p => p.TimeBlocker)
                .WithOne(p => p.Student)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.StudentFilter)
                .WithOne(p => p.Student)
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Badge>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(200);
                e.Property(e => e.PublicId)
             .HasDefaultValueSql("NEWID()");

                e.Property(p => p.Emote).HasMaxLength(10);

            });

            modelBuilder.Entity<Achievement>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(200);
                e.Property(e => e.PublicId)
             .HasDefaultValueSql("NEWID()");

            });

            modelBuilder.Entity<StudentBadges>(e =>
            {
                e.HasKey(p => new { p.StudentId, p.BadgeId });
                    
                e.HasOne(p => p.Student)
                .WithMany(p => p.StudentBadges)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.Badge)
                .WithMany(p => p.StudentBadges)
                .HasForeignKey(p => p.BadgeId)
                .OnDelete(DeleteBehavior.Cascade);

                e.Property(p => p.IsSeen).HasDefaultValue(false);

            });

            modelBuilder.Entity<AchievementStudents>(e =>
            {
                e.HasKey(p => new { p.StudentId, p.AchievementId });

                e.HasOne(p => p.Student)
                .WithMany(p => p.AchievementStudents)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.Achievement)
                .WithMany(p => p.AchievementStudents)   
                .HasForeignKey(p => p.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<StudentExercises>(e =>
            {
                e.HasKey(p => new { p.StudentId, p.ExerciseId });

                e.HasOne(p => p.Student)    
                .WithMany(p => p.StudentExercises)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.Exercise)   
                .WithMany(p => p.StudentExercises)
                .HasForeignKey(p => p.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

                e.Property(p => p.IsCompleted).HasDefaultValue(false);
                e.Property(p => p.Attempts).HasDefaultValue(0);
                e.Property(p => p.WrongAnswers).HasDefaultValue(0);

            });

            modelBuilder.Entity<LearningPathExercises>(e =>
            {
                e.HasKey(p => new { p.LearningPathId, p.ExerciseId });

                e.HasOne(p => p.LearningPath)
                .WithMany(p => p.LearningPathExercises)
                .HasForeignKey(p => p.LearningPathId)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.Exercise)   
                .WithMany(p => p.LearningPathExercises)
                .HasForeignKey(p => p.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Notification>(e =>
            {
                e.HasOne(p => p.Message)
                .WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.Cascade);

                e.Ignore(p => p.TypeEnum);
                e.Property(e => e.PublicId)
             .HasDefaultValueSql("NEWID()");
                    
            });

            modelBuilder.Entity<NotificationMessage>(e =>
            {
                e.Property(e => e.PublicId)
            .HasDefaultValueSql("NEWID()");

            });

                modelBuilder.Entity<Subject>(e =>
            {
                e.HasMany(p => p.Exercises)
                .WithOne(p => p.Subject)
                .HasForeignKey(p => p.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

                e.Property(p => p.Name).HasMaxLength(100);
                e.Property(e => e.PublicId)
             .HasDefaultValueSql("NEWID()");
            });

            modelBuilder.Entity<Exercise>(e =>  
            {              
                e.HasOne(p => p.Answer)
                .WithOne(p => p.Exercise)
                .OnDelete(DeleteBehavior.Cascade);

                e.Ignore(p => p.Points);

                e.Ignore(p => p.LevelEnum);
                e.Property(e => e.PublicId)
             .HasDefaultValueSql("NEWID()");

            });

            modelBuilder.Entity<Answer>(e =>    
            {
                e.Property(p => p.CorrectText).HasMaxLength(150);
                e.Property(e => e.PublicId)
             .HasDefaultValueSql("NEWID()");

                e.Property(p => p.IncorrectOption1)
                .HasMaxLength(150);

                e.Property(p => p.IncorrectOption2)
                .HasMaxLength(150);

                e.Property(p => p.IncorrectOption3)
                .HasMaxLength(150);

                e.Property(p => p.CorrectOption)
              .HasMaxLength(150);

            });

            modelBuilder.Entity<LearningPath>(e =>
            {
                e.HasMany(p => p.Badges)
                .WithOne(p => p.LearningPath)
                .HasForeignKey(p => p.LearningPathId)
                .OnDelete(DeleteBehavior.Restrict);

                e.Ignore(p => p.EnumType);
                e.Property(e => e.PublicId)
             .HasDefaultValueSql("NEWID()");

                e.Property(p => p.Name)
                .HasMaxLength(100);

                e.HasOne(p => p.Subject)
                .WithMany(p => p.LearningPaths)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(p => p.SubjectId);

                e.HasOne(p => p.Teacher)
                .WithMany(p => p.LearningPaths)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            });


            modelBuilder.Entity<HomeWork>(e =>
            {
                e.HasOne(p => p.Classroom)
                .WithMany(p => p.HomeWorks)
                .HasForeignKey(p => p.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.Teacher)
                .WithMany(p => p.HomeWorks)
                .HasForeignKey(p => p.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

                e.HasMany(p => p.Exercises)
                .WithOne(p => p.HomeWork)
                .HasForeignKey(p => p.HomeWorkId)
                .OnDelete(DeleteBehavior.Cascade);

                e.Property(e => e.PublicId)
              .HasDefaultValueSql("NEWID()");

                e.Ignore(p => p.TypeEnum);

            });

            modelBuilder.Entity<Classroom>(e =>
            {
                e.HasMany(p => p.Students)
                .WithOne(p => p.Classroom)
                .HasForeignKey(p => p.ClassroomId)
                .OnDelete(DeleteBehavior.SetNull);

                e.HasMany(p => p.HomeWorks)
                .WithOne(p => p.Classroom)
                .HasForeignKey(p => p.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.Teacher)
                .WithMany(p => p.Classrooms)
                .HasForeignKey(p => p.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

                e.Property(e => e.PublicId)
             .HasDefaultValueSql("NEWID()");

                e.Property(p => p.ClassName).HasMaxLength(25);
                e.Property(p => p.ClassroomKey).HasMaxLength(30);

            });



        }
    }
}
