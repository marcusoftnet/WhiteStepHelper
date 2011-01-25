using White.Core;
using White.Core.AutomationElementSearch;
using White.Core.Factory;
using White.Core.UIItems.WindowItems;

namespace Marcusoft.BDD.WhiteStepHelper
{
    /// <summary>
    /// Extension methods for the White application class
    /// </summary>
    public static class WhiteApplicationExtensions
    { 

        /// <summary>
        /// Returns true if the application is still running
        /// </summary>
        /// <param name="instance">the application we're testing</param>
        /// <returns>true if the application is running</returns>
        public static bool IsRunning(this Application instance)
        {
            return instance != null && instance.HasExited == false;
        }

        /// <summary>
        /// Close and save state for the application
        /// </summary>
        /// <param name="instance">the application instance</param>
        public static void CloseAndSaveState(this Application instance)
        {
            if (instance.IsRunning())
            {
                instance.KillAndSaveState();
            }
        }

        public static Window GetWindowByTitle(this Application instance, string title)
        {
            Window w; 
            try
            {
                w = instance.GetWindow(title, InitializeOption.WithCache.AndIdentifiedBy(title));
            }
            catch
            {
                var message = string.Format("Cannot find a window with the title '{0}' in the application '{1}'", title, instance.Name);
                throw new AutomationElementSearchException(message);
            }

            return w;
        }
    }
}
