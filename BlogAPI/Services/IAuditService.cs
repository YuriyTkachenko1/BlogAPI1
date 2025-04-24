namespace BlogAPI.Services
{
    public interface IAuditService
    {
        Task LogAsync(string action, int entityId);
    }
}
