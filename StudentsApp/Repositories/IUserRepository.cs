using System.Threading.Tasks;

namespace StudentsApp.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);

    }
}