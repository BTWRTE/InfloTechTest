using System;
using UserManagement.Enums;

namespace UserManagement.Web.Models.Logging;

public class LogListViewModel
{
    public IEnumerable<LogViewModel> Items { get; set; } = [];
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class LogViewModel
{
    public long Id { get; set; }
    public long EntityId { get; set; }
    public string EntityName { get; set; } = default!;
    public LogType Type { get; set; }
    public string TypeName { get; set; } = default!;
    public DateTime Date { get; set; }
    public string? PropertyName { get; set; }
    public string? OriginalValue { get; set; }
    public string? CurrentValue { get; set; }
}

public class UserLogViewModel
{
    public string? Fullname { get; set; }
    public bool IsActive { get; set; }
    public string TypeName { get; set; } = default!;
    public DateTime Date { get; set; }
    public string? PropertyName { get; set; }
    public string? OriginalValue { get; set; }
    public string? CurrentValue { get; set; }
}
