using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UserManagement.Enums;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<User>().HasData(
        [
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = new DateTime(1978, 06, 22) },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = new DateTime(1997, 08, 31) },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false, DateOfBirth = new DateTime(1962, 02, 09) },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true, DateOfBirth = new DateTime(1964, 05, 29) },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true, DateOfBirth = new DateTime(2001, 01, 13) },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true, DateOfBirth = new DateTime(1967, 08, 31) },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false, DateOfBirth = new DateTime(1990, 05, 18) },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false, DateOfBirth = new DateTime(2003, 04, 04) },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false, DateOfBirth = new DateTime(1976, 01, 04) },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true, DateOfBirth = new DateTime(1963, 02, 20) },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true, DateOfBirth = new DateTime(1985, 12, 19) },
        ]);

        model.Entity<Log>().HasData([]);
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Log>? Logs { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : BaseEntity
        => base.Set<TEntity>();

    public async Task<TEntity?> Get<TEntity>(long id) where TEntity : BaseEntity
    {
        return await base
            .Set<TEntity>()
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<TEntity> Create<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        entity.Id = await GetNextId<TEntity>();

        base.Add(entity);
        await SaveChangesAsync();

        return entity;
    }

    public async new Task<TEntity> Update<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        base.Update(entity);
        await SaveChangesAsync();

        return entity;
    }

    public async Task Delete<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        base.Remove(entity);
        await SaveChangesAsync();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentTime = DateTime.UtcNow;
        var nextId = await GetNextId<Log>();

        var changes = ChangeTracker
            .Entries()
            .Where(e => e.State != EntityState.Unchanged)
            .ToArray();

        foreach (var change in changes)
        {
            var entityEntry = Entry(change.Entity);

            if (change.State == EntityState.Added)
            {
                base.Add(new Log
                {
                    Id = nextId++,
                    EntityId = ((BaseEntity)entityEntry.Entity).Id,
                    EntityName = entityEntry.Metadata.DisplayName(),
                    Type = LogType.Create,
                    Date = currentTime
                });
            }
            else if (change.State == EntityState.Modified)
            {
                foreach (var property in entityEntry.OriginalValues.Properties)
                {
                    var original = entityEntry.OriginalValues[property.Name];
                    var current = entityEntry.CurrentValues[property.Name];

                    if (!Equals(original, current))
                    {
                        base.Add(new Log
                        {
                            Id = nextId++,
                            EntityId = ((BaseEntity)entityEntry.Entity).Id,
                            EntityName = entityEntry.Metadata.DisplayName(),
                            Type = LogType.Update,
                            Date = currentTime,
                            PropertyName = GetPropertyName(property),
                            OriginalValue = original?.ToString(),
                            CurrentValue = current?.ToString()
                        });
                    }
                }
            }
            else if (change.State == EntityState.Deleted)
            {
                base.Add(new Log
                {
                    Id = nextId++,
                    EntityId = ((BaseEntity)entityEntry.Entity).Id,
                    EntityName = entityEntry.Metadata.DisplayName(),
                    Type = LogType.Delete,
                    Date = currentTime
                });
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task<long> GetNextId<TEntity>() where TEntity : BaseEntity
    {
        var ids = await base
            .Set<TEntity>()
            .Select(e => e.Id)
            .ToArrayAsync();

        if (!ids.Any())
        {
            return 1;
        }

        return ids.Max() + 1;
    }

    private string GetPropertyName(IProperty property)
    {
        var displayAttribute = property.PropertyInfo?.CustomAttributes
            .SingleOrDefault(ca => ca.AttributeType == typeof(DisplayAttribute));

        if (displayAttribute != null)
        {
            var displayNameArgument = displayAttribute.NamedArguments
                .Single(cana => cana.MemberName == "Name");

            return displayNameArgument.TypedValue.Value?.ToString() ?? property.Name;
        }

        return property.Name;
    }
}
