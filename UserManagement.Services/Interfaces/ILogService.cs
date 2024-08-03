using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
    /// <summary>
    /// Return a log
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Log?> Get(long id);

    /// <summary>
    /// Return logs for an entity
    /// Optional filtering by date
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    Task<IEnumerable<Log>> GetForEntity<TEntity>(TEntity entity, DateTime? fromDate = null, DateTime? toDate = null) where TEntity : BaseEntity;

    /// <summary>
    /// Return all logs
    /// Optional filtering by date
    /// </summary>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    Task<IEnumerable<Log>> GetAll(DateTime? fromDate = null, DateTime? toDate = null);
}
