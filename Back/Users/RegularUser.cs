using CodeExcercises.Interfaces;

namespace CodeExcercises.Back.Users
{
    public class RegularUser : IUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string CellphoneNumber { get; set; }
        public DateTime SubscriptionEndDate { get; set; } // Additional property specific to RegularUser
    }
}
