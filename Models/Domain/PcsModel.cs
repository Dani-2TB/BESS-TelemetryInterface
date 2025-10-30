using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table("PCS_MODEL")]
public class PcsModel
{
    public int Id { get; set; }

<<<<<<< HEAD
    [Required, DisplayName("Rated Power")]
    public int RatedPower { get; set; }

    [Required, DisplayName("Max Voltage DC")]
    public int VoltageMaxDc { get; set; }

    [Required, DisplayName("Min Voltage DC")]
    public int VoltageMinDc { get; set; }

    [Required, DisplayName("Max Current DC")]
=======
    [Required, DisplayName("Rated Power"), Range(0,sizeof(UInt32))]
    public int RatedPower { get; set; }

    [Required, DisplayName("Max Voltage DC"), Range(0,sizeof(UInt32))]
    public int VoltageMaxDc { get; set; }

    [Required, DisplayName("Min Voltage DC"), Range(0,sizeof(UInt32))]
    public int VoltageMinDc { get; set; }

    [Required, DisplayName("Max Current DC"), Range(0,sizeof(UInt32))]
>>>>>>> 3826126 (feat: added model validations)
    public int CurrentMaxDc { get; set; }

    [Required, MaxLength(130)]
    public string Name { get; set; } = "noname";
}
