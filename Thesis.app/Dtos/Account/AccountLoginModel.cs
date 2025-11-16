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
        public string Email { get; set; }   
        public string Password { get; set; }    
    }


    public class AccountLoginModelValidator : AbstractValidator<AccountLoginModel>
    {
        public AccountLoginModelValidator() 
        {
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Password).NotEmpty();

        }
    }
}
