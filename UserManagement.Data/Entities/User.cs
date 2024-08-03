using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models;

public class User : BaseEntity
{
    public string Forename { get; set; } = default!;
    public string Surname { get; set; } = default!;
    [Display(Name = "Email Address")]
    public string Email { get; set; } = default!;
    [Display(Name = "Is This User Active?")]
    public bool IsActive { get; set; }
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }
}
