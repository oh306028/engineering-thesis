using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data;

namespace Thesis.app.Dtos.Account
{
    public class TeacherAccountRegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }


    public class TeacherAccountRegisterModelValidator : AbstractValidator<TeacherAccountRegisterModel>
    {
        public AppDbContext dbContext { get; set;}
        public TeacherAccountRegisterModelValidator(AppDbContext context)
        {
            dbContext = context;

            RuleFor(p => p.Login).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.File).NotEmpty().WithMessage("Pole jest wymagane");

            RuleFor(p => p.Email)
             .EmailAddress()
             .Custom((email, context) =>
             {
                 if (dbContext.Users.Any(u => u.Email == email))
                     context.AddFailure("Istnieje już taki email");
             });

            RuleFor(p => p.Login)
             .Custom((login, context) =>
             {
                 if (dbContext.Users.Any(u => u.Login == login))
                     context.AddFailure("Istnieje już taki login");

             });

            RuleFor(p => p.Password).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.ConfirmPassword).NotEmpty().WithMessage("Pole jest wymagane");

            RuleFor(p => p.Password).Equal(p => p.ConfirmPassword).WithMessage("Hasła muszą się zgadzać");
        }
    }
}
