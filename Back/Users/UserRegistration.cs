using CodeExcercises.Interfaces;

namespace CodeExcercises.Back.Users
{
    public class UserRegistration //: IUserRegistration
    {
        private static int _lastId = 0;
        public void RegisterUser(IUser user)
        {
            // Implementation for registering a user
            // For example, saving user details to an XML file
            user.Id = _lastId++;
            Console.WriteLine($"User {user.Username} registered successfully.");
        }
    }
}
