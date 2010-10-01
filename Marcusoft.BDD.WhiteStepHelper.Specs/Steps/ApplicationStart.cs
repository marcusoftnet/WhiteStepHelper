using System.IO;
using System.Reflection;
using TechTalk.SpecFlow;

namespace Marcusoft.BDD.WhiteStepHelper.Specs.Steps
{
    [Binding]
    public class ApplicationStart : WhiteStepBase
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filePath = Path.Combine(filePath, TestConfig.ApplicationUnderTestPath);
            
            StartApplicationUnderTest(filePath, TestConfig.MainWindowName);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            SUT.CloseAndSaveState();
        }

    }
}
