using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace AeroDynasty
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Called when the application starts
        protected override void OnStartup(StartupEventArgs e)
        {
            // Set up Trace listeners
            Trace.Listeners.Add(new ConsoleTraceListener());   // Logs to the console
            Trace.Listeners.Add(new TextWriterTraceListener("log.txt")); // Logs to a file
            Trace.AutoFlush = true; // Ensures that the log is written immediately

            // Log that the application is starting
            Trace.WriteLine("Application Starting...");

            try
            {
                base.OnStartup(e);

                // Example usage of logging
                Trace.WriteLine(DateTime.Now.ToString() + " - AeroDynasty initialized successfully.");
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Application encountered an error: {ex.Message}");
            }
        }

        // Optional: Handle app exit logging
        protected override void OnExit(ExitEventArgs e)
        {
            Trace.WriteLine(DateTime.Now.ToString() + " - AeroDynasty finished.");
            base.OnExit(e);
        }
    }

}
