using UnityEngine;

namespace Interfaces.Services
{
    public class LoggingService : ILoggingService
    {
        public void LogInfo(string message)
        {
            Debug.Log(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}