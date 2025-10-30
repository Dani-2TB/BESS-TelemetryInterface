using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table("PCS_MODEL")]
public class PcsModel
{
    public int Id { get; set; }

    [Required, Range(0, 255)]
    public int ModbusId { get; set; }

    [Required, DisplayName("Rated Power"), Range(0,sizeof(UInt32))]
    public int RatedPower { get; set; }

    [Required, DisplayName("Max Voltage DC"), Range(0,sizeof(UInt32))]
    public int VoltageMaxDc { get; set; }

    [Required, DisplayName("Min Voltage DC"), Range(0,sizeof(UInt32))]
    public int VoltageMinDc { get; set; }

    [Required, DisplayName("Max Current DC"), Range(0,sizeof(UInt32))]
    public int CurrentMaxDc { get; set; }

    [Required, MaxLength(130)]
    public string Name { get; set; } = "noname";
}
