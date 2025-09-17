namespace Adaptor.Logging
{
    /// <summary>
    /// Interface for logging in the application
    /// </summary>
    public interface ILogger
    {
       
        void LogInfo(string message);
      
        void LogWarning(string message);
     
        void LogError(string message);
       
        void LogSuccess(string message);
    }
    
 
    public class ConsoleLogger : ILogger
    {
       
        public void LogInfo(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
       
        public void LogWarning(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
        
        public void LogError(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
       
        public void LogSuccess(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
    }
}