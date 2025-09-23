using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table(name: "BESS")]
public class Bess
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = "noname";
    [Required]
    public int CurrentMaxAcIn { get; set; }
    [Required]
    public int CurrentMaxAcOut { get; set; }
    [Required, Column(name: "OPERATION_MODE_id")]
    public int OperationModeId { get; set; }
    public OperationMode? OperationMode { get; set; }
}