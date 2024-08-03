using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public async void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(null);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async void ViewUser_WhenServiceReturnsUser_ModelMustEqualUserAndLogs()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();
        var logs = SetupLogs(users);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.ViewUser(0);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<EditUserViewModel>()
            .Which.User.Should().BeEquivalentTo(users[0]);

        result.Model
            .Should().BeOfType<EditUserViewModel>()
            .Which.Logs.Items.Should().BeEquivalentTo(logs);
    }

    [Fact]
    public async void EditUser_WhenServiceReturnsUser_ModelMustEqualUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.EditUser(0);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<EditUserViewModel>()
            .Which.User.Should().BeEquivalentTo(users[0]);
    }

    [Fact]
    public async void EditUser_ControllerCreatesUser_ModelMustNotEqualUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.EditUser(null);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<EditUserViewModel>()
            .Which.User.Should().NotBeEquivalentTo(users[0]);
    }

    [Fact]
    public async void SetUser_WhenServiceReturnsUser_ModelMustEqualUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller
            .SetUser(new EditUserViewModel
            {
                User = new UserListItemViewModel
                {
                    Id = users[0].Id,
                    Forename = users[0].Forename,
                    Surname = users[0].Surname,
                    Email = users[0].Email,
                    DateOfBirth = users[0].DateOfBirth,
                    IsActive = users[0].IsActive
                }
            });

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<EditUserViewModel>()
            .Which.User.Should().BeEquivalentTo(users[0]);
    }

    [Fact]
    public async void Delete_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.Delete(users[0].Id);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true, DateTime dateOfBirth = default)
    {
        var users = new[]
        {
            new User
            {
                Id = 0,
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = dateOfBirth
            }
        };

        _userService
            .Setup(s => s.GetAllAsync(null))
            .Returns(Task.Run(() => (IEnumerable<User>)users));

        _userService
            .Setup(s => s.Get(0))
            .Returns(Task.Run(() => (User?)users[0]));

        _userService
            .Setup(s => s.Create(users[0]))
            .Returns(Task.Run(() => users[0]));

        return users;
    }

    private Log[] SetupLogs(User[]  users, DateTime date = default)
    {
        var logs = new[]
        {
            new Log
            {
                EntityId = 0,
                EntityName = "User",
                Type = LogType.Create,
                Date = date
            }
        };

        _logService
            .Setup(s => s.GetForEntity(users[0], null, null))
            .Returns(Task.Run(() => (IEnumerable<Log>)logs));

        return logs;
    }

    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<ILogService> _logService = new();
    private UsersController CreateController()
    {
        return new(_userService.Object, _logService.Object);
    }
}
