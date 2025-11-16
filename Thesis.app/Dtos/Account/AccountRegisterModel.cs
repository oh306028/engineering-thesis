using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data;

namespace Thesis.app.Dtos.Account
{
    public class AccountRegisterModel
    {
        public string ParentFirstName { get; set; } 
        public string ParentLastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } 

        public DateTime? StudentDateOfBirth { get; set; }   

        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; } 
    }

    public class AccountRegisterModelValidator : AbstractValidator<AccountRegisterModel>
    {
        public AppDbContext dbContext { get; set; } 
        public AccountRegisterModelValidator(AppDbContext context)
        {
            dbContext = context;

            RuleFor(p => p.ParentFirstName).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.ParentLastName).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.StudentFirstName).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.StudentLastName).NotEmpty().WithMessage("Pole jest wymagane");

            RuleFor(p => p.Email)
             .NotEmpty()
             .EmailAddress()
             .MustAsync(async (email, context) =>
             {  
                 return !await dbContext.Users.AnyAsync(u => u.Email == email, context);
             })
             .WithMessage("Email jest zajęty");

            RuleFor(p => p.Password).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.ConfirmPassword).NotEmpty().WithMessage("Pole jest wymagane");

            RuleFor(p => p.Password).Equal(p => p.ConfirmPassword).WithMessage("Hasła muszą się zgadzać");
        }
    }
}
