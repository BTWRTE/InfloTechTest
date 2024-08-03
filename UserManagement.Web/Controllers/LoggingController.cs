using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logging;

namespace UserManagement.WebMS.Controllers;

[Route("Logging")]
public class LoggingController : Controller
{
    private readonly ILogService _logService;
    private readonly IUserService _userService;
    public LoggingController(ILogService logService, IUserService userService)
    {
        _logService = logService;
        _userService = userService;
    }

    [HttpGet]
    [Route("List")]
    public async Task<ViewResult> List(DateTime? fromDate = null, DateTime? toDate = null)
    {
        fromDate ??= DateTime.Now.Date.AddDays(-14);
        toDate ??= DateTime.Now.Date;
        toDate = new DateTime(toDate.Value.Year, toDate.Value.Month, toDate.Value.Day, 23, 59, 59);

        var items = (await _logService
            .GetAll(fromDate, toDate))
            .Select(l => new LogViewModel
            {
                Id = l.Id,
                EntityId = l.EntityId,
                EntityName = l.EntityName,
                Type = l.Type,
                TypeName = l.Type.ToString(),
                Date = l.Date,
                PropertyName = l.PropertyName,
                OriginalValue = l.OriginalValue,
                CurrentValue = l.CurrentValue
            });

        var model = new LogListViewModel
        {
            Items = items.ToList(),
            FromDate = fromDate.Value,
            ToDate = toDate.Value
        };

        return View(model);
    }

    [HttpGet]
    [Route("ViewLog/{id}")]
    public async Task<ViewResult> ViewLog(long id)
    {
        var log = await _logService
            .Get(id);

        if (log != null)
        {
            var user = await _userService
                .Get(log.EntityId);

            return View("LogView", new UserLogViewModel
            {
                Fullname = user?.Forename + " " + user?.Surname,
                IsActive = user?.IsActive ?? false,
                TypeName = log.Type.ToString(),
                Date = log.Date,
                PropertyName = log.PropertyName,
                OriginalValue = log.OriginalValue,
                CurrentValue = log.CurrentValue
            });
        }
        else
        {
            return View("Error");
        }
    }
}
