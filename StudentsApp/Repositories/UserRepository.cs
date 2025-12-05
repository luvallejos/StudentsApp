using System;
using System.Threading.Tasks;

namespace StudentsApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> GetUserByIdAsync(int userId) 
        {
            throw new NotImplementedException();
        }

    }
}
