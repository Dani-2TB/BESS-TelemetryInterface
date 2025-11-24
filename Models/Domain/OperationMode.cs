using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Models.Domain;

public class OperationMode
{
    public int Id { get; set; }
    [Required, MaxLength(30)]
    public string Name { get; set; } = "noname";
}