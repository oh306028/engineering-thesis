using MediatR;
using Thesis.data.Data;

namespace Thesis.app.Events
{
    public class PointsAddedEvent : INotification
    {
        public int StudentId { get; }
        public int AddedPoints { get; }
        public int TotalPoints { get; }

        public PointsAddedEvent(int studentId, int addedPoints, int totalPoints)
        {
            StudentId = studentId;  
            AddedPoints = addedPoints;
            TotalPoints = totalPoints;
        }
    }
}
