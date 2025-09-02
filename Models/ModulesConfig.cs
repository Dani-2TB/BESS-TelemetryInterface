using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Models;

public class ConfigBess{
    public Guid Id {get; set; }
    public required Module ModuleId { get; set; }
    public int MaxDCCurrent {get; set; }
    public int MinDCCurrent {get; set; }


}