using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Domain;

[Table(name: "AUDIT_LOG")]
public class AuditLog
{
    public Guid Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string LogLevel { get; set; }
    public string Category { get; set; }
    public string Message { get; set; }
    public string? EntityType { get; set; }
    public string? EntityId { get; set; }
    public string? Changes { get; set; }
    public string? Detail { get; set; }
}