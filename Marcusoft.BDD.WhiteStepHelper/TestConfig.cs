using System.Configuration;

namespace Marcusoft.BDD.WhiteStepHelper
{
    /// <summary>
    /// Reads configuration properties from .config
    /// </summary>
    /// <remarks></remarks>
    public static class TestConfig
    {
        public static string ApplicationUnderTestPath
        {
            get { return ConfigurationManager.AppSettings["ApplicationUnderTestPath"]; }
        }

        public static string MainWindowName
        {
            get { return ConfigurationManager.AppSettings["MainWindowName"]; }
        }
    }
}
