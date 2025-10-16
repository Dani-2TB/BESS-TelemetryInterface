using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table("BATTERY")]
public class Battery
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "noname";

    [Required]
    public int SocMax { get; set; }

    [Required]
    public int SocMin { get; set; }

    [Required]
    public int CurrentMax { get; set; }

    [Required]
    public int VoltageMax { get; set; }

    [Required]
    public int VoltageMin { get; set; }

    [Required]
    public int VotageAbsorption { get; set; }

    [Required]
    public int CurrentCharging { get; set; }

    [Required]
    public int PwrMax { get; set; }

    [Required, Column(name: "BESS_id")]
    public int BessId { get; set; }
    public Bess? Bess { get; set; }
}
