namespace CodeExcercises.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }
        string Username { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        string EmailAddress { get; set; }
        string CellphoneNumber { get; set; }

    }
}
