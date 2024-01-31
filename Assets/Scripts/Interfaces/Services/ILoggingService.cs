namespace Interfaces.Services
{
    public interface ILoggingService {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}