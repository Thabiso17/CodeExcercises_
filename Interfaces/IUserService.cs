using CodeExcercises.Models.Users;

namespace CodeExcercises.Interfaces
{
    public interface IUserService
    {
        List<User> GetUsers();
        (bool Success, string ErrorMessage) SaveUser(User user);
        void SaveUsers(List<User> users);
        void ClearUsers() { }
        (bool Success, string ErrorMessage) DeleteUser(int userId);
        (bool Success, string ErrorMessage) EditUser(User user);

    }
}
