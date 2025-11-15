using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.api.Extensions;
using Thesis.api.Modules.Answer.Update;
using Thesis.app.Exceptions;
using Thesis.data;
using Thesis.data.Interfaces;
using static Thesis.app.Commands.ExerciseCommand;

namespace Thesis.app.Commands
{
    public class ExerciseCommand
    {
        public class Answer : IRequest<Thesis.data.Data.Answer>
        {
            public AnswerModel Model{ get; set; }
            public string ExercisePublicId { get; set; }        
            public Answer(string publicId, AnswerModel model)
            {
                Model = model;
                ExercisePublicId = publicId;
            }
        }
    }


    public class AnswerHandler : IRequestHandler<ExerciseCommand.Answer, Thesis.data.Data.Answer>, IHandler
    {
        public AppDbContext DbContext { get; set; }

        public AnswerHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Thesis.data.Data.Answer> Handle(Answer request, CancellationToken cancellationToken)
        {
            var exercise = await DbContext.Exercises
                .WithAllIncludes()
                .SingleAsync(p => p.PublicId.ToString() == request.ExercisePublicId);

            var answer = exercise.Answer;

            if (request.Model.CorrectOption.HasValue && request.Model.CorrectOption != answer?.CorrectOption)
                throw new WrongAnswerException("Answer is incorrect");

            if (!string.IsNullOrEmpty(request.Model.CorrectText) && request.Model.CorrectText != answer.CorrectText)
                throw new WrongAnswerException("Answer is incorrect");

            if (request.Model.CorrectOption.HasValue && request.Model.CorrectOption != answer?.CorrectOption)
                throw new WrongAnswerException("Answer is incorrect");

            return answer;

        }
    }

}
