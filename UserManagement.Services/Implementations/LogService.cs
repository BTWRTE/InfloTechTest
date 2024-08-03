using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return a log
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Log?> Get(long id)
    {
        return await _dataAccess
            .GetAll<Log>()
            .SingleOrDefaultAsync(l => l.Id == id);
    }

    /// <summary>
    /// Return logs for an entity
    /// Optional filtering by date
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Log>> GetForEntity<TEntity>(TEntity entity,
        DateTime? fromDate = null, DateTime? toDate = null)
        where TEntity : BaseEntity
    {
        return await _dataAccess
            .GetAll<Log>()
            .Where(l => l.EntityName == typeof(TEntity).Name
                && l.EntityId == entity.Id
                && (fromDate == null || l.Date >= fromDate)
                && (toDate == null || l.Date <= toDate))
            .ToArrayAsync();
    }

    /// <summary>
    /// Return all logs
    /// Optional filtering by date
    /// </summary>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Log>> GetAll(DateTime? fromDate = null, DateTime? toDate = null)
    {
        return await _dataAccess
            .GetAll<Log>()
            .Where(l => (fromDate == null || l.Date >= fromDate)
                && (toDate == null || l.Date <= toDate))
            .ToArrayAsync();
    }
}
