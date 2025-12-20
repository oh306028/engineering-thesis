using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Game
{
    public class GameDetails
    {
        public int QuestionsCount { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers => QuestionsCount - CorrectAnswers;
    }
}
