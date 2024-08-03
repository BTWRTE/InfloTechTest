using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Data;

public interface IDataContext
{
    /// <summary>
    /// Get a list of items
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    IQueryable<TEntity> GetAll<TEntity>() where TEntity : BaseEntity;

    /// <summary>
    /// Return an item by Id
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity?> Get<TEntity>(long id) where TEntity : BaseEntity;

    /// <summary>
    /// Create a new item
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> Create<TEntity>(TEntity entity) where TEntity : BaseEntity;

    /// <summary>
    /// Uodate an existing item matching the ID
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> Update<TEntity>(TEntity entity) where TEntity : BaseEntity;

    Task Delete<TEntity>(TEntity entity) where TEntity : BaseEntity;
}
