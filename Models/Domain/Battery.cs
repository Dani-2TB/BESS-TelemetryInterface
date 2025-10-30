using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table("BATTERY")]
public class Battery
{
    public int Id { get; set; }

<<<<<<< HEAD
    [Required, DisplayName("Modbus ID")]
    public int ModbusId { get; set; }

    [Required]
=======
    [Required, MaxLength(120)]
>>>>>>> 3826126 (feat: added model validations)
    public string Name { get; set; } = "noname";

    [Required, DisplayName("Max Soc Threshold"), Range(0, 100)]
    public int SocMax { get; set; }

    [Required, DisplayName("Min Soc Threshold"), Range(0,100)]
    public int SocMin { get; set; }

    [Required, DisplayName("Max Current"), Range(0,sizeof(UInt32))]
    public int CurrentMax { get; set; }

    [Required, DisplayName("Max Voltage"), Range(0,sizeof(UInt32))]
    public int VoltageMax { get; set; }

    [Required, DisplayName("Min Voltage"), Range(0,sizeof(UInt32))]
    public int VoltageMin { get; set; }

<<<<<<< HEAD
    [Required, DisplayName("Voltage Absorption")]
    public int VoltageAbsorption { get; set; }
=======
    [Required, DisplayName("Voltage Absorption"), Range(0,sizeof(UInt32))]
    public int VotageAbsorption { get; set; }
>>>>>>> 3826126 (feat: added model validations)

    [Required, DisplayName("Current Charging"), Range(0,sizeof(UInt32))]
    public int CurrentCharging { get; set; }

    [Required, DisplayName("Max Power"), Range(0,sizeof(UInt32))]
    public int PwrMax { get; set; }

    [Required, Column(name: "BESS_id"), DisplayName("Bess Configuration")]
    public int BessId { get; set; }
    public Bess? Bess { get; set; }
}
