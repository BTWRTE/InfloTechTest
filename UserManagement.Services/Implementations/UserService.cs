using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return all users
    /// Optional filtering by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public async Task<IEnumerable<User>> GetAllAsync(bool? isActive = null)
    {
        return await _dataAccess
            .GetAll<User>()
            .Where(u => isActive == null
                || u.IsActive == isActive)
            .ToArrayAsync();
    }

    /// <summary>
    /// Return all users
    /// Optional filtering by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> GetAll(bool? isActive = null)
    {
        return _dataAccess
            .GetAll<User>()
            .Where(u => isActive == null
                || u.IsActive == isActive)
            .ToArray();
    }

    /// <summary>
    /// Return a user by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User?> Get(long id)
    {
        return await _dataAccess
            .Get<User>(id);
    }

    /// <summary>
    /// Add a new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<User> Create(User user)
    {
        return await _dataAccess
            .Create(user);
    }

    /// <summary>
    /// Update a user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<User> Update(User user)
    {
        return await _dataAccess
            .Update(user);
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task Delete(User user)
    {
        await _dataAccess
            .Delete(user);
    }
}
