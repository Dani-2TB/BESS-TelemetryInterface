using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Models;

public class ConfigBess{
    public Guid IdConfig {get; set; }
    public Bess IdBess { get; set; }
    public int MaxDCCurrent {get; set; }
    public int MinDCCurrent {get; set; }


}