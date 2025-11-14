using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class LoginHistory : IEntity
    {
        public int Id { get; set; }

        public DateTime LoginDate { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public bool IsSucceeded { get; set; }   
    }   
}
