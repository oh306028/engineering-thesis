using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Events
{
    public class CreatedHomeWorkEvent : INotification
    {
        public int ClassroomId { get; set; }
        public CreatedHomeWorkEvent(int classroomId)
        {
            ClassroomId = classroomId;
        }
    }
}
