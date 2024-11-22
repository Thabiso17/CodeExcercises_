using CodeExcercises.Interfaces;
using CodeExcercises.Models.Users;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace CodeExcercises.Back.Services
{
    public class UserService : IUserService
    {
        private readonly string _filePath = "CodeExcersises/Db/users.xml";
        private readonly ILogger _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public List<User> GetUsers()
        {
            if (!File.Exists(_filePath))
            {
                _logger.LogWarning("User file not found, returning an empty list.");
                return new List<User>();
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                using (FileStream fs = new FileStream(_filePath, FileMode.Open))
                {
                    return (List<User>)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reading the user file.");
                return new List<User>();
            }
        }

        public (bool Success, string ErrorMessage) SaveUser(User user)
        {
            if (user == null)
            {
                _logger.LogError("User cannot be null.");
                return (false, "User cannot be null.");
            }

            List<User> users = GetUsers();

            if (users.Any(u => u.CellphoneNumber == user.CellphoneNumber))
            {
                _logger.LogError("Cellphone number already exists.");
                return (false, "Cellphone number already exists.");
            }

            if (users.Any(u => u.EmailAddress == user.EmailAddress))
            {
                _logger.LogError("Email address already exists.");
                return (false, "Email address already exists.");
            }

            if (users.Any(u => u.Username == user.Username))
            {
                _logger.LogError("Username already exists.");
                return (false, "Username already exists.");
            }

            users.Add(user);
            SaveUsers(users);
            return (true, null);
        }

        public void SaveUsers(List<User> users)
        {
            if (users == null)
            {
                _logger.LogError("Users list cannot be null.");
                throw new ArgumentNullException(nameof(users), "Users list cannot be null");
            }

            try
            {
                string directory = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                using (FileStream fs = new FileStream(_filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, users);
                }
                _logger.LogInformation("Users saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving users.");
                throw new IOException("An error occurred while saving users", ex);
            }
        }

        // Method to clear the XML file
        public void ClearUsers()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                    _logger.LogInformation("User file cleared successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while clearing the user file.");
                throw new IOException("An error occurred while clearing the user file", ex);
            }
        }

        public (bool Success, string ErrorMessage) DeleteUser(int userId)
        {
            List<User> users = GetUsers();
            var userToDelete = users.FirstOrDefault(u => u.Id == userId);
            if (userToDelete == null)
            {
                _logger.LogError("User not found.");
                return (false, "User not found.");
            }

            users.Remove(userToDelete);
            SaveUsers(users);
            return (true, null);
        }

        public (bool Success, string ErrorMessage) EditUser(User user)
        {
            if (user == null)
            {
                _logger.LogError("User cannot be null.");
                return (false, "User cannot be null.");
            }

            List<User> users = GetUsers();
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                _logger.LogError("User not found.");
                return (false, "User not found.");
            }

            // Update user details
            existingUser.Username = user.Username;
            existingUser.Name = user.Name;
            existingUser.Surname = user.Surname;
            existingUser.EmailAddress = user.EmailAddress;
            existingUser.CellphoneNumber = user.CellphoneNumber;

            SaveUsers(users); // Save the updated list to XML
            return (true, null);
        }
    }
}