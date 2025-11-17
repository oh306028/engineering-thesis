using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Account
{
    public class AccountLoginModel
    {
        public string Login { get; set; }   
        public string Password { get; set; }    
    }


    public class AccountLoginModelValidator : AbstractValidator<AccountLoginModel>
    {   
        public AccountLoginModelValidator() 
        {
            RuleFor(p => p.Login).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Pole jest wymagane");

        }
    }
}
