namespace WM.Domain.Entities;

public class AuditLog
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public String EntityType { get; private set; } = String.Empty;
    public int EntityId { get; private set; }
    public String Action { get; private set; } = String.Empty;
    public String? OldValues { get; private set; }
    public String? NewValues { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private AuditLog() { }

    public AuditLog(int userId, String entityType, int entityId, String action, String? oldValues, String? newValues)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.", nameof(userId));

        if (String.IsNullOrWhiteSpace(entityType))
            throw new ArgumentException("Entity type is required.", nameof(entityType));

        if (entityId <= 0)
            throw new ArgumentException("Entity ID must be greater than zero.", nameof(entityId));

        if (String.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action is required.", nameof(action));

        UserId = userId;
        EntityType = entityType.Trim();
        EntityId = entityId;
        Action = action.Trim();
        OldValues = oldValues;
        NewValues = newValues;
        CreatedAt = DateTime.UtcNow;
    }
}
