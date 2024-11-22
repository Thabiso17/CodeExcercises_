namespace CodeExcercises.Interfaces
{
    public interface IUserRegistration
    {
        (bool Success, string ErrorMessage) RegisterUser(IUser user);
    }
}
