using System;
using System.ComponentModel.DataAnnotations;
using UserManagement.Web.Models.Logging;

namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public IEnumerable<UserListItemViewModel> Items { get; set; } = [];
}

public class EditUserViewModel
{
    public UserListItemViewModel User { get; set; } = new();
    public LogListViewModel Logs { get; set; } = new();
    public bool IsEditable { get; set; }
}

public class UserListItemViewModel
{
    public long? Id { get; set; }
    [Required]
    public string? Forename { get; set; }
    [Required]
    public string? Surname { get; set; }
    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    [Required]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }
}
