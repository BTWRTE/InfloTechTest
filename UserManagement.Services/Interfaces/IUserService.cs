using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return all users
    /// Optional filtering by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task<IEnumerable<User>> GetAllAsync(bool? isActive = null);

    /// <summary>
    /// Return all users
    /// Optional filtering by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<User> GetAll(bool? isActive = null);

    /// <summary>
    /// Return a user by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User?> Get(long id);

    /// <summary>
    /// Add a new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<User> Create(User user);

    /// <summary>
    /// Update a user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<User> Update(User user);

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task Delete(User user);
}
