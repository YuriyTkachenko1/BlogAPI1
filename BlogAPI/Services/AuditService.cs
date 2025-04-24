namespace BlogAPI.Services
{
    public class AuditService : IAuditService
    {
        private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "audit-log.txt");

        public Task LogAsync(string action, int entityId)
        {
            var logEntry = $"[{DateTime.UtcNow:u}] Action: {action}, Entity ID: {entityId}{Environment.NewLine}";
            return File.AppendAllTextAsync(_filePath, logEntry);
        }
    }
}
