using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table("BATTERY")]
public class Battery
{
    public int Id { get; set; }

    [Required, DisplayName("Modbus ID")]
    public int ModbusId { get; set; }

    [Required]
    public string Name { get; set; } = "noname";

    [Required, DisplayName("Max Soc Threshold")]
    public int SocMax { get; set; }

    [Required, DisplayName("Min Soc Threshold")]
    public int SocMin { get; set; }

    [Required, DisplayName("Max Current")]
    public int CurrentMax { get; set; }

    [Required, DisplayName("Max Voltage")]
    public int VoltageMax { get; set; }

    [Required, DisplayName("Min Voltage")]
    public int VoltageMin { get; set; }

    [Required, DisplayName("Voltage Absorption")]
    public int VoltageAbsorption { get; set; }

    [Required, DisplayName("Current Charging")]
    public int CurrentCharging { get; set; }

    [Required, DisplayName("Max Power")]
    public int PwrMax { get; set; }

    [Required, Column(name: "BESS_id"), DisplayName("Bess Configuration")]
    public int BessId { get; set; }
    public Bess? Bess { get; set; }
}
