using System.Threading.Tasks;
using zwajApp.API.Models;

namespace zwajApp.API.Data
{
    public interface IAuthRepository
    {
         Task<Users> Register (Users users,string password);
         Task<Users> Login (string username,string password);
         Task<bool> UserExists(string username);
    }
}