using System;

namespace BlogAPI.Services
{
    public class AuditService : IAuditService
    {
        private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "audit-log.txt");
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task LogAsync(string action, int entityId, string? user = null)
        {
            var httpUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var userInfo = string.IsNullOrEmpty(user) ? (string.IsNullOrEmpty(httpUser) ? "UnknownUser" : httpUser) : user;
            var logEntry = $"[{DateTime.UtcNow:u}] User: {userInfo}, Action: {action}, Entity ID: {entityId}{Environment.NewLine}";
            return File.AppendAllTextAsync(_filePath, logEntry);
        }
    }
}
