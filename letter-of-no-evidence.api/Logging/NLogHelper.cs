using NLog;
using NLog.Targets;

namespace letter_of_no_evidence.api.Logging
{
    public static class NLogHelper
    {
        public static Logger ConfigureLogger()
        {
            LogFactory logFactory = NLog.Web.NLogBuilder.ConfigureNLog("nLog.config");

            SetNLogSqlTarget();
            LogManager.ConfigurationReloaded += (sender, e) =>
            {
                //Re apply if config reloaded
                SetNLogSqlTarget();
            };
            return logFactory.GetCurrentClassLogger();
        }

        private static void SetNLogSqlTarget()
        {
            string nlogSqlConnectionString = Environment.GetEnvironmentVariable("NLOG_SQL_CONNECTION") ?? string.Empty;

            if (String.IsNullOrEmpty(nlogSqlConnectionString))
            {
                throw new ApplicationException("NLog SQL connection string must be provided via the NLOG_SQL_CONNECTION environment variable.");
            }

            var configuration = LogManager.Configuration;
            var targets = configuration.AllTargets;

            DatabaseTarget databaseTarget = (DatabaseTarget)targets.First(t => t.GetType() == typeof(DatabaseTarget));
            databaseTarget.ConnectionString = nlogSqlConnectionString;
            LogManager.Configuration = configuration;
        }
    }
}
