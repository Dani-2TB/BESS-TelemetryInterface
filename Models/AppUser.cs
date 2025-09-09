using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/*
namespace DotnetAPI.Models;

public class User
{
    public Guid Id { get; set; }
    [StringLength(30)]
    public required string Username { get; set; }
    [StringLength(30), MinLength(8)]
    public required string Password { get; set; }
    public required string Email { get; set; }
}*/

namespace AuthApi.Models;
public class AppUser
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(64)]
    public string UserName { get; set; } = default!;

    [Required, EmailAddress, MaxLength(256)]
    public string Email { get; set; } = default!;

    [Required]
    public string PasswordHash { get; set; } = default!;
}
