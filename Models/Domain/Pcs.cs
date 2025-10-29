using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table("PCS")]
public class Pcs
{
    public int Id { get; set; }

    public int ModbusId { get; set; }

    [Required, Column(name: "BATTERY_id"), DisplayName("Battery")]
    public int BatteryId { get; set; }
    public Battery? Battery { get; set; }

    [Required, Column(name: "PCS_MODEL_id"), DisplayName("PCS Model")]
    public int PcsModelId { get; set; }
    public PcsModel? PcsModel { get; set; }
}
