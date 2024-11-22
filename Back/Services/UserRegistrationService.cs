using CodeExcercises.Interfaces;
using System.Xml.Linq;

namespace CodeExcercises.Back.Services
{
    public class UserRegistrationService : IUserRegistration
    {
        private static int _lastId = 0;
        private const string FilePath = "CodeExcersises/Db/users.xml";
        private readonly ILogger<UserRegistrationService> _logger;
        private readonly IUserService _userService;

        public UserRegistrationService(ILogger<UserRegistrationService> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public (bool Success, string ErrorMessage) RegisterUser(IUser user)
        {
            if (user == null)
            {
                _logger.LogError("User cannot be null");
                return (false, "User cannot be null");
            }

            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.EmailAddress))
            {
                _logger.LogError("Username and Email Address are required");
                return (false, "Username and Email Address are required");
            }

            var users = _userService.GetUsers();

            if (users.Any(u => u.Username == user.Username))
            {
                _logger.LogError("Username already exists.");
                return (false, "Username already exists.");
            }

            if (users.Any(u => u.EmailAddress == user.EmailAddress))
            {
                _logger.LogError("Email address already exists.");
                return (false, "Email address already exists.");
            }

            if (users.Any(u => u.CellphoneNumber == user.CellphoneNumber))
            {
                _logger.LogError("Cellphone number already exists.");
                return (false, "Cellphone number already exists.");
            }

            user.Id = ++_lastId;

            try
            {
                // Ensure the directory exists
                string directory = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Check if the file exists
                if (!File.Exists(FilePath))
                {
                    // Create the file and add the root element
                    var doc = new XDocument(new XElement("Users"));
                    doc.Save(FilePath);
                }

                // Load the existing XML file
                var xdoc = XDocument.Load(FilePath);

                // Find the last ID
                var lastId = xdoc.Root.Elements("User")
                    .Select(x => (int)x.Element("Id"))
                    .DefaultIfEmpty(0)
                    .Max();

                // Increment the ID
                user.Id = lastId + 1;

                // Create a new XElement for the user
                var userElement = new XElement("User",
                    new XElement("Id", user.Id),
                    new XElement("Username", user.Username),
                    new XElement("Name", user.Name),
                    new XElement("Surname", user.Surname),
                    new XElement("EmailAddress", user.EmailAddress),
                    new XElement("CellphoneNumber", user.CellphoneNumber)
                );

                // Add the new user to the XML document
                xdoc.Root.Add(userElement);

                // Save the updated XML document
                xdoc.Save(FilePath);

                _logger.LogInformation($"User {user.Username} registered successfully");
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while registering the user: {ex.Message}");
                return (false, $"An error occurred while registering the user: {ex.Message}");
            }
        }
    }
}