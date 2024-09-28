using FluentValidation;
using Library.Core.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Contracts.RequestValidation
{
    public class LoginRequestUserDtoValidator : AbstractValidator<LoginRequestUserDto>
    {
        public LoginRequestUserDtoValidator() 
        {

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Min password length is 6");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email form");
        }
    }
}
