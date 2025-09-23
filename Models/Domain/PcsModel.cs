using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table("PCS_MODEL")]
public class PcsModel
{
    public int Id { get; set; }

    [Required]
    public int RatedPower { get; set; }

    [Required]
    public int VoltageMaxDc { get; set; }

    [Required]
    public int VoltageMinDc { get; set; }

    [Required]
    public int CurrentMaxDc { get; set; }

    [Required]
    public string Name { get; set; } = "noname";
}
