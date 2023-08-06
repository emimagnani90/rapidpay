using DataAccess.Repositories.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {

        }

        public User AddUser(User user)
        {
            _context.Users.Add(user);
            return user;
        }


        public User? GetByUserName(string userName)
        {
            var user = _context.Users.Where(x => x.UserName == userName).FirstOrDefault();
            return user;
        }
    }
}
