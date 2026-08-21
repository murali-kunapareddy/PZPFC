using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using PFCRepository.Repositories.Common.Enums;
using PFCRepository.Repositories.Common.Interfaces;
using PFCRepository.Repositories.Common.Models;
using PFCRepository.Utilities;

namespace PFCRepository.Repositories.Common.ServiceProviders
{
    public class LoggingProvider : ILoggingProvider
    {

        private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        private static readonly string canLogWithNLogger = AppConfig.CanLogWithNLogger;
        LogMessageEntity objLogMessageEntity;
        private string jsonString;
        private string UserSESA;

        /// <summary>
        /// LoggingProvider
        /// </summary>
        /// <param name="contextAccessor"></param>
        public LoggingProvider()
        {
            objLogMessageEntity = new LogMessageEntity();
            UserSESA = null ;

        }


        private void SetNlogLogLevel(NLog.LogLevel level)
        {
            string MiniumLogLevel = "Trace";
            try
            {
                //MiniumLogLevel = _objCommonProvider.GetAppSettingByName(Constants.MiniumLogLevel);
            }
            catch (Exception ex)
            {
            }

            if (level == NLog.LogLevel.Off)
            {
                LogManager.DisableLogging();
            }
            else
            {
                if (!LogManager.IsLoggingEnabled())
                {
                    LogManager.EnableLogging();
                }

                foreach (var rule in LogManager.Configuration.LoggingRules)
                {
                    // Iterate over all levels up to and including the target, (re)enabling them.
                    for (int i = level.Ordinal; i <= 5; i++)
                    {
                        rule.EnableLoggingForLevel(NLog.LogLevel.FromOrdinal(i));
                    }
                }
            }

            LogManager.ReconfigExistingLoggers();
        }



        /// <summary>
        /// LogMessage
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        public void LogMessage(LogType logLevel, string message)
        {
            try
            {

                if (canLogWithNLogger == "1")
                {
                    switch (logLevel)
                    {
                        case LogType.Info:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Info(jsonString);
                            break;
                        case LogType.Warn:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Warn(jsonString);
                            break;
                        case LogType.Fatal:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Fatal(jsonString);
                            break;
                        case LogType.Error:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Error(jsonString);
                            break;
                        case LogType.Debug:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Debug(jsonString);
                            break;
                        case LogType.Trace:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Trace(jsonString);
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// LogMessage with object
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public void LogMessage(LogType logLevel, string message, params object[] args)
        {
            try
            {
                if (canLogWithNLogger == "1")
                {
                    switch (logLevel)
                    {
                        case LogType.Info:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Info(jsonString, args);
                            break;
                        case LogType.Warn:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Warn(jsonString, args);
                            break;
                        case LogType.Fatal:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Fatal(jsonString, args);
                            break;
                        case LogType.Error:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Error(jsonString, args);
                            break;
                        case LogType.Debug:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Debug(jsonString, args);
                            break;
                        case LogType.Trace:
                            objLogMessageEntity.message = message;
                            objLogMessageEntity.userSESA = GetUserSESA();
                            jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                            logger.Trace(jsonString, args);
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// CaptureElapsedTime
        /// </summary>
        /// <param name="method"></param>
        /// <param name="elapsedtime"></param>
        public void LogElapsedTime(string method, string elapsedtime)
        {
            try
            {
                if (canLogWithNLogger == "1")
                {
                    method = "UserSESA(" + GetUserSESA() + "): " + method;
                    logger.Info("Method: " + method + ", ExecutionTime: " + elapsedtime + " s");
                }
            }
            catch (Exception)
            {

            }

        }


        /// <summary>
        /// LogException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void LogException(string message, Exception exception)
        {
            try
            {
                objLogMessageEntity.message = message;
                objLogMessageEntity.userSESA = GetUserSESA();
                jsonString = JsonConvert.SerializeObject(objLogMessageEntity);
                logger.Error(exception, jsonString);
            }
            catch (Exception)
            {

            }
        }


        public string GetUserSESA()
        {
            try
            {
                return PFCApiRequestDetails.CreatedBy;

            }
            catch (Exception)
            {
                return "-";
            }

        }



        



        /// <summary>
        /// LogMessageEntity
        /// </summary>
        public class LogMessageEntity
        {
            public string userSESA { get; set; }
            public string message { get; set; }

        }


        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }




    }
}
