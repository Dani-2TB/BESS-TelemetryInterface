using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table("PCS_MODEL")]
public class PcsModel
{
    public int Id { get; set; }

    [Required, DisplayName("Rated Power")]
    public int RatedPower { get; set; }

    [Required, DisplayName("Max Voltage DC")]
    public int VoltageMaxDc { get; set; }

    [Required, DisplayName("Min Voltage DC")]
    public int VoltageMinDc { get; set; }

    [Required, DisplayName("Max Current DC")]
    public int CurrentMaxDc { get; set; }

    [Required]
    public string Name { get; set; } = "noname";
}
