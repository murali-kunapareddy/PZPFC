using PFCRepository.Repositories.Common.Enums;

namespace PFCRepository.Repositories.Common.Interfaces
{
    public interface ILoggingProvider : IDisposable
    {
        /// <summary>
        /// LogMessage
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        void LogMessage(LogType logLevel, string message);

        /// <summary>
        /// LogMessage with object
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogMessage(LogType logLevel, string message, params object[] args);

        /// <summary>
        /// LogException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void LogException(string message, Exception exception);

        /// <summary>
        /// LogElapsedTime
        /// </summary>
        /// <param name="method"></param>
        /// <param name="elapsedtime"></param>
        void LogElapsedTime(string method, string elapsedtime);

    }
}
