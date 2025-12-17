using System.Linq.Expressions;
using Thesis.data.Data;
using Thesis.data.Interfaces;
using LinqKit;


namespace Thesis.app.Filters.AdminQueryFilters
{

    public class UsersFilter : IFIlter<User>
    {
        public string Role { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Expression<Func<User, bool>> GetPredicate()
        {
            var predicate = PredicateBuilder.New<User>(true);

            if (!string.IsNullOrEmpty(Role))
            {
                predicate = predicate.And(user => user.Role == Role);
            }

            if (!string.IsNullOrEmpty(Login))
            {
                string loginLower = Login.ToLower();
                predicate = predicate.And(user => user.Login != null && user.Login.ToLower().Contains(loginLower));
            }

            if (!string.IsNullOrEmpty(Email))
            {
                string emailLower = Email.ToLower();
                predicate = predicate.And(user => user.Email != null && user.Email.ToLower().Contains(emailLower));
            }

            if (!string.IsNullOrEmpty(FirstName))
            {
                string firstNameLower = FirstName.ToLower();
                predicate = predicate.And(user => user.FirstName != null && user.FirstName.ToLower().Contains(firstNameLower));
            }
            if (!string.IsNullOrEmpty(LastName))
            {
                string lastNameLower = LastName.ToLower();
                predicate = predicate.And(user => user.LastName != null && user.LastName.ToLower().Contains(lastNameLower));
            }

            return predicate;
        }
    }
}
