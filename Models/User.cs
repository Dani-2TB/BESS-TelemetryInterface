using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Models;

public class User 
{
    public Guid Id { get; set; }
    [StringLength(30)]
    public required string Username { get; set; }
    [StringLength(30), MinLength(8)]
    public required string Password { get; set; }
    public required string Email { get; set; }
}