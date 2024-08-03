using System;
using UserManagement.Enums;

namespace UserManagement.Models;

public class Log : BaseEntity
{
    public long EntityId { get; set; }
    public string EntityName { get; set; } = default!;
    public LogType Type { get; set; }
    public DateTime Date { get; set; }
    public string? PropertyName { get; set; }
    public string? OriginalValue { get; set; }
    public string? CurrentValue { get; set; }
}
