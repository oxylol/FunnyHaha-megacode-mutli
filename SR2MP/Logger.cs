using MelonLoader;
using MelonLoader.Utils;
using SR2E.Managers;

namespace SR2MP
{
    public static class Logger
    {
        private static readonly MelonLogger.Instance melonLogger;
        internal static string logPath;

        static Logger()
        {
            melonLogger = new MelonLogger.Instance("SR2MP");
            
            string folderPath = Path.Combine(MelonEnvironment.UserDataDirectory, "SR2MP");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            
            logPath = Path.Combine(folderPath, "latest.log");
            
            try
            {
                File.WriteAllText(logPath, $"{DateTime.Now} [SR2MP] Log Initialized!");
            }
            catch (Exception ex)
            {
                melonLogger.Error($"Failed to initialize log file: {ex}");
            }
        }

        public static void Log(string message) => Log(message, 100);

        public static void Log(string message, int sr2eSize)
        {
            SR2ELogManager.SendMessage($"<size={sr2eSize}%>{message}</size>");
            melonLogger.Msg(message);
            WriteToFile("[INFO]", message);
        }

        public static void Warn(string message)
        {
            SR2ELogManager.SendWarning(message);
            melonLogger.Warning(message);
            WriteToFile("[WARNING]", message);
        }

        public static void Error(string message)
        {
            SR2ELogManager.SendError(message);
            melonLogger.Error(message);
            WriteToFile("[ERROR]", message);
        }

        public static void Debug(string message)
        {
            WriteToFile("[DEBUG]", message);
        }

        private static void WriteToFile(string level, string message)
        {
            try
            {
                string line = $"{DateTime.Now:HH:mm:ss} {level} {message}";
                File.AppendAllText(logPath, "\n" + line);
            }
            catch (Exception ex)
            {
                melonLogger.Error($"Failed to write to log file: {ex}");
            }
        }
    }
}
