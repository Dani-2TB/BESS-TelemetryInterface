using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table(name: "BESS")]
public class Bess
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = "noname";
    [Required, DisplayName("AC Current Input Max (A)")]
    public int CurrentMaxAcIn { get; set; }
    [Required, DisplayName("AC Current Output Max (A)")]
    public int CurrentMaxAcOut { get; set; }
    [Required, Column(name: "OPERATION_MODE_id"), DisplayName("Operation Mode")]
    public int OperationModeId { get; set; }
    public OperationMode? OperationMode { get; set; }

}