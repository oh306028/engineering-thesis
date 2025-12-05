using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Exercise;
using Thesis.data.Enums;

namespace Thesis.app.Dtos.HomeWork
{
    public class HomeWorkModel
    {
        public HomeWorkType Type { get; set; }
        public List<ExerciseModel> Exercises{ get; set; }
        public DateTime? DeadLine { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; } 
    }

    public class HomeWorkModelValidator : AbstractValidator<HomeWorkModel>
    {
        public HomeWorkModelValidator()
        {
            RuleFor(p => p.Exercises).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.DeadLine).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.Description).NotEmpty().WithMessage("Pole jest wymagane");
            RuleFor(p => p.Subject).NotEmpty().WithMessage("Pole jest wymagane");
        }
    }
}
