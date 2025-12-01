using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Events
{
    public class LogInEvent : INotification
    {
        public int StudentId { get; set; }
        public DateTime LogInDate { get; set; }
        public LogInEvent(int studentId, DateTime logInDate)
        {
            StudentId = studentId;
            LogInDate = logInDate;
        }
    }
}
