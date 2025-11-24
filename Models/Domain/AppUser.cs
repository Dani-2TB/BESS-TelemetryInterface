using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DotnetAPI.Models.Domain;
[Table(name: "APP-USER")]
public class AppUser : IdentityUser<Guid>
{
    [Required, Range(1000000, 99999999)]
    public int Rut { get; set; }
    public string Dv { get; set; } = "";
    public string? NombreCompleto { get; set; }
    public string? Cargo { get; set; }
}