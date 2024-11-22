using CodeExcercises.Back.Services;
using CodeExcercises.Interfaces;
using CodeExcercises.Models.Users;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly Mock<ILogger<UserService>> _loggerMock;

    public UserServiceTests()
    {
        _loggerMock = new Mock<ILogger<UserService>>();
        _userService = new UserService(_loggerMock.Object);
        _userService.ClearUsers(); // Clear the XML file before each test
    }

    [Fact]
    public void RegisterUser_ValidUser_AddsUser()
    {
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Name = "Test",
            Surname = "User",
            EmailAddress = "testuser@example.com",
            CellphoneNumber = "1234567890"
        };

        _userService.SaveUser(user);
        var users = _userService.GetUsers();

        Assert.Contains(users, u => u.Username == "testuser");
    }

    [Fact]
    public void RegisterUser_DuplicateUsername_ThrowsException()
    {
        var user1 = new User
        {
            Id = 1,
            Username = "testuser",
            Name = "Test",
            Surname = "User",
            EmailAddress = "testuser1@example.com",
            CellphoneNumber = "1234567890"
        };

        var user2 = new User
        {
            Id = 2,
            Username = "testuser",
            Name = "Test",
            Surname = "User",
            EmailAddress = "testuser2@example.com",
            CellphoneNumber = "0987654321"
        };

        _userService.SaveUser(user1);

        var result = _userService.SaveUser(user2);
        Assert.False(result.Success);
        Assert.Equal("Username already exists.", result.ErrorMessage);
    }

    [Fact]
    public void RegisterUser_DuplicateEmail_ThrowsException()
    {
        var user1 = new User
        {
            Id = 1,
            Username = "testuser1",
            Name = "Test",
            Surname = "User",
            EmailAddress = "testuser@example.com",
            CellphoneNumber = "1234567890"
        };

        var user2 = new User
        {
            Id = 2,
            Username = "testuser2",
            Name = "Test",
            Surname = "User",
            EmailAddress = "testuser@example.com",
            CellphoneNumber = "0987654321"
        };

        _userService.SaveUser(user1);

        var result = _userService.SaveUser(user2);
        Assert.False(result.Success);
        Assert.Equal("Email address already exists.", result.ErrorMessage);
    }

    [Fact]
    public void RegisterUser_DuplicateCellphoneNumber_ThrowsException()
    {
        var user1 = new User
        {
            Id = 1,
            Username = "testuser1",
            Name = "Test",
            Surname = "User",
            EmailAddress = "testuser1@example.com",
            CellphoneNumber = "1234567890"
        };

        var user2 = new User
        {
            Id = 2,
            Username = "testuser2",
            Name = "Test",
            Surname = "User",
            EmailAddress = "testuser2@example.com",
            CellphoneNumber = "1234567890"
        };

        _userService.SaveUser(user1);

        var result = _userService.SaveUser(user2);
        Assert.False(result.Success);
        Assert.Equal("Cellphone number already exists.", result.ErrorMessage);
    }

    [Fact]
    public void EditUser_ShouldUpdateUserDetails()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Name = "Test", Surname = "User", EmailAddress = "test@example.com", CellphoneNumber = "1234567890" };
        _userService.SaveUser(user);

        var updatedUser = new User { Id = 1, Username = "updateduser", Name = "Updated", Surname = "User", EmailAddress = "updated@example.com", CellphoneNumber = "0987654321" };

        // Act
        var result = _userService.EditUser(updatedUser);

        // Assert
        Assert.True(result.Success);
        var users = _userService.GetUsers();
        var editedUser = users.FirstOrDefault(u => u.Id == updatedUser.Id);
        Assert.NotNull(editedUser);
        Assert.Equal("updateduser", editedUser.Username);
        Assert.Equal("Updated", editedUser.Name);
        Assert.Equal("User", editedUser.Surname);
        Assert.Equal("updated@example.com", editedUser.EmailAddress);
        Assert.Equal("0987654321", editedUser.CellphoneNumber);
    }

    [Fact]
    public void DeleteUser_ShouldRemoveUser()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Name = "Test", Surname = "User", EmailAddress = "test@example.com", CellphoneNumber = "1234567890" };
        _userService.SaveUser(user);

        // Act
        var result = _userService.DeleteUser(user.Id);

        // Assert
        Assert.True(result.Success);
        var users = _userService.GetUsers();
        Assert.DoesNotContain(users, u => u.Id == user.Id);
    }
}