using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Data;
using Thesis.data.Interfaces;
using static Thesis.app.Commands.AccountCommand;

namespace Thesis.app.Filters.AdminQueryFilters
{
    public class ClassFilter : IFIlter<Classroom>
    {
        public string ClassName { get; set; }
        public string ClassKey { get; set; }

        public DateTime? DateCreatedFrom { get; set; }
        public DateTime? DateCreatedTo { get; set; }

        public string TeacherName { get; set; } 
        public Expression<Func<Classroom, bool>> GetPredicate()
        {
            var predicate = PredicateBuilder.New<Classroom>(true);
                
            if (!string.IsNullOrEmpty(ClassName))
            {
                string classNameLower = ClassName.ToLower();
                predicate = predicate.And(classroom => classroom.ClassName != null && classroom.ClassName.ToLower().Contains(classNameLower));
            }

            if (!string.IsNullOrEmpty(ClassKey))
            {
                string classKeyLower = ClassKey.ToLower();
                predicate = predicate.And(classroom => classroom.ClassroomKey != null && classroom.ClassroomKey.ToLower().Contains(classKeyLower));
            }

            if (DateCreatedFrom.HasValue)
            {
                Expression<Func<Classroom, bool>> dateFromFilter =
                    entry => entry.DateCreated >= DateCreatedFrom.Value;

                predicate = predicate.And(dateFromFilter);
            }

            if (DateCreatedTo.HasValue)
            {
                Expression<Func<Classroom, bool>> dateToFilter =
                    entry => entry.DateCreated <= DateCreatedTo.Value;

                predicate = predicate.And(dateToFilter);
            }

            if (!string.IsNullOrEmpty(TeacherName))
            {
                string teacherNameLower = TeacherName.ToLower();
                predicate = predicate.And(classroom => classroom.Teacher.LastName != null && classroom.Teacher.LastName.ToLower().Contains(teacherNameLower));
            }

            return predicate;

        }
    }
}
