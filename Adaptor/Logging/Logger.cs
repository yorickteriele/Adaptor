namespace Adaptor.Logging
{
    /// <summary>
    /// Interface for logging in the application
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogInfo(string message);
        
        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogWarning(string message);
        
        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogError(string message);
        
        /// <summary>
        /// Log a success message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogSuccess(string message);
    }
    
    /// <summary>
    /// Implementation of ILogger that logs to the console with colors
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Log an informational message in white
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogInfo(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
        
        /// <summary>
        /// Log a warning message in yellow
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogWarning(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
        
        /// <summary>
        /// Log an error message in red
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogError(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
        
        /// <summary>
        /// Log a success message in green
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogSuccess(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
    }
}