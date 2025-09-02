using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Models;

public class ConfigBess{
    public Guid Id {get; set; }

    public required Guid ModuleId { get; set; }
    public Module Module { get; set; } = null!;

    public int MaxDCCurrent {get; set; }
    public int MinDCCurrent {get; set; }
}