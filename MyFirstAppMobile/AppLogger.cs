using MyFirstAppMobile.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAppMobile
{
    internal class AppLogger : IAppLogger
    {
        IPlatformLogger _platformLogger;

        private string _logPath;
        private readonly object _lock = new object();

        public AppLogger(IPlatformLogger platformlog) {
            _platformLogger = platformlog;
            _logPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.log");
        }
        public void Crit(Exception? ex, string message) => Write("CRITICAL", message, ex);

        public void Info(string message) => Write("INFO", message, null);

        public void Warn(Exception ex, string message) => Write("WARNING", message, ex);

        private void Write(string level, string message, Exception? ex)
        {

            var line = $"{DateTime.UtcNow:o} [{level}] {message} {ex}\n";

            _platformLogger.Log(line);

            lock(_lock)
            {                 
                System.IO.File.AppendAllText(_logPath, line);
            }
        }
    }
}
