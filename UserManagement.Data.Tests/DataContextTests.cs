using System;
using System.Linq;
using UserManagement.Models;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    [Fact]
    public async void GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com",
            DateOfBirth = new DateTime(1997, 07, 19)
        };
        await context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public async void GetAll_WhenNewEntityAdded_MustIncludeNewLog()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "Brand New 002",
            Surname = "User002",
            Email = "brandnewuser002@example.com",
            DateOfBirth = new DateTime(1997, 07, 20)
        };

        entity = await context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<Log>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().Contain(s => s.EntityId == entity.Id);
    }

    [Fact]
    public async void GetAll_WhenUpdated_MustIncludeUpdatedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var testGuid = Guid.NewGuid().ToString();

        var entity = context.GetAll<User>().First();
        entity.Forename = testGuid;

        await context.Update(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();
        var updatedEntity = result.First();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().Contain(s => s.Forename == testGuid);
    }

    [Fact]
    public async void GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = context.GetAll<User>().First();
        await context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);
    }

    private DataContext CreateContext() => new();
}
